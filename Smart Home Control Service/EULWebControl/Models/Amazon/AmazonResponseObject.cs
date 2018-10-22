using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeWebControl.Models.Amazon
{
    public class AmazonResponseObject
    {
        public AmazonOutputSpeech outputSpeech { get; set; }
        public AmazonCard card { get; set; }
        public AmazonReprompt reprompt { get; set; }
        public bool shouldEndSession { get; set; }
    }
}
