using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SmartHomeControl.Remotes.Generic {
    public class GenericRemoteCommand {
        private string _commandName;
        private GenericRemote parentRemote;
        public GenericRemote ParentRemote {
            get {
                return parentRemote;
            }
        }

        private string voiceResponse;

        public string VoiceResponse {
            get {
                return voiceResponse;
            }
        }


        public GenericRemoteCommand(XmlNode settings, GenericRemote parentRemote)
        {
            _commandName = settings.InnerText;
            if (settings.Attributes["onProgramStart"] != null) {
                triggerOnStartUp = settings.Attributes["onProgramStart"].InnerText == "true" ? true : false;
            }
            if (settings.Attributes["voicePrompt"] != null) {
                string voicePrompt = settings.Attributes["voicePrompt"].InnerText;
                string[] splitStr = voicePrompt.Split('|');
                voicePrompts = new List<string>(splitStr);
            }

            if (settings.Attributes["voiceResponse"] != null) {
                voiceResponse = settings.Attributes["voiceResponse"].InnerText;
            }

            this.parentRemote = parentRemote;
        }

        public string commandName
        {
            get
            {
                return _commandName;
            }
        }

        private bool triggerOnStartUp;
        public bool TriggerOnStartUp {
            get {
                return triggerOnStartUp;
            }
        }

        private List<string> voicePrompts;
        public List<string> VoicePrompts {
            get {
                return voicePrompts;
            }
        }

        public bool ContainsVoicePrompt(string voicePrompt) {
            if (VoicePrompts == null) return false;
            foreach (string str in voicePrompts) {
                if (str == voicePrompt) return true;
            }
            return false;
        }
  }
}
