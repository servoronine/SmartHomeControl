using SmartHomeControl.Devices.Generic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHomeControl.Devices.Generic.Gateways;
using System.Xml;
using System.Threading;
using FuzzyString;
using SmartHomeControl.Devices.Generic.Events;

namespace SmartHomeControl.Devices.Sonos {
    public class SonosPlayerController : GenericDevice, IStatefulDevice {
        protected SonosGateway gtw;
        private string playerName;
        private SonosPlayerControllerState currentState = new SonosPlayerControllerState();

        public event DeviceStateChangedDelegate StateChanged;

        public SonosPlayerController(XmlNode settings, GenericZone parentZone, GenericDeviceGateway gateway) : 
            base(settings, parentZone, gateway) {
            gtw = gateway as SonosGateway;
            if (settings.Attributes["playerName"] != null) {
                playerName = settings.Attributes["playerName"].InnerText;
            }

            gtw.SonosPlayerFound += Gtw_SonosPlayerFound;

            if (gtw.FindPlayerByName(playerName) != null) {
                gtw.FindPlayerByName(playerName).StateChanged += SonosPlayerController_StateChanged;
            }           
        }

        private void Gtw_SonosPlayerFound(string deviceName) {
            if (deviceName == playerName) {
                gtw.FindPlayerByName(playerName).StateChanged += SonosPlayerController_StateChanged;
            }
        }

        private void SonosPlayerController_StateChanged(SonosPlayer obj) {
            currentState.CurrentSeekPosition = obj.CurrentState.RelTime.ToString();
            currentState.CurrentVolume = obj.CurrentState.Volume;
            currentState.CurrentMuteState = obj.CurrentState.MuteStatus;
            currentState.CurrentPowerState = obj.CurrentStatus == PlayerStatus.Playing ? true : false;

            List<string> stationList = new List<string>();
            IList<SonosItem> gtwList = gtw.GetSonosFavorites();
            if (gtwList != null) {
                foreach (SonosItem si in gtw.GetSonosFavorites()) {
                    stationList.Add(si.DIDL.Title);
                }
                currentState.CurrentList = stationList.ToArray();
            }

            if (StateChanged != null) {
                StateChanged(this, new DeviceStateChangedEventArgs(currentState));
            }
        }

        public void PlayFavorite(int favoriteNumber) {
            IList<SonosItem> si = gtw.GetSonosFavorites();

            if (favoriteNumber > si.Count) return;

            SonosPlayer pl = gtw.FindPlayerByName(playerName);
            if (pl != null) {

                pl.SetAVTransportURI(new SonosTrack {
                    Uri = si[favoriteNumber].Track.Uri,
                    MetaData = si[favoriteNumber].Track.MetaData
                });
                Thread.Sleep(1000);
                pl.Play();
            }
        }

        public void PlayFavoriteByName(string stationName) {
            IList<SonosItem> siList = gtw.GetSonosFavorites();

            Dictionary<double, int> matchCoef = new Dictionary<double, int>();
            foreach (SonosItem si in siList) {
                double ov = si.DIDL.Title.OverlapCoefficient(stationName);
                if (matchCoef.ContainsKey(ov)) {
                    ov += 0.0001;
                }
                matchCoef.Add(ov, siList.IndexOf(si));
            }

            List<double> ls = matchCoef.Keys.ToList();
            ls.Sort();

            if (ls[ls.Count - 1] >= 0.60) {
                PlayFavorite(matchCoef[ls[ls.Count - 1]]);
            }
        }

        public void StopPlayer() {
            SonosPlayer pl = gtw.FindPlayerByName(playerName);
            if (pl != null) {
                pl.Pause();
            }
        }

        public void Mute() {
            SonosPlayer pl = gtw.FindPlayerByName(playerName);
            if (pl != null) {
                pl.Mute();
            }
        }

        public void UnMute() {
            SonosPlayer pl = gtw.FindPlayerByName(playerName);
            if (pl != null) {
                pl.UnMute();
            }
        }

        public void SetVolume(ushort volume) {
            SonosPlayer pl = gtw.FindPlayerByName(playerName);
            if (pl != null) {
                pl.SetVolume(volume);
            }
        }

        public void VolumeUpVoice() {
            SetVolume((ushort)(currentState.CurrentVolume + 5));
        }

        public void VolumeDownVoice() {
            SetVolume((ushort)(currentState.CurrentVolume - 5));
        }

        public GenericDeviceState GetCurrentState() {
            return currentState;
        }

        public void ForceStateRefresh() {
            //throw new NotImplementedException();
        }
    }
}
