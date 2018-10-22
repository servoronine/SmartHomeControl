using SmartHomeControl.Devices.Generic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeControl.Devices.Generic.Events {
    public class DeviceStateChangedEventArgs : EventArgs {
        public DeviceStateChangedEventArgs(GenericDeviceState state) {
            DeviceState = state;
        }

        public GenericDeviceState DeviceState { get; set; }
    }
}
