using SmartHomeControl.Devices.Generic.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartHomeControl.Devices.Generic.Devices {
    public interface IStatefulDevice {
        GenericDeviceState GetCurrentState();

        event DeviceStateChangedDelegate StateChanged;

        void ForceStateRefresh();
    }  
}
