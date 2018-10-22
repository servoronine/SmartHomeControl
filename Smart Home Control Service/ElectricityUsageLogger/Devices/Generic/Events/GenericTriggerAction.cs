using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartHomeControl.Devices.Generic.Events {

    public class GenericTriggerAction {
        private string action;
        public string Action {
            get {
                return action;
            }
        }

        public GenericTriggerAction(string action, string[] parameters) {
            this.action = action;
            PopulateParameters(parameters);
        }

        protected virtual void PopulateParameters(string[] parameters) {
        }

        public virtual object[] GetParametersForInvoke() {
            return null;
        }
    }
}
