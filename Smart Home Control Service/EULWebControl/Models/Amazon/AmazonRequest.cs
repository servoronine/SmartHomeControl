using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeWebControl.Models.Amazon
{
    public class AmazonRequest
    {
        public string version { get; set; }
        public AmazonSession session { get; set; }
        public AmazonRequestObject request { get; set; }
    }
}
