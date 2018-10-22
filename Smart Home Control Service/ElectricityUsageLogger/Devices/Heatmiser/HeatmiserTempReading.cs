using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeControl.Devices.Heatmiser {
    public class HeatmiserTempReading {
        public int CurrentAirTemp { get; set; }
        public DateTime Date { get; set; }
        public bool IsHeating { get; set; }
    }
}
