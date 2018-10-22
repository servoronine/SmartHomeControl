using SmartHomeControl.Devices.Generic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHomeControl.Devices.Generic.Gateways;
using System.Xml;
using SmartHomeControl.EventProcessor;
using SmartHomeControl.Helpers;

namespace SmartHomeControl.Devices.Samsung {
    public class SamsungTV : GenericIPDevice {
        private const string remoteName = "SmartHomeControlRemote";
        private const string app = "SmartHomeControlApp";
        private const string tv = "UE40ES6800";

        public SamsungTV(XmlNode settings, GenericZone parentZone, GenericDeviceGateway gateway) : 
            base(settings, parentZone, gateway) {
        }

        protected override bool ConnectProviderSpecific() {
            return true;
        }

        public void SwitchTVOff() {
            SendKey("KEY_POWEROFF");
        }

        public void SendKey(string key) {
            SendConnectionMessageToTV();
            SendKeyMessageToTV(key);
        }

        private void SendConnectionMessageToTV() {
            SocketsHelper.SendMessageToTCPSocket(localEndPoint, targetEndPoint,
                Encoding.ASCII.GetBytes(ConstructConnectionMessage()));
        }

        private void SendKeyMessageToTV(string key) {
            SocketsHelper.SendMessageToTCPSocket(localEndPoint, targetEndPoint,
                Encoding.ASCII.GetBytes(ConstructKeyMessage(key)));
        }


        private string ConstructKeyMessage(string key) {
            string msg = Convert.ToChar(0x00).ToString() +
                Convert.ToChar(0x00).ToString() +
                Convert.ToChar(0x00).ToString() +
                ConstructKeyPacketFragment(key);
            string pkt = Convert.ToChar(0x00).ToString() +
                ConstructKeyPacketFragment(tv) +
                ConstructKeyPacketFragment(msg);
            return pkt;
        }

        private string ConstructConnectionMessage() {
            string connectMsg = Convert.ToChar(0x64).ToString() + Convert.ToChar(0x00).ToString() +
                ConstructKeyPacketFragment(this.localEndPoint.Address.ToString()) +
                ConstructKeyPacketFragment(LocalSettings.MacAddress) +
                ConstructKeyPacketFragment(remoteName);

            string connectPacket = Convert.ToChar(0x00) +
                ConstructKeyPacketFragment(app) +
                ConstructKeyPacketFragment(connectMsg);

            return connectPacket;
        }

        private string ConstructKeyPacketFragment(string str) {
            return ConvertIntToBase64(str.Length) +
            Convert.ToChar(0x00).ToString() +
            ConvertStringToBase64(str);
        }


        private string ConvertStringToBase64(string str) {
           return Convert.ToBase64String(
                    Encoding.UTF8.GetBytes(str)
                    );
        }

        private string ConvertIntToBase64(int val) {
            return Convert.ToChar(val).ToString();
        }
    }
}
