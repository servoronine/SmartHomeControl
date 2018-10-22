using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeWebControl.Models.Amazon
{
    public class AmazonCard
    {
        public string type { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string text { get; set; }
        public AmazonImage image { get; set; }
    }
}
