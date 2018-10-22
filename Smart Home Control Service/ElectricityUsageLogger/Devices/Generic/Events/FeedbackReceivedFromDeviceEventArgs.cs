using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartHomeControl.Devices.Generic.Events {
    public class FeedbackReceivedFromDeviceEventArgs : EventArgs {
        private string command;
        public string Command {
            get {
                return command;
            }
        }

        private object[] parameters;
        public object[] Parameters {
            get {
                return parameters;
            }
        }

        public FeedbackReceivedFromDeviceEventArgs(string command, object[] parameters) {
            this.command = command;
            this.parameters = parameters;
        }
    }
}
