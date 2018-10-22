using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using SmartHomeControl.Devices.Generic.Devices;

namespace SmartHomeControl.Devices.Heatmiser {
    public enum RunModes { NormalOperation, FrostProtection };
    public enum VendorID { Heatmiser, OEM };
    public enum TemperatureFormat { Celsius, Fahrenheit };
    public enum ProgramMode { ModeWeekdayWeekend, Mode7Day };
    public enum SensorSelection { builtIn };

    //Refer to page 26 of the protocol document for DCB description
    [DataContract]
    public class HeatmiserState : GenericDeviceState {
        byte[] rawDCB = null;
        byte[] originalDCB = null;

        [DataMember]
        public VendorID VendorID {
            get {
                return (rawDCB[2] == 0) ? VendorID.Heatmiser : VendorID.OEM;
            }
            set {
                throw new NotImplementedException();
            }
        }

        [DataMember]
        public byte Version {
            get {
                return rawDCB[3];
            }
            set {
                throw new NotImplementedException();
            }
        }

        [DataMember]
        public byte Model {
            get {
                return rawDCB[4];
            }
            set {
                throw new NotImplementedException();
            }
        }

        [DataMember]
        public TemperatureFormat TemperatureFormat {
            get {
                return (rawDCB[5] == 0) ? TemperatureFormat.Celsius : TemperatureFormat.Fahrenheit;
            }
            set {
                throw new NotImplementedException();
            }
        }

        [DataMember]
        public byte SwitchDifferential {
            get {
                return rawDCB[6];
            }
            set {
                throw new NotImplementedException();
            }
        }

        [DataMember]
        public bool FrostProtection {
            get {
                return (rawDCB[7] == 0) ? false : true;
            }
            set {
                throw new NotImplementedException();
            }
        }

        [DataMember]
        public byte OutputDelay {
            get {
                return rawDCB[10];
            }
            set {
                throw new NotImplementedException();
            }
        }

        [DataMember]
        public byte OptimumStart {
            get {
                return rawDCB[14];
            }
            set {
                throw new NotImplementedException();
            }
        }

        [DataMember]
        public byte RateOfChange {
            get {
                return rawDCB[15];
            }
            set {
                throw new NotImplementedException();
            }
        }

        [DataMember]
        public ProgramMode ProgramMode {
            get {
                return (rawDCB[16] == 0) ? ProgramMode.ModeWeekdayWeekend : ProgramMode.Mode7Day;
            }
            set {
                throw new NotImplementedException();
            }
        }

        [DataMember]
        public byte FrostProtectTemperature {
            get {
                return rawDCB[17];
            }
            set {
                if (value >= 7 && value <= 17) {
                    rawDCB[17] = value;
                }
                else {
                    throw new Exception("Frost Protect Temperature value not valid!");
                }
            }
        }

        [DataMember]
        [StateResponseFormat("Your thermostat is set to {0} degrees celsius", "")]
        public byte SetRoomTemp {
            get {
                return rawDCB[18];
            }
            set {
                if (value >= 5 && value <=35) {
                    rawDCB[18] = value;
                } else {
                    throw new Exception("Set room temperature value not valid!");
                }
            }
        }

        [DataMember]
        public bool OnOff {
            get {
                return (rawDCB[21] == 0) ? false : true;
            }
            set {
                rawDCB[21] = (!value) ? (byte)0 : (byte)1;
            }
        }

        [DataMember]
        public bool KeyLock {
            get {
                return (rawDCB[22] == 0) ? false : true;
            }
            set {
                rawDCB[22] = (!value) ? (byte)0 : (byte)1;
            }
        }

        [DataMember]
        public RunModes RunMode {
            get {
                return (rawDCB[23] == 0) ? RunModes.NormalOperation : RunModes.FrostProtection;
            }
            set {
                rawDCB[23] = (value == RunModes.NormalOperation) ? (byte)0 : (byte)1;
            }
        }

        [DataMember]
        public bool AwayMode {
            get {
                return (rawDCB[24] == 0) ? false : true;
            }
            set {
                rawDCB[24] = (!value) ? (byte)0 : (byte)1;
            }
        }

