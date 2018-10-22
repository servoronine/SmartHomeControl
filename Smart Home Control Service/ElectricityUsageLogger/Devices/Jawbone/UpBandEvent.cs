using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeControl.Devices.Jawbone {
    public enum BandAction { ENTER_SLEEP_MODE, EXIT_SLEEP_MODE, ENTER_STOPWATCH_MODE, EXIT_STOPWATCH_MODE, UNKNOWN_ACTION };

    public class UpBandEvent : UpBandDataItem {
        public int date;
        public string action;
        public string tz;
        public int time_created;

        public BandAction Action {
            get {
                switch (action) {
                    case "enter_sleep_mode":
                        return BandAction.ENTER_SLEEP_MODE;
                    case "exit_sleep_mode":
                        return BandAction.EXIT_SLEEP_MODE;
                    case "enter_stopwatch_mode":
                        return BandAction.ENTER_STOPWATCH_MODE;
                    case "exit_stopwatch_mode":
                        return BandAction.EXIT_STOPWATCH_MODE;
                    default:
                        return BandAction.UNKNOWN_ACTION;
                }
            }
        }

        public DateTime Date {
            get {
                return DateTime.ParseExact(date.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
            }
        }

        public DateTime TimeCreated {
            get {
                DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(time_created).ToLocalTime();
                return dtDateTime;
            }
        }
    }
}
