using SmartHomeWebControl.Models;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeWebControl.Controllers
{
    //Commented for testing purposes
    //[RequireHttps]
    [Route("Login")]
    public class AccountController : Controller
    {
        [FromServices]
        WebControlDbContext context { get; set; }
        private string amazonUri;
        private string clientSecret;
        private string clientID;

        public AccountController(IOptions<AppSettings> siteSettings) {
            amazonUri = siteSettings.Value.AmazonUri;
            clientSecret = siteSettings.Value.AmazonClientSecret;
            clientID = siteSettings.Value.AmazonClientID;
        }

        [HttpGet]
        public IActionResult Login(string state, string client_id, string response_type, string scope, string redirect_uri) {
            TempData["state"] = state;
            TempData["redirectURI"] = redirect_uri;

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model) {
            string code = Guid.NewGuid().ToString("N");
            TempData["code"] = code;

            string redirectUrl = TempData["redirectUri"] + "?state=" + TempData["state"] + "&code=" + code;

            return Redirect(redirectUrl);
        }
    }
}
