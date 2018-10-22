using SmartHomeControl.Devices.HolidayManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SmartHomeControl.Schedules {
    public class Schedule {
        public string ScheduleName { get; set; }
        public bool ValidDuringHoliday { get; set; }

        //The actual date here is random. All we will be extracting is day of week and time.
        public List<DateTime> TriggerTimes = new List<DateTime>();

        private CurrentHolidayStateEnum currentHolidayState;

        public Schedule(XmlNode settings) {
            ScheduleName = settings.Attributes["name"].InnerText;
            ValidDuringHoliday = settings.Attributes["validDuringHoliday"].InnerText == "true" ? true : false;

            if (settings.ChildNodes.Count == 1 && settings.ChildNodes[0].Name == "Daily") {
                LoadDailySchedule(settings.FirstChild);
            } else {
                LoadDetailedSchedule(settings.ChildNodes);
            }
        }

        private DateTime AddTimeToDate(DateTime date, string time) {
            string[] splitTime = time.Split(':');
            TimeSpan ts = new TimeSpan(int.Parse(splitTime[0]), int.Parse(splitTime[1]), 0);
            return date.Date + ts;
        }

        private DayOfWeek GetDayOfWeekFromText(string dowText) {
            DayOfWeek dow = DayOfWeek.Sunday;
            switch (dowText) {
                case "Mon":
                    dow = DayOfWeek.Monday;
                    break;
                case "Tue":
                    dow = DayOfWeek.Tuesday;
                    break;
                case "Wed":
                    dow = DayOfWeek.Wednesday;
                    break;
                case "Thu":
                    dow = DayOfWeek.Thursday;
                    break;
                case "Fri":
                    dow = DayOfWeek.Friday;
                    break;
                case "Sat":
                    dow = DayOfWeek.Saturday;
                    break;
                case "Sun":
                    dow = DayOfWeek.Sunday;
                    break;
            }

            return dow;
        }

        private void LoadDailySchedule(XmlNode node) {
            foreach (XmlNode timeNode in node.ChildNodes) {
                for (int i = 0; i < 7; i++) {
                    DateTime dateToAdd = DateTime.Today.AddDays(i);
                    TriggerTimes.Add(AddTimeToDate(dateToAdd, timeNode.InnerText));
                }
            }
        }

        private void LoadDetailedSchedule(XmlNodeList nodes) {
            foreach (XmlNode child in nodes) {
                DayOfWeek dow = GetDayOfWeekFromText(child.Name);

                DateTime today = DateTime.Today;
                int daysUntilNextSched = ((int)dow - (int)today.DayOfWeek + 7) % 7;
                DateTime nextSchedDay = today.AddDays(daysUntilNextSched);

                foreach (XmlNode childTime in child.ChildNodes) {
                    DateTime finalDate = AddTimeToDate(nextSchedDay, childTime.InnerText);
                    TriggerTimes.Add(finalDate);
                }
            }
        }

        public bool ValidateIfDueToFire(DateTime date) {
            foreach (DateTime schedTime in TriggerTimes) {
                if (schedTime.DayOfWeek == date.DayOfWeek && 
                    schedTime.Hour == date.Hour &&
                    schedTime.Minute == date.Minute) {

                    if (currentHolidayState != CurrentHolidayStateEnum.OnHoliday ||
                        ValidDuringHoliday) {
                        return true;
                    }
                }
            }
            return false;
        }

        public void UpdateHolidayState(CurrentHolidayStateEnum currState) {
            currentHolidayState = currState;
        }
    }
}
