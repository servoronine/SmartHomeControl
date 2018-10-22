using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SmartHomeControl.Devices.Generic.Gateways {
    public class GenericXMLWebGateway : GenericDeviceGateway {
        protected string location;
        public string Location {
            get {
                return location;
            }
        }

        public GenericXMLWebGateway(XmlNode settings)
            : base(settings) {
        }
    }
}
