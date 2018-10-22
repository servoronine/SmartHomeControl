using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeControl.Remotes.Generic {
    public class RemoteCommandList : List<GenericRemoteCommand> {
        public GenericRemoteCommand this[string command] {
            get {
                foreach (GenericRemoteCommand cm in this) {
                    if (command == cm.commandName) {
                        return cm;
                    }
                }
                return null;
            }
        }
    }
}
