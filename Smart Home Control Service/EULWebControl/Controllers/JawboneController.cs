using Microsoft.AspNet.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartHomeWebControl.Controllers
{
    [Route("api/[controller]")]
    public class JawboneController : Controller
    {
        private const string jawboneAuthAddress = "https://jawbone.com/auth/oauth2/auth";
        private const string jawboneTokenAddress = "https://jawbone.com/auth/oauth2/token";
        private const string clientID = "rD_cXGn_c2M";
        private const string scope = "basic_read extended_read location_read friends_read mood_read mood_write move_read move_write sleep_read sleep_write meal_read meal_write weight_read weight_write generic_event_read generic_event_write heartrate_read";
        private const string redirectURI = "https://webcontrol.voronin.co.uk/api/jawbone/token";
        private const string appSecret = "cd2b3cac6157829c6a197566db7886efdf0cd721";

        [Route("authenticate")]
        public IActionResult Authenticate() {
            string redirectUriEncoded = redirectURI.Replace("/", "%2F").Replace(":", "%3A");

            string redirectUrl = jawboneAuthAddress +
                "?response_type=code" +
                "&client_id=" + clientID +
                "&scope=" + scope +
                "&redirect_uri=" + redirectUriEncoded;
            return Redirect(redirectUrl);
        }

        [Route("token")]
        public void Token(string code) {
            string callURI = jawboneTokenAddress +
                "?client_id=" + clientID +
                "&client_secret=" + appSecret +
                "&grant_type=authorization_code" +
                "&code=" + code;

            HttpClient client = new HttpClient();
            Uri uri = new Uri(callURI);
            var response = client.GetAsync(uri);

            string str = response.Result.Content.ReadAsStringAsync().Result;
        }
    }
}