        [DataMember]
        public bool IsOnHoliday {
            get {
                return rawDCB[30] == 0 ? false : true;
            }
            set {
                throw new NotImplementedException();
            }
        }

        public void SetHoliday(DateTime returnDate) {

            rawDCB[25] = (byte)(returnDate.Year - 2000);
            rawDCB[26] = (byte)returnDate.Month;
            rawDCB[27] = (byte)returnDate.Day;
            rawDCB[28] = (byte)returnDate.Hour;
            rawDCB[29] = (byte)returnDate.Minute;
       }

        public void CancelHoliday() {
            rawDCB[30] = 0;
        }

        public DateTime HolidayReturnDate {
            get {
                return new DateTime(rawDCB[25] + 2000, rawDCB[26], rawDCB[27], rawDCB[28], rawDCB[29], 0);
            }
            set {
                throw new NotImplementedException();
            }
        }

        [DataMember]
        public ushort TempHold {
            get {
                return BitConverter.ToUInt16(rawDCB, 31);
            }
            set {
                rawDCB[31] = BitConverter.GetBytes(value)[1];
                rawDCB[32] = BitConverter.GetBytes(value)[0];
            }
        }

        [DataMember]       
        public ushort CurrentAirTemp {
            get {
                return BitConverter.ToUInt16(rawDCB, 37);
            }
            set {
                throw new NotImplementedException();
            }
        }

        [DataMember]
        [StateResponseFormat("Current temperature in your house is {0} degrees celsius", "")]
        public string CurrentAirTempStr {
            get {
                return CurrentAirTemp.ToString().Insert(2, ".");
            }
        }


        [DataMember]
        public bool IsHeating {
            get {
                return (rawDCB[40] == 0) ? false : true;
            }
            set {
                throw new NotImplementedException();
            }
        }

        [DataMember]
        [StateResponseFormat("Heating us currently {0}", "")]
        public string IsHeatingStr {
            get {
                return IsHeating ? "on" : "off";
            }
        }

        [DataMember]
        public ushort Boost {
            get {
                return BitConverter.ToUInt16(rawDCB, 41);
            }
            set {
                rawDCB[41] = BitConverter.GetBytes(value)[0];
                rawDCB[42] = BitConverter.GetBytes(value)[1];
            }
        }

        [DataMember]
        public bool IsHotWater {
            get {
                return (rawDCB[43] == 0 || rawDCB[43] == 2) ? false : true;
            }
            set {
                throw new NotImplementedException();
            }
        }

        [DataMember]
        [StateResponseFormat("Hot water is currently {0}", "")]
        public string IsHotWaterStr {
            get {
                return IsHotWater ? "on" : "off";
            }
        }


        public void SetHotWaterState(byte hotWaterState) {
            if (hotWaterState >= 0 && hotWaterState <= 2) {
                rawDCB[43] = hotWaterState;
            }
            else {
                throw new Exception("Hot water state value not valid!");
            }
        }

        [DataMember]
        public DateTime SystemTime {
            get {
                return new DateTime(rawDCB[44] + 2000, rawDCB[45], rawDCB[46], rawDCB[48], rawDCB[49], rawDCB[50]);
            }
            set {
                rawDCB[44] = (byte)(value.Year - 2000);
                rawDCB[45] = (byte)value.Month;
                rawDCB[46] = (byte)value.Day;
                rawDCB[47] = (byte)value.DayOfWeek == 0 ? (byte)7 : (byte)value.DayOfWeek;
                rawDCB[48] = (byte)value.Hour;
                rawDCB[49] = (byte)value.Minute;
                rawDCB[50] = (byte)value.Second;
            }
        }

        public int FindSetTemperatureByTimeAndDay(DayOfWeek day, TimeSpan time) {
            if (HeatingTimes.Length == 8) {
                if ((int)day >=1 && (int)day <= 5) {
                    return FindSetTemperatureByStartingIndex(time, 0);
                } else {
                    return FindSetTemperatureByStartingIndex(time, 4);
                }
            } else {
                return FindSetTemperatureByStartingIndex(time, ((int)day - 1) * 4);
            }
        }

