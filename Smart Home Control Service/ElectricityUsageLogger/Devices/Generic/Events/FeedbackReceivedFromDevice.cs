﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeControl.Devices.Generic.Events {
    public delegate void FeedbackReceivedFromDeviceDelegate(object sender, FeedbackReceivedFromDeviceEventArgs e);
}
