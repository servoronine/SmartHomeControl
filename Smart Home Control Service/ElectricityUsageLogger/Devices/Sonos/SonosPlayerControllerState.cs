using SmartHomeControl.Devices.Generic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeControl.Devices.Sonos {
    public class SonosPlayerControllerState  : GenericDeviceState {
        private List<string> currentList = new List<string>();
        [DataMember]
        public string[] CurrentList {
            get {
                return currentList.ToArray();
            }
            set {
                currentList = new List<string>(value);
            }
        }

        private string currentSeekPosition;
        [DataMember]
        public string CurrentSeekPosition {
            get {
                return currentSeekPosition;
            }
            set {
                currentSeekPosition = value;
            }
        }

        private int currentVolume;
        [DataMember]
        public int CurrentVolume {
            get {
                return currentVolume;
            }
            set {
                currentVolume = value;
            }
        }

        bool currentPowerState;
        [DataMember]
        public bool CurrentPowerState {
            get {
                return currentPowerState;
            }
            set {
                currentPowerState = value;
            }
        }

        public bool CurrentMuteState { get; set; }
    }
}
