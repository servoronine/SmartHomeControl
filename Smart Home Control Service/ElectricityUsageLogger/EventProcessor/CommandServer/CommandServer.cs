using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHomeControl.Devices.Generic.Devices;
using System.ServiceModel;
using System.Web.Script.Serialization;
using System.Reflection;

namespace SmartHomeControl.EventProcessor.CommandServer {
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class CommandServer : ICommandServer {
        public ExternalCommandReceivedDelegate commandDelegate;
        private RequestDeviceStateDelegate requestDelegate;

        public CommandServer(RequestDeviceStateDelegate reqDel, ExternalCommandReceivedDelegate comDel) {
            requestDelegate = reqDel;
            commandDelegate = comDel;
        }

        public string GetDeviceState(string deviceName) {
            return new JavaScriptSerializer().Serialize(requestDelegate(deviceName));
        }

        public string GetDeviceStateVariable(string deviceName, string variableName) {
            string finalResponse = null;
            GenericDeviceState state = requestDelegate(deviceName);

            foreach (PropertyInfo prop in state.GetType().GetProperties()) {
                if (prop.Name == variableName) {
                    try {
                        object objAttr = prop.GetCustomAttribute(typeof(StateResponseFormatAttribute));
                        StateResponseFormatAttribute attr = objAttr as StateResponseFormatAttribute;
                        object objValue = state.GetType().GetProperty(variableName).GetValue(state);
                        if (objValue != null) {
                            finalResponse = String.Format(attr.ResponseFormat, objValue);
                        } else {
                            finalResponse = attr.NullFormat;
                        }
                        
                    } catch {
                        finalResponse = "An error occurred. Please check your Alexa configuration.";
                    }
                }
            }
            return finalResponse;
        }

        public string TriggerCommand(string remoteName, string commandName, object[] parameters) {
            return commandDelegate(remoteName, commandName, parameters);
        }
    }
}
