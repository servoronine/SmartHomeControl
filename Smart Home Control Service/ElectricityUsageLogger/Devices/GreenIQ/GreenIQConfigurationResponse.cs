using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeControl.Devices.GreenIQ {
    public class GreenIQConfigurationResponse {
        public PortsConfiguration data { get; set; }
        public bool status;
    }
}
