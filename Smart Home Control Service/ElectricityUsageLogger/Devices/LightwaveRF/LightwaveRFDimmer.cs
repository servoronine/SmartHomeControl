using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using SmartHomeControl.Devices.Generic.Devices;
using SmartHomeControl.Devices.Generic.Gateways;

namespace SmartHomeControl.Devices.LightwaveRF {
    class LightwaveRFDimmer : GenericDimmableDevice {
        private LightwaveRFGateway gateway;
        public LightwaveRFDimmer(XmlNode settings, GenericZone parentZone, GenericDeviceGateway gateway)
            : base(settings, parentZone, gateway) {
            this.gateway = (LightwaveRFGateway)gateway;
        }
                   
        public override bool SetDimLevel(int dimLevel) {
            try {
                gateway.SendCommand(
                    LightwaveRFHelper.GenerateDimString(this, dimLevel)
                    );
                return true;
            } catch {
                return false;
            }
        }

        public override void ToggleDeviceState(int state) {
                gateway.SendCommand(
                    LightwaveRFHelper.GenerateToggleDeviceStateString(this, state)
                );
        }
    }
}
