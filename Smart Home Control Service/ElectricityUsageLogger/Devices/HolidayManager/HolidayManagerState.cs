using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using SmartHomeControl.Devices.Generic.Devices;

namespace SmartHomeControl.Devices.HolidayManager {
    public class HolidayManagerState : GenericDeviceState
    {
        public CurrentHolidayStateEnum CurrentHolidayState = CurrentHolidayStateEnum.Unknown;

        private List<PlannedHoliday> plannedHolidays = new List<PlannedHoliday>();

        public List<PlannedHoliday> PlannedHolidays {
            get {
                return plannedHolidays;
            }
            set {
                throw new NotImplementedException();
            }
        }

        [StateResponseFormat("Your next planned holiday is set {0}", "You currently have no planned holidays")]
        public PlannedHoliday NextPlannedHoliday {
            get {
                foreach (PlannedHoliday plh in plannedHolidays) {
                    if (plh.FromDate > DateTime.Now) {
                        return plh;
                    }
                }
                return null;
            }
        }

        public PlannedHoliday GetHolidayStartingOn(DateTime fromDate) {
            foreach (PlannedHoliday plh in plannedHolidays) {
                if (plh.FromDate.Date == fromDate.Date) {
                    return plh;
                }
            }
            return null;
        }
    }
}
