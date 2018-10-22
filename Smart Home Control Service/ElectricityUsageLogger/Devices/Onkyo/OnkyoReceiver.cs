using SmartHomeControl.Devices.Generic.Devices;
using SmartHomeControl.Devices.Generic.Gateways;
using SmartHomeControl.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmartHomeControl.Devices.Generic.Events;
using System.Threading;

namespace SmartHomeControl.Devices.Onkyo {
    public class OnkyoReceiver : GenericIPDevice, IStatefulDevice {
        private OnkyoState currentState = new OnkyoState();

        public event DeviceStateChangedDelegate StateChanged;

        public OnkyoReceiver(XmlNode settings, GenericZone parentZone, GenericDeviceGateway gateway)
            : base(settings, parentZone, gateway) {
        }

        protected override bool ConnectProviderSpecific() {
            return InitiateConnectionToReceiver();
        }

        public void SwitchReceiverOnAndOpenNetworkFavorites() {
            SwitchReceiverOn();
            OpenNetworkFavorites();
        }

        public void PlaySelectedStation(int stationNumber) {
            SelectItemFromTheList(stationNumber);
            System.Threading.Thread.Sleep(200);
            ClickOk();
        }

        public override void ToggleDeviceState(int state) {
            switch (state) {
                case 0:
                    SwitchReceiverOff();
                    break;
                case 1:
                    SwitchReceiverOn();
                    break;
            }
        }


        public GenericDeviceState GetCurrentState() {
            return currentState;
        }

        public void SwitchReceiverOn() {
            if (!currentState.CurrentPowerState) {
                SocketsHelper.SendMessageToTCPSocket(localEndPoint, targetEndPoint,
                    ContructReceiverMessage("PWR", "01"));
                //System.Threading.Thread.Sleep(7000);
            }
        }

        public void SwitchReceiverOff() {
            SocketsHelper.SendMessageToTCPSocket(localEndPoint, targetEndPoint,
                ContructReceiverMessage("PWR", "00"));
            SocketsHelper.SendMessageToTCPSocket(localEndPoint, targetEndPoint,
                ContructReceiverMessage("PWR", "QSTN"));
        }

        public void VolumeUp() {
            SocketsHelper.SendMessageToTCPSocket(localEndPoint, targetEndPoint,
                ContructReceiverMessage("MVL", "UP"));
            Thread.Sleep(500);
            SocketsHelper.SendMessageToTCPSocket(localEndPoint, targetEndPoint,
                ContructReceiverMessage("MVL", "QSTN"));
        }

        public void VolumeUpVoice() {
            SetVolume(currentState.CurrentVolume + 5);
        }

        public void VolumeDownVoice() {
            SetVolume(currentState.CurrentVolume - 5);
        }

        public void SetVolume(int level) {
            SocketsHelper.SendMessageToTCPSocket(localEndPoint, targetEndPoint,
                ContructReceiverMessage("MVL", level.ToString("X")));
            Thread.Sleep(500);
            SocketsHelper.SendMessageToTCPSocket(localEndPoint, targetEndPoint,
                ContructReceiverMessage("MVL", "QSTN"));
        }

        public void VolumeDown() {
            SocketsHelper.SendMessageToTCPSocket(localEndPoint, targetEndPoint,
                ContructReceiverMessage("MVL", "DOWN"));
            Thread.Sleep(500);
            SocketsHelper.SendMessageToTCPSocket(localEndPoint, targetEndPoint,
                ContructReceiverMessage("MVL", "QSTN"));
        }

        public void Stop() {
            SocketsHelper.SendMessageToTCPSocket(localEndPoint, targetEndPoint,
                ContructReceiverMessage("NTC", "STOP"));
        }

        public void Back() {
            SocketsHelper.SendMessageToTCPSocket(localEndPoint, targetEndPoint,
                ContructReceiverMessage("NTC", "RETURN"));
        }

        public void SwitchTVOn() {
            SocketsHelper.SendMessageToTCPSocket(localEndPoint, targetEndPoint,
                ContructReceiverMessage("CTV", "PWRON"));
        }
        public void SwitchTVOff() {
            SocketsHelper.SendMessageToTCPSocket(localEndPoint, targetEndPoint,
                ContructReceiverMessage("CTV", "PWROFF"));
        }

