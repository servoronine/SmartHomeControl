using SmartHomeControl.Devices.Generic.Gateways;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SmartHomeControl.EventProcessor {
    public class GatewayList : List<GenericDeviceGateway> {
        public GenericDeviceGateway this[string gateway] {
            get {
                foreach (GenericDeviceGateway gtw in this) {
                    if (gtw.Name == gateway) {
                        return gtw;
                    }
                }
                return null;
            }
        }

        public GatewayList(XmlNode settings) {
            foreach (XmlNode child in settings.ChildNodes) {
                Type typ = Type.GetType(child.Attributes["type"].InnerText);
                GenericDeviceGateway gtw = (GenericDeviceGateway)Activator.CreateInstance(typ, child);
                this.Add(gtw);
            }
        }
    }
}
