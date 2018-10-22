using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeControl.Devices.LocationManager {
    public class GoogleElement {
        public string status { get; set; }
        public GoogleResult duration { get; set; }
        public GoogleResult distance { get; set; }
    }
}
