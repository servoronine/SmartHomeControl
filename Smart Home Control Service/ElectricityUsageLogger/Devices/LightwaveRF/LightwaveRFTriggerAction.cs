using SmartHomeControl.Devices.Generic.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartHomeControl.Devices.LightwaveRF {
    public class LightwaveRFTriggerAction : GenericTriggerAction {

        public int value;

        public LightwaveRFTriggerAction(string action, string[] parameters)
            : base(action, parameters) {
        }

        protected override void PopulateParameters(string[] parameters) {
            base.PopulateParameters(parameters);
            if (parameters.Length > 0) {
                this.value = int.Parse(parameters[0]);
            }
            else {
                value = -1;
            }
        }

        public override object[] GetParametersForInvoke() {
            if (value > -1) {
                return new object[] { value };
            }
            else {
                return null;
            }
        }
    }
}
