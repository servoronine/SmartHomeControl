using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeWebControl.Controllers
{
    [Route("Token")]
    public class TokenController : Controller {
        [HttpPost]
        public IActionResult Token([FromForm] List<string> s) {
            return new NoContentResult();
        }
    }
}