        public void ForceStateRefresh() {
            InitiateConnectionToReceiver();
        }

        public bool InitiateConnectionToReceiver() {
            SocketsHelper.ConnectToTCPSocketAndListenToCallback(localEndPoint, targetEndPoint, ProcessReceivedMessage,
                ContructReceiverMessage("PWR", "QSTN"), null, ProcessReceivedMessage);
            SocketsHelper.SendMessageToTCPSocket(localEndPoint, targetEndPoint,
                ContructReceiverMessage("PWR", "QSTN"));
            SocketsHelper.SendMessageToTCPSocket(localEndPoint, targetEndPoint,
                ContructReceiverMessage("MVL", "QSTN"));
            return true;
        }

        public void OpenNetworkFavorites() {
            SocketsHelper.SendMessageToTCPSocket(localEndPoint, targetEndPoint,
                ContructReceiverMessage("SLI", "28"));
            //System.Threading.Thread.Sleep(1500);
        }

        public void Mute() {
            SocketsHelper.SendMessageToTCPSocket(localEndPoint, targetEndPoint,
                ContructReceiverMessage("AMT", "01"));
        }

        public void UnMute() {
            SocketsHelper.SendMessageToTCPSocket(localEndPoint, targetEndPoint,
               ContructReceiverMessage("AMT", "00"));
        }

        public void OpenTvCd() {
            SocketsHelper.SendMessageToTCPSocket(localEndPoint, targetEndPoint,
                ContructReceiverMessage("SLI", "23"));
        }

        public void SelectItemFromTheList(int itemNumber) {
            SocketsHelper.SendMessageToTCPSocket(localEndPoint, targetEndPoint,
            ContructReceiverMessage("NLS", "L" + itemNumber.ToString()));
        }

        public void ClickOk() {
            SocketsHelper.SendMessageToTCPSocket(localEndPoint, targetEndPoint,
                ContructReceiverMessage("NTC", "SELECT"));
        }

        private void ProcessReceivedMessage(string content) {
            string[] separators = new string[] { "\r\n" };
            List<string> commands = content.Split(separators, StringSplitOptions.RemoveEmptyEntries).ToList();
            try {
                for (int i = 0; i < commands.Count; i++) {
                    string command = String.Empty;
                    string parameters = String.Empty;
                    if (commands[i].Length > 18) {
                        commands[i] = commands[i].Substring(18);
                        command = commands[i].Substring(0, 3);
                        parameters = commands[i].Substring(3, commands[i].Length - 1 - 3);
                    }
                    else {
                        command = commands[i];
                    }
                    currentState.ProcessFeedback(command, parameters);
                    StateChanged(this, new DeviceStateChangedEventArgs(this.currentState));
                }
            } catch {
                //Need to fix the issue with processing feedback - it appears that partial messages are being received
                //potentially ip buffer is too small or need to be linked across multiple packets
                //TBD
            }
        }

        private byte[] ContructReceiverMessage(string command, string parameter) {
            List<byte> fullCommand = new List<byte>();
            fullCommand.AddRange(Encoding.ASCII.GetBytes("ISCP"));
            fullCommand.AddRange(GetIntInBigEndianFormat(0x00000010));
            //Contrary to the document, this is the full size of the message (including the header)
            fullCommand.AddRange(GetIntInBigEndianFormat(command.Length + parameter.Length + 2 + 1 + 16));
            fullCommand.Add(0x01);
            fullCommand.AddRange(BitConverter.GetBytes(0x000000));
            fullCommand.AddRange(Encoding.ASCII.GetBytes("!1"));
            fullCommand.AddRange(Encoding.ASCII.GetBytes(command));
            fullCommand.AddRange(Encoding.ASCII.GetBytes(parameter));
            fullCommand.Add(0x0d);
            return fullCommand.ToArray();
        }

        private byte[] GetIntInBigEndianFormat(int number) {
            if (BitConverter.IsLittleEndian) {
                return BitConverter.GetBytes(number).Reverse().ToArray();
            }
            else {
                return BitConverter.GetBytes(number);
            }
        }
    }
}
