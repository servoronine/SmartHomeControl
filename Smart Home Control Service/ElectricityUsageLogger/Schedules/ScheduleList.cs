using SmartHomeControl.Devices.Generic.Events;
using SmartHomeControl.Devices.HolidayManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;

namespace SmartHomeControl.Schedules {
    public class ScheduleList : List<Schedule> {
        Timer schedTimer = new Timer(60000);
        public event FeedbackReceivedFromDeviceDelegate ScheduleFired; 

        public ScheduleList(XmlNode settings) {
            foreach (XmlNode child in settings.ChildNodes) {
                Schedule sched = new Schedule(child);
                this.Add(sched);
            }

            schedTimer.Elapsed += SchedTimer_Elapsed;
            schedTimer.Enabled = true;
        }

        private void SchedTimer_Elapsed(object sender, ElapsedEventArgs e) {
            DateTime checkDate = DateTime.Now;
            foreach (Schedule sched in this) {
                if (sched.ValidateIfDueToFire(checkDate) && 
                    ScheduleFired != null) {
                    ScheduleFired(sched, new FeedbackReceivedFromDeviceEventArgs(sched.ScheduleName, null));
                }
            }
        }

        public void ProcessChangedDeviceState(object sender, DeviceStateChangedEventArgs e) {
            if (e.DeviceState is HolidayManagerState) {
                foreach (Schedule sched in this) {
                    sched.UpdateHolidayState(((HolidayManagerState)e.DeviceState).CurrentHolidayState);
                }
            }
        }
    }
}
