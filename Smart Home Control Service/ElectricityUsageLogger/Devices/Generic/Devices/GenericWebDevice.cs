using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SmartHomeControl.Devices.Generic.Gateways;

namespace SmartHomeControl.Devices.Generic.Devices {
    public class GenericWebDevice : GenericDevice {
        protected string location;
        public GenericWebDevice(XmlNode settings, GenericZone parentZone, GenericDeviceGateway gateway) : base(settings, parentZone, gateway) {
            location = settings.Attributes["location"].Value;
        }
    }
}
