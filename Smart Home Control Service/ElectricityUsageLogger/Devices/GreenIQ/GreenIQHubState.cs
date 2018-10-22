using SmartHomeControl.Devices.Generic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeControl.Devices.GreenIQ {
    public class GreenIQHubState : GenericDeviceState {
        public PortsConfiguration PortsConfiguration { get; set; }
    }
}
