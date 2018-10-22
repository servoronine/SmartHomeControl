using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace SmartHomeControl.Helpers {
    public static class LoggingHelper {
        private const string logFileName = "loggerlog.log";

        public static void LogExceptionInApplicationLog(string source, Exception ex, EventLogEntryType type) {
            EventLog appLog = new EventLog();
            appLog.Source = source;
            appLog.WriteEntry("Error occured: " + ex.Message, type);
        }

        public static void WriteExceptionLogEntry(string source, Exception ex) {
            lock (logFileName) {
                File.AppendAllText(logFileName, "Error occured: " + source + "; Message: " + ex.Message + "; Stack trace:\r\n" + ex.StackTrace + "\r\n\n");
            }
        }

        public static void WriteInformationLogEntry(string source, string message) {
            lock (logFileName) {
                File.AppendAllText(logFileName, "Information message from: " + source + "; Message: " + message + "\r\n\n");
            }
        }
    }
}
