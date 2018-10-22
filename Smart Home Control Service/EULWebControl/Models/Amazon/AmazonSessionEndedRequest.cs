using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeWebControl.Models.Amazon
{
    public class AmazonSessionEndedRequest : AmazonRequestObject
    {
        public string reason { get; set; }
    }
}
