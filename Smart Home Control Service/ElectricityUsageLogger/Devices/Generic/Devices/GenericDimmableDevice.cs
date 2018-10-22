using SmartHomeControl.Devices.Generic.Gateways;
using SmartHomeControl.EventProcessor;
using SmartHomeControl.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SmartHomeControl.Devices.Generic.Devices
{
    public class GenericDimmableDevice : GenericDevice
    {
        public GenericDimmableDevice(XmlNode settings, GenericZone parentZone, GenericDeviceGateway gateway)
            : base(settings, parentZone, gateway)
        {
        }

        public virtual bool SetDimLevel(int dimLevel)
        {
            if (dimLevel < 0 || dimLevel > 100)
            {
                throw new Exception("Incorrect Dim Level");
            }
            return true;
        }

        public bool SetDimLevelIfDark(int dimLevel) {
            string[] coords = LocalSettings.HomeCoordinates.Split(',');
            SolarInfo si = SolarInfo.ForDate(double.Parse(coords[0]), double.Parse(coords[1]), DateTime.Now);

            if (si.Sunset <= DateTime.Now || si.Sunrise >= DateTime.Now) {
                return SetDimLevel(dimLevel);
            }
            return false;
        }
    }
}
