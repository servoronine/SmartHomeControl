using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SmartHomeControl.Devices.Generic.Gateways;
using System.Net;
using SmartHomeControl.EventProcessor;
using System.Timers;

namespace SmartHomeControl.Devices.Generic.Devices {
    public class GenericIPDevice : GenericDevice {
        protected IPEndPoint targetEndPoint;
        protected IPEndPoint localEndPoint;
        public GenericIPDevice(XmlNode settings, GenericZone parentZone, GenericDeviceGateway gateway) : base(settings, parentZone, gateway) {
            IPAddress ipAddress = IPAddress.Parse(settings.Attributes["location"].Value);
            int port;
            if (settings.Attributes["port"] != null) {
                port = int.Parse(settings.Attributes["port"].Value);
            } else {
                port = 0;
            }

            if (settings.Attributes["retryDelay"] != null) {
                retryDelay = int.Parse(settings.Attributes["retryDelay"].Value);
            } else {
                retryDelay = 1000;
            }

            if (settings.Attributes["retryCount"] != null) {
                retryCount = int.Parse(settings.Attributes["retryCount"].Value);
            }
            else {
                retryCount = 0;
            }

            targetEndPoint = new IPEndPoint(ipAddress, port);
            localEndPoint = new IPEndPoint(LocalSettings.LocalIPAddress, 0);
            EstablishServerConnection();
        }

        private Timer aTimer = null;
        private int attemptCount = 0;
        private bool connected = false;
        protected int retryDelay;
        protected int retryCount;
        public event IRConnectStatusUpdateHandler statusUpdate;
        public event IRConnectStatusUpdateHandler connectedUpdate;

        private void InitializeTimer() {
            if (aTimer == null) {
                aTimer = new Timer();
                aTimer.Interval = retryDelay;
            }
            aTimer.Enabled = true;
            attemptCount = 0;
        }

        public void EstablishServerConnection() {
            InitializeTimer();
            aTimer.Elapsed += new ElapsedEventHandler(AttemptToEstablishConnection);
        }


        private object connectionLockObject = new object();
        private void AttemptToEstablishConnection(object sender, ElapsedEventArgs e) {
            lock (connectionLockObject) {
                if (!connected && (attemptCount < retryCount || retryCount == 0)) {
                    bool result = ConnectProviderSpecific();
                    if (result) {
                        connected = true;
                        aTimer.Enabled = false;
                    }
                    else {
                        attemptCount++;
                        if (statusUpdate != null) {
                            statusUpdate(this, new DeviceConnectionStatusEventArgs("Unable to connect (attempt " + attemptCount.ToString() + "). Retrying..."));
                        }
                    }
                }
                else if (!connected && attemptCount > retryCount) {
                    if (statusUpdate != null) {
                        statusUpdate(this, new DeviceConnectionStatusEventArgs("Unable to connect. Aborting..."));
                    }
                    aTimer.Enabled = false;
                }
                if (connected && connectedUpdate != null) {
                    connectedUpdate(this, new DeviceConnectionStatusEventArgs("Connected!"));
                }
            }
        }

        protected virtual bool ConnectProviderSpecific() {
            throw new Exception("ConnectProviderSpecific method MUST be overloaded in implementing class!");
        }

        protected void ReestablishConnection() {
            connected = false;
            aTimer.Enabled = true;
        }
    }
}
