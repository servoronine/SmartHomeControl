using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeControl.Devices.GreenIQ {
    public class PortsConfiguration {
        public bool master { get; set; }
        public List<IrrigationPort> ports { get; set; }
    }
}
