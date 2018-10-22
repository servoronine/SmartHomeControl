using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmartHomeControl.Devices.Generic.Devices;
using SmartHomeControl.Remotes.Generic;

namespace SmartHomeControl.Devices.Generic.Events
{
    public class GenericTrigger
    {
        private List<string> triggerCommands = new List<string>();
        public List<string> TriggerCommands {
            get {
                return triggerCommands;
            }
        }

        public SenderTypeEnum SenderType { get; set; }
        public string Sender { get; set; }

        private int sequence = 1;
        public int Sequence {
            get {
                return sequence;
            }
        }
        private GenericDevice parentDevice;
        private GenericTriggerAction action;

        public string this[int i]
        {
            get
            {
                return triggerCommands[i];
            }
        }

        public GenericTrigger(XmlNode settings, GenericDevice parentDevice)
        {
            string[] parameters = new string[settings.Attributes.Count - 5];
            for (int i = 5; i < settings.Attributes.Count; i++) {
                parameters[i - 5] = settings.Attributes[i].InnerText;
            }

            switch (settings.Attributes["senderType"].InnerText) {
                case "Remote":
                    SenderType = SenderTypeEnum.Remote;
                    break;
                case "Device":
                    SenderType = SenderTypeEnum.Device;
                    break;
                case "Schedule":
                    SenderType = SenderTypeEnum.Schedule;
                    break;
            }

            Sender = settings.Attributes["sender"].InnerText;

            Type typ = Type.GetType(settings.Attributes["actionType"].InnerText);
            action = (GenericTriggerAction)Activator.CreateInstance(typ, settings.Attributes["action"].InnerText, parameters);

            sequence = int.Parse(settings.Attributes["sequence"].InnerText);
            this.parentDevice = parentDevice;

            foreach (XmlNode buttonNode in settings.ChildNodes) {
                triggerCommands.Add(buttonNode.InnerText);
            }    
        }

        public void InvokeAction(object[] parameters) {
            parentDevice.InvokeAction(this.action, parameters);
        }
    }
}