        private int FindSetTemperatureByStartingIndex(TimeSpan targetTime, int startIndex) {
            TimeSpan htSpan = new TimeSpan(HeatingTimes[startIndex].Hour, HeatingTimes[startIndex].Minute, 0);

            if (htSpan > targetTime) {
                return HeatingTimes[3].Temperature;
            }

            for (int i = 3 + startIndex; i >= startIndex; i--) {
                htSpan = new TimeSpan(HeatingTimes[i].Hour, HeatingTimes[i].Minute, 0);

                if (htSpan < targetTime) {
                    return HeatingTimes[i].Temperature;
                }
            }
            return 0;
        }

        public TemperatureSwitchSetting[] HeatingTimes {
            get {
                TemperatureSwitchSetting[] switchSetting = null;
                if (this.ProgramMode == ProgramMode.ModeWeekdayWeekend) {
                    switchSetting = new TemperatureSwitchSetting[8];
                    for (int i = 0; i < 8; i++) {
                        TemperatureSwitchSetting setting = new TemperatureSwitchSetting();
                        setting.Hour = rawDCB[i * 3 + 51];
                        setting.Minute = rawDCB[i * 3 + 52];
                        setting.Temperature = rawDCB[i * 3 + 53];
                        switchSetting[i] = setting;
                    }
                }
                else {
                    switchSetting = new TemperatureSwitchSetting[28];
                    for (int i = 0; i < 28; i++) {
                        TemperatureSwitchSetting setting = new TemperatureSwitchSetting();
                        setting.Hour = rawDCB[i * 3 + 107];
                        setting.Minute = rawDCB[i * 3 + 108];
                        setting.Temperature = rawDCB[i * 3 + 109];
                        switchSetting[i] = setting;
                    }
                }
                return switchSetting;
            }
            set {
                if (this.ProgramMode == ProgramMode.ModeWeekdayWeekend && value.Length == 8) {
                    for (int i = 0; i < 8; i++) {
                        rawDCB[i*3 + 51] = value[i].Hour;
                        rawDCB[i*3 + 52] = value[i].Minute;
                        rawDCB[i*3 + 53] = value[i].Temperature;
                    }
                } else if (this.ProgramMode == ProgramMode.Mode7Day && value.Length == 28) {
                    for (int i = 0; i < 28; i++) {
                        rawDCB[i*3 + 107] = value[i].Hour;
                        rawDCB[i*3 + 108] = value[i].Minute;
                        rawDCB[i*3 + 109] = value[i].Temperature;
                    }
                } else {
                    throw new Exception("Temperature timings incorrect!");
                }
            }
        }


        public SwitchSetting[] HotWaterTimes {
            get {
                SwitchSetting[] switchSetting = null;
                if (this.ProgramMode == ProgramMode.ModeWeekdayWeekend) {
                    switchSetting = new SwitchSetting[16];
                    for (int i = 0; i < 16; i++) {
                        SwitchSetting setting = new SwitchSetting();
                        setting.Hour = rawDCB[i * 2 + 75];
                        setting.Minute = rawDCB[i * 2 + 76];
                        switchSetting[i] = setting;
                    }
                }
                else {
                    switchSetting = new SwitchSetting[56];
                    for (int i = 0; i < 56; i++) {
                        SwitchSetting setting = new SwitchSetting();
                        setting.Hour = rawDCB[i * 2 + 191];
                        setting.Minute = rawDCB[i * 2 + 192];
                        switchSetting[i] = setting;
                    }
                }
                return switchSetting;
            }
            set {
                if (this.ProgramMode == ProgramMode.ModeWeekdayWeekend && value.Length == 8) {
                    for (int i = 0; i < 16; i++) {
                        rawDCB[i * 2 + 75] = value[i].Hour;
                        rawDCB[i * 2 + 76] = value[i].Minute;
                    }
                }
                else if (this.ProgramMode == ProgramMode.Mode7Day && value.Length == 28) {
                    for (int i = 0; i < 56; i++) {
                        rawDCB[i * 2 + 191] = value[i].Hour;
                        rawDCB[i * 2 + 192] = value[i].Minute;
                    }
                }
                else {
                    throw new Exception("Temperature timings incorrect!");
                }
            }
        }

