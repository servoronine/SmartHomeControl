using SmartHomeControl.Devices.Generic.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeControl.Devices.Onkyo {
    public class OnkyoVolumeTriggerAction : GenericTriggerAction {
        private int volume;

        public OnkyoVolumeTriggerAction(string action, string[] parameters)
            : base(action, parameters) {
        }

        protected override void PopulateParameters(string[] parameters) {
            base.PopulateParameters(parameters);

            if (parameters.Length > 0) {
                int volumeLevel = int.Parse(parameters[0]);
                if (volumeLevel >= 0 && volumeLevel <= 50) {
                    this.volume = volumeLevel;
                }
            }
            else {
                volume = -1;
            }
        }

        public override object[] GetParametersForInvoke() {
            if (volume > -1) {
                return new object[] { volume };
            }
            else {
                return null;
            }
        }
    }
}
