using SmartHomeControl.Devices.Generic.Devices;
using SmartHomeControl.Devices.Generic.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SmartHomeControl.EventProcessor {
    public class ZoneList : List<GenericZone> {
        public GenericZone this[string zone] {
            get {
                foreach (GenericZone zn in this) {
                    if (zone == zn.zoneName) {
                        return zn;
                    }
                }
                return null;
            }
        }

        public ZoneList(XmlNode settings, GatewayList gateways, DeviceStateChangedDelegate stateChanged,
            FeedbackReceivedFromDeviceDelegate feedbackReceived) {
            foreach (XmlNode child in settings.ChildNodes) {
                GenericZone zn = new GenericZone(child, gateways, stateChanged, feedbackReceived);
                this.Add(zn);
            }
        }

        public List<GenericTrigger> GetListOfTriggers() {
            List<GenericTrigger> triggerList = new List<GenericTrigger>();
            foreach (GenericZone zone in this) {
                foreach (GenericDevice dev in zone.Devices) {
                    triggerList.AddRange(dev.GetListOfTriggers());
                }
            }
            return triggerList;
        }

        public GenericDeviceState GetCurrentState(string deviceName) {
            foreach (GenericZone zn in this) {
                foreach (GenericDevice dev in zn.Devices) {
                    if (dev.deviceName == deviceName && dev is IStatefulDevice) {
                        return ((IStatefulDevice)dev).GetCurrentState();
                    }
                }
            }
            return null;
        }
    }
}
