using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SmartHomeControl.Devices.Generic.Gateways {
    public class GenericJSONWebGateway : GenericDeviceGateway {
        protected string location;
        public string Location {
            get {
                return location;
            }
        }

        public GenericJSONWebGateway(XmlNode settings)
            : base(settings) {
        }
    }
}
