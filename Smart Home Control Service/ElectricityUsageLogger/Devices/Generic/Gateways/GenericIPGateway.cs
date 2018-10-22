using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using SmartHomeControl.EventProcessor;

namespace SmartHomeControl.Devices.Generic.Gateways {
    public class GenericIPGateway : GenericDeviceGateway {
        protected IPEndPoint localEndPoint;
        public IPEndPoint LocalEndPoint {
            get {
                return localEndPoint;
            }
        }

        public GenericIPGateway(XmlNode settings)
            : base(settings) {
                localEndPoint = new IPEndPoint(LocalSettings.LocalIPAddress, 0);
        }
    }
}
