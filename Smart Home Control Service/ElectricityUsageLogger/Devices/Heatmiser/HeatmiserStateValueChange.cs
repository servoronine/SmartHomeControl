using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartHomeControl.Devices.Heatmiser {
    public class HeatmiserStateValueChange {
        public ushort Address;
        public byte[] Contents;
        public byte NumberOfBytes;
    }
}
