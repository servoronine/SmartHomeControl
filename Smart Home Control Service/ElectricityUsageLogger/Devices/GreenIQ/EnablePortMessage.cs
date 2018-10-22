using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeControl.Devices.GreenIQ {
    public class EnablePortMessage {
        public int number { get; set; }
        public PortConfigurationEnum configuration { get; set; }
        public int duration { get; set; }
    }
}
