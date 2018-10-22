using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.OptionsModel;
using SmartHomeWebControl.Models;
using SmartHomeWebControl.Models.CommandServer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeWebControl.Controllers
{
    public class CommandSenderHelper : ICommandSenderHelper
    {
        private CommandServerProxy commandSrvProxy;

        public CommandSenderHelper(IOptions<AppSettings> siteSettings) {
            commandSrvProxy = new CommandServerProxy(siteSettings.Value.SmartHomeServerEndPointUrl);
        }

        public string PushRemoteCommand(ReceivedCommand command, Controller controller) {
            if (command == null && controller != null) {
                return null;
            }

            object[] parameters = null;
            if (command.Parameters != null) {
                parameters = ConvertParametersToRealObjects(command.Parameters);
            }   

            return commandSrvProxy.SendCommand(command.Remote, command.Command, parameters); ;
        }

        public string GetDeviceStateVariable(string deviceName, string variableName) {
            if (deviceName == null || variableName == null) {
                return null;
            }

            return commandSrvProxy.RequestDeviceStateVariable(deviceName, variableName);
        }

        private object[] ConvertParametersToRealObjects(List<string> stringParams) {
            List<object> parameters = new List<object>();
            foreach (string str in stringParams) {
                string[] splitParam = str.Split('|');
                switch (splitParam[0]) {
                    case "date":
                        parameters.Add(DateTime.ParseExact(splitParam[1], "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture));
                        break;
                    case "int":
                        parameters.Add(int.Parse(splitParam[1]));
                        break;
                    case "string":
                        parameters.Add(splitParam[1]);
                        break;
                }
            }
            return parameters.ToArray();
        }

        public IActionResult RequestDeviceState(string deviceName, Controller controller) {
            if (deviceName == null) {
                return controller.HttpBadRequest();
            }
            string state = commandSrvProxy.RequestDeviceState(deviceName);
            return controller.Content(state, "application/json");
        }
    }
}
