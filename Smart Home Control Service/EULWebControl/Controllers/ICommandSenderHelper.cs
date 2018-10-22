using Microsoft.AspNet.Mvc;
using SmartHomeWebControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeWebControl.Controllers
{
    public interface ICommandSenderHelper {
        string PushRemoteCommand(ReceivedCommand command, Controller controller);
        IActionResult RequestDeviceState(string deviceName, Controller controller);
        string GetDeviceStateVariable(string deviceName, string variableName);
    }
}
