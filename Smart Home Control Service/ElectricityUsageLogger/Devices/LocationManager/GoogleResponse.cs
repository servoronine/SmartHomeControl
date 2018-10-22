using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeControl.Devices.LocationManager {
    public class GoogleResponse {
        public string status { get; set; }
        public List<string> origin_addresses { get; set; }
        public List<string> destination_addresses { get; set; }
        public List<GoogleRow> rows { get; set; }
    }
}
