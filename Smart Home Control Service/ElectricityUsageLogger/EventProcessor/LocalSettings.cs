using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Data.SqlClient;
using System.Data;
using System.Timers;
using SmartHomeControl.Devices.HolidayManager;
using SmartHomeControl.Remotes.Generic;

namespace SmartHomeControl.EventProcessor {
    public static class LocalSettings {
        public static string SqlConnectionString;
        public static IPAddress LocalIPAddress;
        private static Processor parentProcessor;
        public static string MacAddress;
        public static string HomeCoordinates;

        public static void InitializeParameters(IPAddress localIPAddress, 
            string sqlConnectionString, 
            Processor parentProcessor1, 
            string macAddress,
            string homeCoordinates) {
            SqlConnectionString = sqlConnectionString;
            LocalIPAddress = localIPAddress;
            parentProcessor = parentProcessor1;
            MacAddress = macAddress;
            HomeCoordinates = homeCoordinates;
        }
    }
}
