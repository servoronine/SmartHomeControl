using SmartHomeControl.Devices.Generic.Devices;
using SmartHomeControl.Devices.Generic.Gateways;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using SmartHomeControl.Devices.Generic.Events;

namespace SmartHomeControl.Devices.PC {
    public class PC : GenericIPDevice, IStatefulDevice {
        #region Hard coded stuff - to remove later
        Ping ping = new Ping();
        Timer timer = new Timer(60000);
        #endregion

        private byte[] macAddress;
        private PCState currentState = new PCState();

        public event DeviceStateChangedDelegate StateChanged;

        public PC(XmlNode settings, GenericZone parentZone, GenericDeviceGateway gateway) : base(settings, parentZone, gateway) {
            macAddress = ConvertMacStringToByteArray(settings.Attributes["macAddress"].Value);
        }
        protected override bool ConnectProviderSpecific() {
            //Hard coded
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            return true;
        }

        public void ForceStateRefresh() {
            Timer_Elapsed(this, null);
        }

        public PCState CurrentState {
            get {
                return currentState;
            }
            set {
                this.currentState = value;
                if (StateChanged != null) {
                    StateChanged(this, new DeviceStateChangedEventArgs(this.currentState));
                }
            }
        }

        public GenericDeviceState GetCurrentState() {
            return currentState;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e) {
            lock (ping) {
                PingReply reply = ping.Send(targetEndPoint.Address);

                PCState state = new PCState();
                state.PCName = this.deviceName;
                if (reply.Status == IPStatus.Success) {
                    state.IsOn = true;
                }
                else {
                    state.IsOn = false;
                }
                CurrentState = state;
            }
        }

        private byte[] ConvertMacStringToByteArray(string macAddress) {
            string[] splitAddress = macAddress.Split(':');
            List<byte> macInBytes = new List<byte>();
            foreach (string str in splitAddress) {
                macInBytes.Add(Convert.ToByte("0x"+str, 16));
            }
            return macInBytes.ToArray();
        }

        public void TurnOn() {
            UdpClient client = new UdpClient();
            client.Connect(IPAddress.Broadcast, 9);

            byte[] packet = new byte[17 * 6];

            for (int i = 0; i < 6; i++)
                packet[i] = 0xFF;

            for (int i = 1; i <= 16; i++)
                for (int j = 0; j < 6; j++)
                    packet[i * 6 + j] = macAddress[j];

            client.Send(packet, packet.Length);
        }

        public void TurnOff() {
            string ipAddress = targetEndPoint.Address.ToString();
            Process.Start("cmd", @"/c shutdown -s -m \\" + ipAddress);
        }
    }
}
