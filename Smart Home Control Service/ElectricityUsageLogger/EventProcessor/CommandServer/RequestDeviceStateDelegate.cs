using SmartHomeControl.Devices.Generic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeControl.EventProcessor.CommandServer {
    public delegate GenericDeviceState RequestDeviceStateDelegate(string deviceName);
}