        public void SetFromDCB(byte[] dcb) {
            this.rawDCB = dcb;

            this.originalDCB = new byte[dcb.Length];
            dcb.CopyTo(originalDCB, 0);
        }

        public List<HeatmiserStateValueChange> GetChangedValues() {
            List<HeatmiserStateValueChange> listOfChangedValues = new List<HeatmiserStateValueChange>();
            for (ushort i = 0; i < rawDCB.Length; i++) {
                if (rawDCB[i] != originalDCB[i]) {
                    HeatmiserStateValueChange changedValues = new HeatmiserStateValueChange();
                    List<byte> changedData = new List<byte>();

                    switch (i) {
                        case 17:
                        case 18:
                        case 21:
                        case 22:
                        case 23:
                            changedValues.Address = i;
                            changedValues.NumberOfBytes = 1;
                            changedValues.Contents = new byte[] {rawDCB[i]};
                            i++;
                            break;
                        case 24:
                            changedValues.Address = 31;
                            changedValues.NumberOfBytes = 1;
                            changedValues.Contents = new byte[] {rawDCB[i]};
                            i++;
                            break;
                        case 43:
                            changedValues.Address = 42;
                            changedValues.NumberOfBytes = 1;
                            changedValues.Contents = new byte[] {rawDCB[i]};
                            i++;
                            break;
                        case 25:
                        case 26:
                        case 27:
                        case 28:
                        case 29:
                        case 30:
                            changedValues.Address = 24;
                            if (i != 30) {
                                i = 25;
                                changedValues.NumberOfBytes = 5;
                                changedValues.Contents = new byte[5];
                                Array.Copy(rawDCB, 25, changedValues.Contents, 0, 5);
                            }
                            else {
                                changedValues.NumberOfBytes = 1;
                                changedValues.Contents = new byte[] {rawDCB[30]};
                            }
                            i = (ushort)(i + 5);
                            break;
                        case 31:
                            changedValues.Address = 32;
                            changedValues.NumberOfBytes = 2;
                            changedValues.Contents = new byte[] { rawDCB[32], rawDCB[31] };
                            i = (ushort)(i + 2);
                            break;
                        case 41:
                            changedValues.Address = 25;
                            changedValues.NumberOfBytes = 2;
                            changedValues.Contents = new byte[] { rawDCB[41], rawDCB[42] };
                            i = (ushort)(i + 2);
                            break;
                        case 44:
                        case 45:
                        case 46:
                        case 47:
                        case 48:
                        case 49:
                        case 50:
                            i = 44;
                            changedValues.Address = 43;
                            changedValues.NumberOfBytes = 7;
                            changedValues.Contents = new byte[7];
                            Array.Copy(rawDCB, 44, changedValues.Contents, 0, 7);
                            i = (ushort)(i + 7);
                            break;
                        case 51:
                        case 63:
                        case 107:
                        case 119:
                        case 131:
                        case 143:
                        case 155:
                        case 167:
                        case 179:
                            //This will need to be changed as it assumes that hour always changes
                            changedValues.Address = (byte)(i - 4);
                            changedValues.NumberOfBytes = 12;
                            changedValues.Contents = new byte[12];
                            for (int b = i; b < i + 12; b++) {
                                changedValues.Contents[b - i] = rawDCB[b];
                            }
                            i = (ushort)(i + 12);
                            break;
                        case 75:
                        case 91:
                        case 191:
                        case 207:
                        case 223:
                        case 239:
                        case 255:
                        case 271:
                        case 287:
                            //This will need to be changed as it assumes that hour always changes
                            changedValues.Address = (byte)(i - 4);
                            changedValues.NumberOfBytes = 16;
                            changedValues.Contents = new byte[16];
                            for (int b = i; b < i + 16; b++) {
                                changedValues.Contents[b - i] = rawDCB[b];
                            }
                            i = (ushort)(i + 16);
                            break;
                    }
                    listOfChangedValues.Add(changedValues);
                }
            }

            return listOfChangedValues;
        }
    }
}
