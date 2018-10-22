using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeControl.Devices.Generic.Events {
    public class DelayTriggerAction : GenericTriggerAction {
        public DelayTriggerAction(string action, string[] parameters) 
            : base(action, parameters) {
        }

        public int value;
        protected override void PopulateParameters(string[] parameters) {
            base.PopulateParameters(parameters);
            if (parameters.Length > 0) {
                this.value = int.Parse(parameters[0]);
            }
            else {
                value = 0;
            }
        }

        public override object[] GetParametersForInvoke() {
            if (value > 0) {
                return new object[] { value };
            }
            else {
                return null;
            }
        }
    }
}
