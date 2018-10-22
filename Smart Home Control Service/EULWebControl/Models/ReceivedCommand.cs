using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeWebControl.Models
{
    public class ReceivedCommand
    {
        public string Remote { get; set; }
        public string Command { get; set; }
        public List<string> Parameters { get; set; }
        public string Token { get; set; }
    }
}
