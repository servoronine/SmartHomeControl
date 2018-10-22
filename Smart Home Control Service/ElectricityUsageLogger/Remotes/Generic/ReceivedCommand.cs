using SmartHomeControl.Devices.Generic.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartHomeControl.Remotes.Generic {
    public class ReceivedCommand  {
        private object sender;
        private string command;
        private DateTime timeReceived;
        private object[] parameters;
        private List<GenericTrigger> processedTriggers = new List<GenericTrigger>();

        public List<GenericTrigger> ProcessedTriggers {
            get {
                return processedTriggers;
            }
        }

        public object Sender {
            get {
                return sender;
            }
        }

        public string Command {
            get {
                return command;
            }
        }

        public DateTime TimeReceived {
            get {
                return timeReceived;
            }
        }

        public object[] Parameters {
            get {
                return parameters;
            }
        }

        public ReceivedCommand(object sender, string command, DateTime timeReceived) {
            this.sender = sender;
            this.command = command;
            this.timeReceived = timeReceived;
        }

        public ReceivedCommand(object sender, string command, DateTime timeReceived, object[] parameters) {
            this.sender = sender;
            this.command = command;
            this.timeReceived = timeReceived;
            this.parameters = parameters;
        }

    }
}
