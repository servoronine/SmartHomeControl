using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Timers;

namespace SmartHomeControl.Helpers {
    public class HeartbeatInfo {
        private Socket destinationSocket;
        private DateTime lastMessageReceived;

        public Socket DestinationSocket {
            get {
                return destinationSocket;
            }
        }

        private byte[] heartbeatMessage;
        private Regex responseFormat;
        public BytesReceivedDelegate CallbackDelegate;
        private Timer timer;

        public HeartbeatInfo(Socket destinationSocket, byte[] heartbeatMessage, Regex responseFormat, 
            BytesReceivedDelegate callbackDelegate, int pollInterval) {
            this.destinationSocket = destinationSocket;
            destinationSocket.ReceiveTimeout = 3;
            this.heartbeatMessage = heartbeatMessage;
            this.responseFormat = responseFormat;
            this.CallbackDelegate += callbackDelegate;

            if (heartbeatMessage != null) {
                this.timer = new Timer();
                timer.Interval = pollInterval * 1000;
                timer.Elapsed += timer_Elapsed;
                timer.Enabled = true;
            }
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e) {
            lock (destinationSocket) {
                try {
                    TimeSpan ts = DateTime.Now - lastMessageReceived;
                    if (ts.Milliseconds > timer.Interval * 3) {
                        CallbackDelegate(String.Empty);
                        this.DestroyObject();
                    }
                    else {
                        destinationSocket.Send(heartbeatMessage);
                    }
                }
                catch (Exception ex) {
                    LoggingHelper.WriteExceptionLogEntry(ex.Source, ex);
                    CallbackDelegate(String.Empty);
                    this.DestroyObject();
                }
            }
        }

        public void ProcessMessageReceivedFromSocket(string bufferReceived) {
            if (bufferReceived != String.Empty) {
                lastMessageReceived = DateTime.Now;
            }
        }

        public void DestroyObject() {
            this.timer.Enabled = false;

            if (destinationSocket != null && destinationSocket.Connected) {
                try {
                    destinationSocket.Shutdown(SocketShutdown.Both);
                    destinationSocket.Close();
                }
                finally {
                    this.destinationSocket = null;
                }

            }
        }
    }
}
