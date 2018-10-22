using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeControl.Devices.Sonos {
    public class SonosPlayerState {
        public string TransportState { get; set; }
        public string NumberOfTracks { get; set; }
        public string CurrentTrack { get; set; }
        public TimeSpan CurrentTrackDuration { get; set; }
        public string CurrentTrackMetaData { get; set; }
        public DateTime LastStateChange { get; set; }
        public TimeSpan RelTime { get; set; }
        public string NextTrackMetaData { get; set; }
        public bool MuteStatus { get; set; }
        public ushort Volume { get; set; }
    }
}
