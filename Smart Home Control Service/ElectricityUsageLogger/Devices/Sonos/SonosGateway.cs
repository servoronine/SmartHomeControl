using FuzzyString;
using SmartHomeControl.Devices.Generic.Gateways;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace SmartHomeControl.Devices.Sonos {
    public class SonosGateway : GenericDeviceGateway {
        protected SonosDiscovery dis = new SonosDiscovery();
        public event SonosDeviceFound SonosPlayerFound;
        public SonosGateway(XmlNode settings) : 
            base(settings) {
            dis.PlayerFound += Dis_PlayerFound;
            dis.StartScan();
            //Thread.Sleep(5000);
        }

        private void Dis_PlayerFound(string deviceName) {
            if (SonosPlayerFound != null) {
                SonosPlayerFound(deviceName);
            }
        }

        public IList<SonosItem> GetSonosFavorites() {
            try {
                return dis.Zones[0].Coordinator.GetFavorites();
            } catch {
                return null;
            }
        }

        public SonosPlayer FindPlayerByName(string playerName) {
            foreach (SonosPlayer pl in dis.Players) {
                if (pl.Name.ToLower() == playerName.ToLower()) {
                    return pl;
                }
            }
            return null;
        }
    }
}
