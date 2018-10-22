using SmartHomeControl.Devices.Generic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartHomeControl.Devices.LightwaveRF {
    public static class LightwaveRFHelper {
        public static string GenerateDimString(GenericDevice device, int dimLevel) {
            string level = Math.Round((decimal)dimLevel / 100 * 32).ToString();
            return "000,!R" + device.ParentZone.zoneNumber.ToString() +
                    "D" + device.deviceNumber.ToString() +
                    "FdP" + level.ToString() + "|\0";
        }

        public static string GenerateToggleDeviceStateString(GenericDevice device, int state) {
            return "000,!R" + device.ParentZone.zoneNumber.ToString() +
                   "D" + device.deviceNumber.ToString() +
                   "F" + state.ToString() + "|\0";
        }
    }
}
