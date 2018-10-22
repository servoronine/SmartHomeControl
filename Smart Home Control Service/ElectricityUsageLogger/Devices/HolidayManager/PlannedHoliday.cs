using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartHomeControl.Devices.HolidayManager {
    public class PlannedHoliday {
        public int HolidayID;
        private DateTime fromDate;
        public DateTime FromDate {
            get {
                return fromDate;
            }
            set {
                fromDate = value;
            }
        }

        private DateTime toDate;
        public DateTime ToDate {
            get {
                return toDate;
            }
            set {
                toDate = value;
            }
        }

        private bool isActioned;
        public bool IsActioned {
            get {
                return isActioned;
            }
            set {
                isActioned = value;
            }
        }

        public PlannedHoliday(int holidayID, DateTime fromDate, DateTime toDate, bool isActioned) {
            this.HolidayID = holidayID;
            this.FromDate = fromDate;
            this.ToDate = toDate;
            this.IsActioned = isActioned;
        }

        public PlannedHoliday() {
            this.HolidayID = 0;
        }

        public override string ToString() {
            string formatString = "yyyy-MM-dd HH:mm";
            return "from " + FromDate.DayOfWeek.ToString() + " " + FromDate.ToString(formatString) + 
                " to " + ToDate.DayOfWeek.ToString() + " " + ToDate.ToString(formatString);
        }
    }
}
