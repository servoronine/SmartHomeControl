using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartHomeControl.Devices.MetOffice {
    public class WeatherInfo {
        public string WindDirection;
        public decimal ScreenRelativeHumidity;
        public ushort Pressure;
        public ushort WindSpeed;
        public decimal Temperature;
        public ushort WeatherType;
        public string PressureTendency;
        public decimal DewPoint;
        public ushort WindGust;
        public DateTime LoggedDateTime;
    }
}
