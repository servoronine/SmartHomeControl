using SmartHomeControl.Devices.Generic.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartHomeControl.EventProcessor {
    public class TriggerToBeInvoked : IComparable {
        public GenericTrigger Trigger;
        public object[] TriggerParameters;

        public int CompareTo(object obj) {
            TriggerToBeInvoked trig = (TriggerToBeInvoked)obj;
            if (this.Trigger.Sequence > trig.Trigger.Sequence) {
                return 1;
            }
            else if (this.Trigger.Sequence < trig.Trigger.Sequence) {
                return -1;
            }
            else {
                return 0;
            }
        }
    }
}
