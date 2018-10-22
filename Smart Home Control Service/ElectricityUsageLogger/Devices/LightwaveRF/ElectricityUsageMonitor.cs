using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Timers;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Xml;
using System.Web.Script.Serialization;
using SmartHomeControl.Devices.Generic.Devices;
using SmartHomeControl.Devices.Generic.Gateways;
using SmartHomeControl.Helpers;
using SmartHomeControl.EventProcessor;

namespace SmartHomeControl.Devices.LightwaveRF {
    class ElectricityUsageMonitor : GenericDevice {
        private LightwaveRFGateway gateway;
        public ElectricityUsageMonitor(XmlNode settings, GenericZone parentZone, GenericDeviceGateway gateway)
            : base(settings, parentZone, gateway) {
            this.gateway = (LightwaveRFGateway)gateway;
        }

        public void InitiateLogging() {
            this.gateway.StartListeningToEnergyMeter(ProcessReceivedData);
        }

        private void PostDataToDb(DateTime timeStamp, int currentReading, int dailyTotal) {
            try {
                SqlConnection connection = new SqlConnection(LocalSettings.SqlConnectionString);
                connection.Open();

                SqlCommand cmd = new SqlCommand("PostMeterReadings", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DateTime", SqlDbType.DateTime).Value = timeStamp;
                cmd.Parameters.Add("@CurrentReading", SqlDbType.Int).Value = currentReading;
                cmd.Parameters.Add("@DailyTotal", SqlDbType.Int).Value = dailyTotal;
                cmd.ExecuteNonQuery();

                connection.Close();
            }
            catch (Exception ex) {
                LoggingHelper.WriteExceptionLogEntry(ex.Source, ex);
            }
        }

        private void ProcessReceivedData(string receivedData) {
            if (receivedData.StartsWith("*!")) {
                string data = receivedData.Substring(2);
                try {
                    EnergyMeterData meterData = new JavaScriptSerializer().Deserialize<EnergyMeterData>(data);
                    PostDataToDb(meterData.timeConverted, meterData.cUse, meterData.todUse);
                }
                catch { }
            }
        }

        public void Register() {
            ((LightwaveRFGateway)gateway).Register();
        }
    }
}
