using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeWebControl.Models.Amazon
{
    public class AmazonOutputSpeech
    {
        public string type { get; set; }
        public string text { get; set; }
        public string ssml { get; set; }
    }
}
