using SmartHomeControl.Devices.Generic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeControl.Devices.PC {
    public class PCState : GenericDeviceState {
        public bool IsOn { get; set; }
        public string PCName { get; set; }
    }
}
