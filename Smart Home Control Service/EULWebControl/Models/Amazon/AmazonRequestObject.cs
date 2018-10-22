using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeWebControl.Models.Amazon
{
    public abstract class AmazonRequestObject
    {
        public string type { get; set; }
        public string requestId { get; set; }
        public string timestamp { get; set; }
    }
}
