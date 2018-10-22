using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeControl.Devices.GreenIQ {
    public class IrrigationPort {
        public int number { get; set; }
        public string name { get; set; }
        public PortConfigurationEnum configuration { get; set; }
        public bool shown { get; set; }
        public int presentation_index { get; set; }
        public bool active { get; set; }
        public string end_time { get; set; }
        public int progress { get; set; }
    }
}
