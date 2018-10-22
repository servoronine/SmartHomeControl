using SmartHomeControl.Devices.Generic.Gateways;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SmartHomeControl.Devices.Generic.Devices {
    public class GenericLockableDevice : GenericDevice
    {
        public GenericLockableDevice(XmlNode settings, GenericZone parentZone, GenericDeviceGateway gateway)
            : base(settings, parentZone, gateway)
        {
        }

        public virtual bool SetLockState(bool state)
        {
            throw new Exception("This method must be overloaded in derived classes!");
        }
    }
}
