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
using System.Threading;
using SmartHomeControl.Devices.Generic.Gateways;
using SmartHomeControl.Helpers;

namespace SmartHomeControl.Devices.LightwaveRF {
    public class LightwaveRFGateway : GenericIPGateway {
        private DateTime lastCommandSentTime;
        private IPEndPoint targetEndPoint;

        public LightwaveRFGateway(XmlNode settings)
            : base(settings) {

            string ipAddress = null;
            string port = null;
            foreach (XmlNode node in settings.ChildNodes) {
                if (node.Name == "Location") {
                    ipAddress = node.InnerText;
                } else if (node.Name == "Port") {
                    port = node.InnerText;
                }
            }
            this.targetEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress),
                int.Parse(port));
        }

        public void Register() {
            lastCommandSentTime = DateTime.Now;
            SocketsHelper.SendMessageToUDPSocket(localEndPoint, targetEndPoint, Encoding.ASCII.GetBytes("001,!F*p|"));
            //Old Command, apparently been replaced in the new firmware version
            //SocketsHelper.SendMessageToUDPSocket(localEndPoint, targetEndPoint, Encoding.ASCII.GetBytes("693,!R1Fa|"));
        }

        public void SendCommand(string commandText) {
            //LightwaveRF seems to get confused if commands are sent one directly after another so introducing
            //a pause of 500 milliseconds between commands
            TimeSpan ts = DateTime.Now - lastCommandSentTime;
            if (ts.Milliseconds < 500) {
                Thread.Sleep(500 - ts.Milliseconds);
            }
            SocketsHelper.SendMessageToUDPSocket(localEndPoint, targetEndPoint, Encoding.ASCII.GetBytes(commandText));
            lastCommandSentTime = DateTime.Now;
        }

        public void StartListeningToEnergyMeter(BytesReceivedDelegate EnergyMeterDataReceived) {
            IPEndPoint LocalHostIPEnd = new IPEndPoint(localEndPoint.Address,
                    targetEndPoint.Port + 1);

            SocketsHelper.SendMessageToUDPSocketAndListenToCallback(LocalHostIPEnd,
                targetEndPoint, null, EnergyMeterDataReceived);
        }
    }
}
