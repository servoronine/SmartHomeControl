using SmartHomeControl.Devices.Generic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHomeControl.Devices.Generic.Gateways;
using System.Xml;
using SmartHomeControl.Helpers;
using SmartHomeControl.Devices.Generic.Events;

namespace SmartHomeControl.Devices.GreenIQ {
    public class GreenIQHub : GenericWebDevice, IStatefulDevice {
        private string accessKey;
        private GreenIQHubState currentState;

        public event DeviceStateChangedDelegate StateChanged;

        public GreenIQHub(XmlNode settings, GenericZone parentZone, GenericDeviceGateway gateway) 
            : base(settings, parentZone, gateway) {
            accessKey = settings.Attributes["accessKey"].Value;
        }

        public void StartCapturingData() {

        }

        public void RequestData() {
            GreenIQConfigurationResponse resp =
                (GreenIQConfigurationResponse)WebClientHelper.RequestJSONData(typeof(GreenIQConfigurationResponse),
                 this.location + "get_valves_state_and_config.php?access_token=" + accessKey, null);
            GreenIQHubState state = new GreenIQHubState();
            state.PortsConfiguration = resp.data;
            currentState = state;
            this.StateChanged(this, new DeviceStateChangedEventArgs(state));
        }

        public void EnableValve(string name, int durationMins) {
            foreach (IrrigationPort port in currentState.PortsConfiguration.ports) {
                if (port.name == name) {
                    EnablePortMessage mes = new EnablePortMessage();
                    mes.number = port.number;
                    mes.configuration = PortConfigurationEnum.On;
                    mes.duration = durationMins;
                    WebClientHelper.PostJSONData(this.location + "php/api/set_valves_config.php?access_token="
                         + accessKey, mes, null);
                }
            }
        }

        public GenericDeviceState GetCurrentState() {
            return currentState;
        }

        public void ForceStateRefresh() {
            RequestData();
        }
    }
}
