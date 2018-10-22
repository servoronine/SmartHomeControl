using SmartHomeControl.Devices.Generic.Events;
using SmartHomeControl.Devices.Generic.Gateways;
using SmartHomeControl.EventProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SmartHomeControl.Devices.Generic.Devices
{
    public class GenericZone
    {
        protected string _zoneName;
        protected int _zoneNumber;

        public string zoneName {
            get
            {
                return _zoneName;
            }
        }

        public int zoneNumber
        {
            get
            {
                return _zoneNumber;
            }
        }

        private List<GenericDevice> _devices = new List<GenericDevice>();
        public List<GenericDevice> Devices {
            get {
                return _devices;
            }
        }

        public GenericDevice this[string deviceName]
        {
            get
            {
                foreach (GenericDevice device in _devices)
                {
                    if (device.deviceName == deviceName)
                    {
                        return device;
                    }
                }
                return null;
            }
        }

        public GenericZone(XmlNode settings, GatewayList gateways, DeviceStateChangedDelegate stateChanged,
            FeedbackReceivedFromDeviceDelegate feedbackReceived)
        {
            _zoneName = settings.Attributes["name"].InnerText;
            _zoneNumber = int.Parse(settings.Attributes["number"].InnerText);

            foreach (XmlNode childNode in settings.ChildNodes)
            {
                GenericDeviceGateway gtw = gateways[childNode.Attributes["gateway"].InnerText];
                Type typ = Type.GetType(childNode.Attributes["type"].InnerText);
                GenericDevice dev = (GenericDevice)Activator.CreateInstance(typ, childNode, this, gtw);
                dev.DeviceEventRaised += feedbackReceived;
                if (dev is IStatefulDevice) {
                    ((IStatefulDevice)dev).StateChanged += stateChanged;
                    ((IStatefulDevice)dev).ForceStateRefresh();
                }

                _devices.Add(dev);
            }
        }

        public override string ToString()
        {
            return zoneName;
        }
    }
}
