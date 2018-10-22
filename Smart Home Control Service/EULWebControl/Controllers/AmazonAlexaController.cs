using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using SmartHomeWebControl.Models.Amazon;
using SmartHomeWebControl.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Extensions.OptionsModel;

namespace SmartHomeWebControl.Controllers
{
    [Route("api/[controller]")]
    public class AmazonAlexaController : Controller {
        private AppSettings siteSettings;
        private ICommandSenderHelper senderHelper;

        public AmazonAlexaController(IOptions<AppSettings> siteSettings, ICommandSenderHelper helper) {
            this.siteSettings = siteSettings.Value;
            senderHelper = helper;
        }

        [HttpPost]
        public IActionResult ReceiveCommand([FromBody]JObject obj) {
            JsonSerializer ser = new JsonSerializer();
            ser.Converters.Add(new AmazonRequestObjectConverter());
            AmazonRequest request = obj.ToObject<AmazonRequest>(ser);

            if (!IsUserAuthorized(request)) return new ObjectResult(CreateUnauthorizedResponse());

            if (request.request.type == "IntentRequest") {
                AmazonResponse response = CreateAuthorizedResponse();
                AmazonIntent intent = ((AmazonIntentRequest)request.request).intent;
                string responseStr;

                if (intent.name.StartsWith("QueryState")) {
                    responseStr = SendStateVariableQuery(intent);
                }
                else {
                    responseStr = SendCommand(request, ref response);
                }
                if (responseStr != null) {
                    response.response.outputSpeech.text = responseStr;
                }
                return new ObjectResult(response);
            }
            return new ObjectResult(CreateDidNotUnderstandResponse());
        }

        private string SendStateVariableQuery(AmazonIntent intent) {
            string[] splitStrs = intent.name.Split('_');
            if (splitStrs.Length != 3) {
                return null;
            }
            return senderHelper.GetDeviceStateVariable(splitStrs[1], splitStrs[2]);
        }

        private string SendCommand(AmazonRequest request, ref AmazonResponse response) {
            ReceivedCommand command = ConvertRequestToCommand(request, ref response);
            string str = null;
            if (command != null) {
                str = senderHelper.PushRemoteCommand(command, null);
            }
            return str;
        }

        #region Response Generation
        private AmazonResponse CreateAuthorizedResponse() {
            AmazonResponse response = CreateGenericResponse();
            response.response.outputSpeech.text = "Done";
            return response;
        }

        private AmazonResponse CreateUnauthorizedResponse() {
            AmazonResponse response = CreateGenericResponse();
            response.response.outputSpeech.text = "You are not authorized to use this service";
            return response;
        }

        private AmazonResponse CreateDidNotUnderstandResponse() {
            AmazonResponse response = CreateGenericResponse();
            response.response.outputSpeech.text = "I am sorry, I did not understand this command. Please try again.";
            return response;
        }

        private AmazonResponse CreateGenericResponse() {
            AmazonResponse response = new AmazonResponse();
            response.version = "1.0";
            response.response = new AmazonResponseObject();
            response.response.outputSpeech = new AmazonOutputSpeech();
            response.response.outputSpeech.type = "PlainText";
            response.response.shouldEndSession = true;
            return response;
        }

        #endregion

        private bool IsUserAuthorized(AmazonRequest request) {
            if (request.session.user.accessToken == siteSettings.AmazonAccessToken) {
                return true;
            }
            return false;
        }

        private ReceivedCommand ConvertRequestToCommand(AmazonRequest request, ref AmazonResponse response) {
            AmazonIntent intent = ValidateIfRequestIsComplete(ref request, ref response);

            if (intent == null) return null;

            //Now we need to merge parameters for date and time. We also need to assign the correct
            //parameter type which can then be interpreted by the service.
            //At the moment this assumes that every time will have a date as well, but you may have a date
            //with no time
            intent.AssignParameterTypeToSlotValues();

            //The aim is to hard code as little as possible. <VOICE> prefix tells the service to look for
            //a voice prompt instead of a technical command name.
            //PAR prefix on the intent slot tells us that it needs to be a parameter to the command, 
            //not a part of the voice prompt
            return intent.ConvertIntentToCommand();
        }

        private AmazonIntent ValidateIfRequestIsComplete(ref AmazonRequest request, ref AmazonResponse response) {
            AmazonIntent intent = ((AmazonIntentRequest)request.request).intent;
            if (!intent.name.Contains("Incomplete") && !intent.name.Contains("FollowUp")) {
                intent.RemoveNullValueSlots();
                List<RequiredSlotWithPrompt> reqSlots = siteSettings.FindRequiredSlotsByIntent(intent.name);
                if (reqSlots != null) {
                    intent.SortSlots(reqSlots);
                }
                return intent;
            } else {
                RequiredSlotWithPrompt slWithPrompt = DetermineNextMissingSlot(ref request, ref response);
                if (slWithPrompt != null) {
                    response.response.outputSpeech.text = "OK. " + slWithPrompt.VoicePrompt;
                    response.response.shouldEndSession = false;
                } else {
                    return intent;
                }
            }
            return null;
        }

        private RequiredSlotWithPrompt DetermineNextMissingSlot(ref AmazonRequest request, ref AmazonResponse response) {
            AmazonIntent intent = ((AmazonIntentRequest)request.request).intent;

            //First we determine the list of required slots
            //We also need to remove the suffix
            List<RequiredSlotWithPrompt> reqSlots = siteSettings.FindRequiredSlotsByIntent(intent.name);

            //This is an incomplete request - i.e. first one issued
            //It will tell us all the other parameters we need to determine
            //First we add them all to session attributes on the response
            if (intent.name.Contains("Incomplete")) {
                //Slots seem to come in a completely random order so we need to arrange them before passing them
                //on
                intent.SortSlots(reqSlots);
            
                return response.CreateAttributesFromSlots(intent.slots, reqSlots); 
            }

            //This is a follow up request so will come with the value for next parameter(s)
            if (intent.name.Contains("FollowUp")) {
                foreach (KeyValuePair<string, AmazonSlot> kv in intent.slots) {
                    string searchStr = kv.Key.Insert(3, request.session.attributes["NextSlot"].ToString());
                    if (request.session.attributes[searchStr] == null) {
                        request.session.attributes[searchStr] = kv.Value.value;
                    }
                }
            }

            //Now we need to sort the session attributes as it appears Amazon randomizes them again :(
            request.session.SortSessionAttributes(reqSlots);

            //Check if all parameters are now present
            RequiredSlotWithPrompt missingSlot = request.session.CheckIfAllSlotsArePresent(reqSlots);
            if (missingSlot != null) {
                response.CopySessionAttributesFromRequest(request);
                return missingSlot;
            }

            //First we need to combine all the currently known slots from session
            List<AmazonSlot> slotsPresent = request.session.ConvertSessionAttributesToSlots();

            //We will replace slots in the main intent object witht the slots we found
            intent.ReplaceSlots(slotsPresent);

            //Now we need to sort them in the order in which they are defined in the configuration file
            intent.SortSlots(reqSlots);         

            intent.name.Remove(intent.name.IndexOf("FollowUp"));

            return null;
        }
    }
}
