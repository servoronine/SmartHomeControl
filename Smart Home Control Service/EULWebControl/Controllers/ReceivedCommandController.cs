using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartHomeWebControl.Models;
using System.Net.Sockets;
using System.Net;
using System.Text;
using SmartHomeWebControl.Models.CommandServer;
using System.Net.Http.Headers;
using Microsoft.Extensions.OptionsModel;

namespace SmartHomeWebControl.Controllers
{
    [Route("api/[controller]")]
    public class ReceivedCommandController : Controller
    {
        private ICommandSenderHelper senderHelper;
        private AppSettings appSettings;
        public ReceivedCommandController(IOptions<AppSettings> siteSettings, ICommandSenderHelper helper) {
            senderHelper = helper;
            appSettings = siteSettings.Value;
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult PushRemoteCommand([FromBody] ReceivedCommand command) {
            if (command != null && command.Token == appSettings.SmartHomeServerAccessToken) {
                senderHelper.PushRemoteCommand(command, this);
                return new NoContentResult();
            }
            return HttpBadRequest();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult RequestDeviceState([FromBody] StateRequest stateRequest) {
            if (stateRequest == null || stateRequest.Token != appSettings.SmartHomeServerAccessToken) {
                return HttpBadRequest();
            }

            return senderHelper.RequestDeviceState(stateRequest.DeviceName, this);
        }
    }
}
