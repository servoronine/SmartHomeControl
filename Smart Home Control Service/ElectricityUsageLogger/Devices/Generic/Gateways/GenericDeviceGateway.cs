using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmartHomeControl.Devices.Generic.Devices;
using SmartHomeControl.Devices.Generic.Events;

namespace SmartHomeControl.Devices.Generic.Gateways
{
    public class GenericDeviceGateway {
        public GenericDeviceGateway(XmlNode settings) {
            name = settings.Attributes["name"].InnerText;
        }

        private string name;
        public string Name {
            get {
                return name;
            }
        }
    }
}
