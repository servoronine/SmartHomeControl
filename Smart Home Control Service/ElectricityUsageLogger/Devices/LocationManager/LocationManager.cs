using SmartHomeControl.Devices.Generic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHomeControl.Devices.Generic.Gateways;
using System.Xml;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using SmartHomeControl.Devices.Generic.Events;
using SmartHomeControl.EventProcessor;

namespace SmartHomeControl.Devices.LocationManager {
    public class LocationManager : GenericWebDevice {
        private string apiKey;
        private string homeCoordinates;
        private LocationStateEnum locationState = LocationStateEnum.Unknown;

        public LocationManager(XmlNode settings, GenericZone parentZone, GenericDeviceGateway gateway) 
            : base(settings, parentZone, gateway) {

            if (settings.Attributes["apiKey"] != null) {
                apiKey = settings.Attributes["apiKey"].InnerText;
            }
            homeCoordinates = LocalSettings.HomeCoordinates;
        }

        public void LogLocation(string currentCoordinates) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(location + "?units=imperial&origins=" +
                homeCoordinates + "&destinations=" + currentCoordinates + "&key=" + apiKey);

            try {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream()) {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    string respJson = reader.ReadToEnd();

                    GoogleResponse googleResp = 
                        new JavaScriptSerializer().Deserialize<GoogleResponse>(respJson);

                    int durationMins = int.Parse(googleResp.rows[0].elements[0].duration.value) / 60;

                    if (durationMins >=30) {
                        locationState = LocationStateEnum.Away;
                        RaiseDeviceEvent(this,
                            new FeedbackReceivedFromDeviceEventArgs("Away", new object[] { durationMins }));
                    } else if (durationMins < 30) {
                        locationState = LocationStateEnum.Home;
                        RaiseDeviceEvent(this,
                            new FeedbackReceivedFromDeviceEventArgs("Home", new object[] { durationMins }));
                    }
                }
            } catch (WebException ex) {

            }
        }
    }
}
