using SmartHomeControl.Devices.Jawbone;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml;

namespace SmartHomeControl.Helpers {
    public class WebClientHelper {
        public static object RequestJSONData(Type returnType, string requestString, string bearerKey) {
            WebClient wc = new WebClient();
            wc.Headers.Add("Accept", "application/json");
            wc.Headers.Add("Content-Type", "multipart/form-data");
            if (bearerKey != null) {
                wc.Headers.Add("Authorization", "Bearer " + bearerKey);
            }

            try {
                System.IO.Stream data = wc.OpenRead(requestString);
                System.IO.StreamReader reader = new System.IO.StreamReader(data);
                string s = reader.ReadToEnd();
                data.Close();
                reader.Close();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                WebClientHelper.RegisterJSONConverters(serializer);
                object deserializedObject = serializer.Deserialize(s, returnType);
                return deserializedObject;
            }
            catch (Exception ex) {
                LoggingHelper.LogExceptionInApplicationLog("WebClientHelper.RequestJSONData", new Exception(ex.Message), System.Diagnostics.EventLogEntryType.Error);
                LoggingHelper.WriteExceptionLogEntry("WebClientHelper.RequestJSONData", new Exception(ex.Message));
                return null;
            }
        }

        public static void PostJSONData(string requestString, object jsonBody, string bearerKey) {
            try {
                WebClient wc = new WebClient();
                wc.Headers.Add("Content-Type", "application/json");
                if (bearerKey != null) {
                    wc.Headers.Add("Authorization", "Bearer " + bearerKey);
                }

                JavaScriptSerializer ser = new JavaScriptSerializer();
                wc.UploadString(requestString, ser.Serialize(jsonBody));
            } catch (Exception ex) {
                LoggingHelper.LogExceptionInApplicationLog("WebClientHelper.PostJSONData", new Exception(ex.Message), System.Diagnostics.EventLogEntryType.Error);
                LoggingHelper.WriteExceptionLogEntry("WebClientHelper.PostJSONData", new Exception(ex.Message));
            }
        }

        private static void RegisterJSONConverters(JavaScriptSerializer serializer) {
            serializer.RegisterConverters(new JavaScriptConverter[] { new UpBandDataItemConverter() });
        }

        public static XmlDocument RequestXMLData(string requestString, string requestParams) {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(requestString);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            try {
                HttpResponseMessage message = client.GetAsync(requestParams).Result;
                XmlDocument doc = null;
                if (message.IsSuccessStatusCode) {
                    Stream stream = message.Content.ReadAsStreamAsync().Result;

                    doc = new XmlDocument();
                    doc.Load(stream);
                }
                return doc;
            }
            catch (Exception ex) {
                LoggingHelper.LogExceptionInApplicationLog("WebClientHelper.RequestXMLData", new Exception(ex.Message), System.Diagnostics.EventLogEntryType.Error);
                LoggingHelper.WriteExceptionLogEntry("WebClientHelper.RequestXMLData", new Exception(ex.Message));
                return null;
            }
        }
    }
}
