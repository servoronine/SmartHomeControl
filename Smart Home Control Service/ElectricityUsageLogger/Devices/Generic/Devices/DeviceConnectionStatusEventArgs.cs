using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartHomeControl.Devices.Generic.Devices
{
    public class DeviceConnectionStatusEventArgs : EventArgs {
        private string updateMessage;

        public DeviceConnectionStatusEventArgs(string message) {
            updateMessage = message;
        }

        public string GetMessage() {
            return updateMessage;
        }
    }
}
