using SmartHomeControl.Devices.Generic.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeControl.Devices.Onkyo {
    public class OnkyoStationTriggerAction : GenericTriggerAction {
        private int stationNumber;

        public OnkyoStationTriggerAction(string action, string[] parameters)
            : base(action, parameters) {
        }

        protected override void PopulateParameters(string[] parameters) {
            base.PopulateParameters(parameters);

            if (parameters.Length > 0) {
                this.stationNumber = int.Parse(parameters[0]);
            } else {
                stationNumber = 0;
            }
        }

        public override object[] GetParametersForInvoke() {
            if (stationNumber > -1) {
                return new object[] { stationNumber };
            } else {
                return null;
            }
        }
    }
}
