using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeControl.Devices.Generic.Devices {
    public class StateResponseFormatAttribute : Attribute {
        public string ResponseFormat { get; set; }
        public string NullFormat { get; set; }
        public StateResponseFormatAttribute(string responseFormat, string nullFormat) {
            ResponseFormat = responseFormat;
            NullFormat = nullFormat;
        }
    }
}
