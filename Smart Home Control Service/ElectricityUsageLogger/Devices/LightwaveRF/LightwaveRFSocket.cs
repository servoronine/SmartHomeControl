using SmartHomeControl.Devices.Generic.Devices;
using SmartHomeControl.Devices.Generic.Gateways;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SmartHomeControl.Devices.LightwaveRF {
    class LightwaveRFSocket : GenericLockableDevice {
        private LightwaveRFGateway gateway;
        public LightwaveRFSocket(XmlNode settings, GenericZone parentZone, GenericDeviceGateway gateway)
            : base(settings, parentZone, gateway) {
            this.gateway = (LightwaveRFGateway)gateway;
        }
                   
        public override void ToggleDeviceState(int state) {
                gateway.SendCommand(
                    LightwaveRFHelper.GenerateToggleDeviceStateString(this, state)
                    );
        }
    }
}
