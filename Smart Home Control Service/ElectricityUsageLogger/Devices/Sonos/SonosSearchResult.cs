using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeControl.Devices.Sonos {
    public class SonosSearchResult {
        public string Result { get; set; }
        public uint StartingIndex { get; set; }
        public uint NumberReturned { get; set; }
        public uint TotalMatches { get; set; }
    }
}
