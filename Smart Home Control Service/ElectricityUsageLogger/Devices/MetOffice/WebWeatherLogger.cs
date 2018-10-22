using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Timers;
using SmartHomeControl.Devices.Generic.Gateways;
using SmartHomeControl.Devices.Generic.Devices;
using SmartHomeControl.Helpers;
using SmartHomeControl.EventProcessor;

namespace SmartHomeControl.Devices.MetOffice {
    public class WebWeatherLogger : GenericWebDevice {
        private Timer timer;

        private string webWeatherAddress;
        private string webWeatherParams;
        private int pollInterval;
        public int PollInterval {
            get {
                return pollInterval;
            }
        }

        //
        //http://datapoint.metoffice.gov.uk/public/data/val/wxobs/all/xml/3740?res=hourly&key=b08fb588-2bf3-4e27-9f81-3356e3215962
        //

        public WebWeatherLogger(XmlNode settings, GenericZone parentZone, GenericDeviceGateway gateway) : 
            base(settings, parentZone, gateway) {
            this.pollInterval = int.Parse(settings.Attributes["loggingPollInterval"].Value);
            this.webWeatherAddress = settings.Attributes["location"].Value;
            this.webWeatherParams = settings.Attributes["params"].Value;
        }

        public void LogWebWeather() {
            if (timer == null) {
                InitializeTimer();
            }
        }

        private void InitializeTimer() {
            timer = new Timer(PollInterval * 1000);
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Enabled = true;
            OnTimedEvent(null, null);
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e) {
            List<WeatherInfo> weatherInfo = RequestData();
            if (weatherInfo != null) { 
                PostWeatherInfoToDb(weatherInfo);
            } else {
                LoggingHelper.LogExceptionInApplicationLog(this.ToString(), new Exception("WebWeatherGateway returned no data!"), System.Diagnostics.EventLogEntryType.Error);
                LoggingHelper.WriteExceptionLogEntry(this.ToString(), new Exception("WebWeatherGateway returned no data!"));
            }
        }

        public void PostWeatherInfoToDb(List<WeatherInfo> infoList) {

            SqlConnection connection = new SqlConnection(LocalSettings.SqlConnectionString);
            connection.Open();

            foreach (WeatherInfo info in infoList) {
                SqlCommand cmd = new SqlCommand("PostWeatherLog", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DateTime", SqlDbType.DateTime).Value = info.LoggedDateTime;
                cmd.Parameters.Add("@WindDirection", SqlDbType.VarChar).Value = info.WindDirection;
                cmd.Parameters.Add("@ScreenRelativeHumidity", SqlDbType.Decimal).Value = info.ScreenRelativeHumidity;
                cmd.Parameters.Add("@Pressure", SqlDbType.SmallInt).Value = info.Pressure;
                cmd.Parameters.Add("@WindSpeed", SqlDbType.SmallInt).Value = info.WindSpeed;
                cmd.Parameters.Add("@Temperature", SqlDbType.Decimal).Value = info.Temperature;
                cmd.Parameters.Add("@WeatherType", SqlDbType.SmallInt).Value = info.WeatherType;
                cmd.Parameters.Add("@PressureTendency", SqlDbType.VarChar).Value = info.PressureTendency;
                cmd.Parameters.Add("@DewPoint", SqlDbType.Decimal).Value = info.DewPoint;
                cmd.Parameters.Add("@WindGust", SqlDbType.Decimal).Value = info.WindGust;
                cmd.ExecuteNonQuery();
            }
            connection.Close();
        }

        public List<WeatherInfo> RequestData() {
            XmlDocument doc = WebClientHelper.RequestXMLData(webWeatherAddress, webWeatherParams);

            if (doc == null) return null;

            XmlNodeList list = doc.SelectNodes("SiteRep/DV/Location/Period");

            List<WeatherInfo> infoList = new List<WeatherInfo>();

            foreach (XmlNode node in list) {
                string date = node.Attributes["value"].Value.Substring(0, 10);
                foreach (XmlNode childNode in node.ChildNodes) {
                    WeatherInfo info = new WeatherInfo();
                    info.WindDirection = childNode.Attributes["D"].Value;
                    if (childNode.Attributes["H"] != null) {
                        info.ScreenRelativeHumidity = decimal.Parse(childNode.Attributes["H"].Value);
                    } else {
                        info.ScreenRelativeHumidity = 0;
                    }
                    if (childNode.Attributes["P"] != null) {
                        info.Pressure = ushort.Parse(childNode.Attributes["P"].Value);
                    } else {
                        info.Pressure = 0;
                    }
                    info.WindSpeed = ushort.Parse(childNode.Attributes["S"].Value);
                    if (childNode.Attributes["T"] != null) {
                        info.Temperature = decimal.Parse(childNode.Attributes["T"].Value);
                    } else {
                        info.Temperature = 0;
                    }
                    if (childNode.Attributes["W"] != null) {
                        info.WeatherType = ushort.Parse(childNode.Attributes["W"].Value);
                    }
                    if (childNode.Attributes["Pt"] != null) {
                        info.PressureTendency = childNode.Attributes["Pt"].Value;
                    } else {
                        info.PressureTendency = String.Empty;
                    }
                    if (childNode.Attributes["Dp"] != null) {
                        info.DewPoint = decimal.Parse(childNode.Attributes["Dp"].Value);
                    } else {
                        info.DewPoint = 0;
                    }
                    if (childNode.Attributes["G"] != null) {
                        info.WindGust = ushort.Parse(childNode.Attributes["G"].Value);
                    }
                    info.LoggedDateTime = new DateTime(int.Parse(date.Substring(0, 4)),
                        int.Parse(date.Substring(5, 2)),
                        int.Parse(date.Substring(8, 2)),
                        int.Parse(childNode.ChildNodes[0].Value) / 60, 0, 0);
                    infoList.Add(info);
                }
            }
            return infoList;
        }
    }
}
