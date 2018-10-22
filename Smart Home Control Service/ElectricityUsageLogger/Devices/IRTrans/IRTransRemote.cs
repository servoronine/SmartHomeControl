using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRTrans.NET;
using System.Threading;
using System.Timers;
using System.Xml;
using SmartHomeControl.Devices.Generic.Devices;
using SmartHomeControl.Devices.Generic.Events;
using SmartHomeControl.Devices.Generic.Gateways;
using SmartHomeControl.Remotes.Generic;

namespace SmartHomeControl.Devices.IRTrans
{
    public class IRTransRemote : GenericIPDevice {
        private IRTransServer irt = null;
        private readonly object lockObject = new object();

        public IRTransRemote(XmlNode settings, GenericZone parentZone, GenericDeviceGateway gateway) : 
            base(settings, parentZone, gateway) {
        }

        protected override bool ConnectProviderSpecific() {
            try {
                if (Monitor.TryEnter(lockObject)) {
                    if (irt == null) {
                        irt = new IRTransServer(base.targetEndPoint.Address.ToString());
                        irt.StartAsnycReceiver();
                        irt.IRReceive += new IRTransServer.IRReceiveEventHandler(IRReceived);
                        irt.ConnectionLost += new IRTransServer.ConnectionLostEventHandler(irt_ConnectionLost);
                    }
                    Monitor.Exit(lockObject);
                    return true;
                }
                return false;
            }
            catch {
                Monitor.Exit(lockObject);
                return false;
            }
        }

        private void irt_ConnectionLost(object sender, EventArgs e) {
            irt.IRReceive -= new IRTransServer.IRReceiveEventHandler(IRReceived);
            irt.ConnectionLost -= new IRTransServer.ConnectionLostEventHandler(irt_ConnectionLost);
            irt = null;
            this.ReestablishConnection();
        }

        private void IRReceived(object sender, EventArgs e, NETWORKRECV recv) {
            RaiseDeviceEvent(this, new FeedbackReceivedFromDeviceEventArgs(recv.command, null));       
        }
    }
}
