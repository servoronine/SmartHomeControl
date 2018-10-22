using SmartHomeControl.Devices.Generic.Devices;
using SmartHomeControl.Devices.Generic.Gateways;
using SmartHomeControl.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using SmartHomeControl.Devices.Generic.Events;

namespace SmartHomeControl.Devices.Jawbone {
    public class JawboneUP3 : GenericWebDevice, IStatefulDevice {
        private Timer timer;

        private JawboneState currentState = new JawboneState();
        private string jawboneKey;

        private int pollInterval;

        public event DeviceStateChangedDelegate StateChanged;

        public int PollInterval {
            get {
                return pollInterval;
            }
        }

        public GenericDeviceState GetCurrentState() {
            return currentState;
        }

        public void ForceStateRefresh() {
            OnTimedEvent(this, null);
        }

        public void RequestData() {
            currentState =
                (JawboneState)WebClientHelper.RequestJSONData(typeof(JawboneState),
                this.location + "bandevents",
                jawboneKey);
            if (StateChanged != null) {
                StateChanged(this, new DeviceStateChangedEventArgs(currentState));
            }
        }

        public JawboneUP3(XmlNode settings, GenericZone parentZone, GenericDeviceGateway gateway)
            : base(settings, parentZone, gateway) {
            this.pollInterval = int.Parse(settings.Attributes["loggingPollInterval"].Value);
            this.jawboneKey = settings.Attributes["accessKey"].Value;
        }

        public void StartCapturingData() {
            InitializeTimer();
        }

        private void InitializeTimer() {
            timer = new Timer(PollInterval * 1000);
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Enabled = true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e) {
            RequestData();
        }
    }
}
