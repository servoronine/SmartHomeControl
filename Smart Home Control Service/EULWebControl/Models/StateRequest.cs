using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeWebControl.Models
{
    public class StateRequest
    {
        public string DeviceName { get; set; }
        public string Token { get; set; }
    }
}
