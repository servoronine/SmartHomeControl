using SmartHomeControl.Devices.Generic.Events;
using SmartHomeControl.Helpers;
using SmartHomeControl.Remotes.Generic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SmartHomeControl.EventProcessor {
    public class RemoteList : List<GenericRemote> {
        public event FeedbackReceivedFromDeviceDelegate ButtonPressed;

        public GenericRemote this[string remote] {
            get {
                foreach (GenericRemote rem in this) {
                    if (rem.remoteName == remote) {
                        return rem;
                    }
                }
                return null;
            }
        }

        public GenericRemote this[object remote] {
            get {
                foreach (GenericRemote rem in this) {
                    if (rem == remote) {
                        return rem;
                    }
                }
                return null;
            }
        }

        public RemoteList(XmlNode settings) {
            foreach (XmlNode child in settings.ChildNodes) {
                GenericRemote rem = new GenericRemote(child);
                this.Add(rem);
            }
        }

        protected void TriggerSpecificCommand(string remote, string command) {
            TriggerSpecificCommand(remote, command, null);
        }

        public string TriggerSpecificCommand(string remote, string command, object[] parameters) {
            foreach (GenericRemote rem in this) {
                if (rem.remoteName == remote) {
                    foreach (GenericRemoteCommand com in rem.RemoteCommands) {
                        if (command.StartsWith("<VOICE>") && com.ContainsVoicePrompt(command.Substring(7)) ||
                            com.commandName == command) {
                            ButtonPressed(rem, new FeedbackReceivedFromDeviceEventArgs(com.commandName, parameters));
                            if (com.VoiceResponse != null) {
                                if (parameters != null) {
                                    for (int i=0; i<parameters.Length; i++) {
                                        if (parameters[i] is DateTime) {
                                            string strDate = ((DateTime)parameters[i]).ToString("yyyy-MM-dd HH:mm");
                                            parameters[i] = strDate;
                                        }
                                    }

                                    return String.Format(com.VoiceResponse, parameters);
                                } else {
                                    return com.VoiceResponse;
                                }
                                
                            } else {
                                return null;
                            }
                        }
                    }
                }
            }

            return "Your command was not found. Please check your configuration.";
        }

        public void SimulateRemoteCommand(GenericRemoteCommand command) {
            TriggerSpecificCommand(command.ParentRemote.remoteName, command.commandName, null);
        }
    }
}
