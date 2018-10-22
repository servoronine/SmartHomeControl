using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeWebControl.Models.Amazon
{
    public class AmazonRequestObjectConverter :JsonCreationConverter<AmazonRequestObject>
    {
        protected override AmazonRequestObject Create(Type objectType, JObject jObject) {
            switch (jObject["type"].ToString()) {
                case "IntentRequest":
                    return new AmazonIntentRequest();
                case "LaunchRequest":
                    return new AmazonLaunchRequest();
                case "SessionEndedRequest":
                    return new AmazonSessionEndedRequest();
            }
            return null;
        }
    }
}
