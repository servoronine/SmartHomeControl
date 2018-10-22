using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using SmartHomeControl.Devices.Generic.Devices;

namespace SmartHomeControl.Devices.Onkyo {
    [DataContract]
    public class OnkyoState : GenericDeviceState {
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

        public void ProcessFeedback(string command, string parameters) {
            switch (command) {
                case "NLS":
                    string infoType = parameters.Substring(0, 1);
                    switch (infoType) {
                        case "C":
                            if (parameters.Substring(parameters.Length - 1, 1) == "P") {
                                currentList.Clear();
                            }
                            break;
                        case "U":
                            int currentItem = int.Parse(parameters.Substring(1, 1));
                            while (currentItem + 1 > currentList.Count) {
                                currentList.Add("");
                            }
                            currentList[currentItem] = parameters.Substring(2);
                            break;
                    }
                    break;
                case "PWR":
                    if (parameters == "00") {
                        currentList.Clear();
                        currentPowerState = false;
                    }
                    else if (parameters == "01") {
                        currentPowerState = true;
                    }
                    break;
                case "MVL":
                    currentVolume = int.Parse(parameters, System.Globalization.NumberStyles.AllowHexSpecifier);
                    break;
                case "NTM":
                    currentSeekPosition = parameters;
                    break;
            }
        }
    }
}
