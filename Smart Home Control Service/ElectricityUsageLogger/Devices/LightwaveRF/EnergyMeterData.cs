using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeControl.Devices.LightwaveRF {
    class EnergyMeterData
    {
        public int trans { get; set; }
        public string mac { get; set; }
        public int time { get; set; }
        public DateTime timeConverted {
            get {
                DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(time).ToLocalTime();
                return dtDateTime;
            }
        }
        public string prod { get; set; }
        public string serial { get; set; }
        public string router { get; set; }
        public string type { get; set; }
        public int cUse { get; set; }
        public int todUse { get; set; }
        public int yesUse { get; set; }
    }
}
