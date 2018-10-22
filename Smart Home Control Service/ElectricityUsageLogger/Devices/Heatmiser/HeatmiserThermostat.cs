using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using SmartHomeControl.Devices.Generic.Devices;
using SmartHomeControl.Devices.Generic.Gateways;
using SmartHomeControl.Devices.HolidayManager;
using System.Threading;
using System.Net.Sockets;
using SmartHomeControl.Helpers;
using SmartHomeControl.EventProcessor;
using SmartHomeControl.Devices.Generic.Events;

namespace SmartHomeControl.Devices.Heatmiser {
    public class HeatmiserThermostat : GenericIPDevice, IStatefulDevice {
        private System.Timers.Timer timer;

        public HeatmiserThermostat(XmlNode settings, GenericZone parentZone, GenericDeviceGateway gateway) : base(settings, parentZone, gateway) {
            this.pollInterval = int.Parse(settings.Attributes["loggingPollInterval"].Value);
            this.pinCode = ushort.Parse(settings.Attributes["pinCode"].Value);
        }

        protected override bool ConnectProviderSpecific() {
            InitializeTimer();
            return true;
        }

        public void ForceStateRefresh() {
            OnTimedEvent(this, null);
        }

        private void InitializeTimer() {
            if (timer == null) {
                timer = new System.Timers.Timer(PollInterval * 1000);
                timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                timer.Enabled = true;
            }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e) {
            HeatmiserState state = RequestData();
            if (logState) {
                PostLogToDatabase(state);
            }

            if (state != null) {
                ResetReceiverIfPerceivedHanging();
            }
         }

        private void PostLogToDatabase(HeatmiserState state) {
            try {
                SqlConnection connection = new SqlConnection(LocalSettings.SqlConnectionString);
                connection.Open();

                SqlCommand cmd = new SqlCommand("PostHeatmiserLog", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DateTime", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@RateOfChange", SqlDbType.TinyInt).Value = state.RateOfChange;
                cmd.Parameters.Add("@SetRoomTemp", SqlDbType.TinyInt).Value = state.SetRoomTemp;
                cmd.Parameters.Add("@RunMode", SqlDbType.TinyInt).Value = state.RunMode;
                cmd.Parameters.Add("@CurrentAirTemp", SqlDbType.SmallInt).Value = state.CurrentAirTemp;
                cmd.Parameters.Add("@IsHeating", SqlDbType.Bit).Value = state.IsHeating;
                cmd.Parameters.Add("@IsHotWater", SqlDbType.Bit).Value = state.IsHotWater;
                cmd.ExecuteNonQuery();

                connection.Close();
            }
            catch (Exception ex) {
                LoggingHelper.WriteExceptionLogEntry(ex.Source, ex);
            }
        }


        private bool logState = false;
        public void LogHeatmiserState() {
            logState = true;
        }

        public override void SetHolidayMode(PlannedHoliday plannedHoliday) {
            SetHoliday(plannedHoliday);
        }

        public void SetAtHomeMode() {
            CancelHoliday();
        }

        public void IncreaseTempByOneDegree() {
            byte setRoomTemp = CurrentState.SetRoomTemp;
            SetTemperature((byte)(setRoomTemp + 1));
        }

        public void DecreaseTempByOneDegree() {
            byte setRoomTemp = CurrentState.SetRoomTemp;
            SetTemperature((byte)(setRoomTemp - 1));
        }

        public List<HeatmiserTempReading> GetListOfReadings(DateTime fromDate) {
            try {
                SqlConnection connection = new SqlConnection(LocalSettings.SqlConnectionString);
                connection.Open();

                SqlCommand cmd = new SqlCommand("GetHeatmiserReadingBasedOnDate", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = fromDate;
                SqlDataReader reader = cmd.ExecuteReader();
                List<HeatmiserTempReading> listOfReadings = null;
                if (reader.HasRows) {
                    listOfReadings = new List<HeatmiserTempReading>();
                    while (reader.Read()) {
                        HeatmiserTempReading reading = new HeatmiserTempReading();
                        reading.Date = reader.GetDateTime(0);
                        reading.CurrentAirTemp = reader.GetByte(1);
                        reading.IsHeating = reader.GetBoolean(2);
                        listOfReadings.Add(reading);
                    }
                }
                connection.Close();
                return listOfReadings;
            }
            catch (Exception ex) {
                LoggingHelper.WriteExceptionLogEntry(ex.Source, ex);
                return null;
            }
        }

        public void ResetReceiverIfPerceivedHanging() {
            HeatmiserState currentState = CurrentState;

            byte currentTemp = (byte)(currentState.CurrentAirTemp / 10);
            byte setRoomTemp = currentState.IsOnHoliday ? currentState.FrostProtectTemperature : currentState.SetRoomTemp;

            if (Math.Abs(currentTemp - setRoomTemp) > 1) {
                DateTime pastReadingDate = DateTime.Now.AddMinutes(currentState.RateOfChange * -1);
                List<HeatmiserTempReading> list = GetListOfReadings(pastReadingDate);

                if (list != null) {
                    bool heatingThroughout = true;
                    bool idleThroughout = true;
                    foreach (HeatmiserTempReading reading in list) {
                        if (!reading.IsHeating) {
                            heatingThroughout = false;
                        }
                        else {
                            idleThroughout = false;
                        }
                    }

                    if (
                        (heatingThroughout &&
                            (currentState.CurrentAirTemp - list[0].CurrentAirTemp) < 7)
                         || 
                        (idleThroughout &&
                            (currentState.CurrentAirTemp - list[0].CurrentAirTemp) > 7)
                        ) {
                            ResetHangingReceiver();
                    }
                }
            }
        }


        private int pollInterval;
        public int PollInterval {
            get {
                return pollInterval;
            }
        }

        private HeatmiserState currentState = null;
        public HeatmiserState CurrentState {
            get {
                return currentState;
            }
        }

        public GenericDeviceState GetCurrentState() {
            return currentState;
        }

        private ushort pinCode;

        public event DeviceStateChangedDelegate StateChanged;

        public HeatmiserState RequestData() {
            HeatmiserState state = SendMessageToThermostat(ReadDCBCommand());
            if (state != null) {
                this.currentState = state;
                if (StateChanged != null) {
                    StateChanged(this, new DeviceStateChangedEventArgs(this.currentState));
                }
            }
            return currentState;
        }

        public void SetTemperature(byte setTemperature) {
            this.CurrentState.SetRoomTemp = setTemperature;
            PostData(currentState);
            RequestData();
        }

        public void ResetHangingReceiver() {
            byte currentTemp = (byte)(this.currentState.CurrentAirTemp / 10);
            byte setRoomTemp = this.currentState.SetRoomTemp;

            if (currentTemp - setRoomTemp > 1 && !currentState.IsOnHoliday) {
                this.currentState.SetRoomTemp = (byte)(currentTemp + 1);
                PostData(currentState);
                Thread.Sleep(1000);
                RequestData();
                this.currentState.SetRoomTemp = setRoomTemp;
                PostData(currentState);
                RequestData();
            }

            if ((setRoomTemp - currentTemp > 1 && !currentState.IsOnHoliday) ||
                (currentState.FrostProtectTemperature - currentTemp > 1 && currentState.IsOnHoliday)) {
                this.currentState.SetRoomTemp = (byte)(currentTemp - 1);
                PostData(currentState);
                Thread.Sleep(1000);
                RequestData();
                this.currentState.SetRoomTemp = setRoomTemp;
                PostData(currentState);
                RequestData();
            }
        }

        public HeatmiserState PostData(HeatmiserState state) {
            HeatmiserState stat = null;
            if (state.GetChangedValues().Count > 0) {
                foreach (HeatmiserStateValueChange stateChange in state.GetChangedValues()) {
                    stat = SendMessageToThermostat(WriteDCBCommand(stateChange));
                }
            }
            return stat;
        }

        private void ProcessMessageFromThermostat(string receivedMessage) {
            HeatmiserState state = new HeatmiserState();
            byte[] dcbExtracted = new byte[292];
            Array.Copy(Encoding.ASCII.GetBytes(receivedMessage), 7, dcbExtracted, 0, 292);

            state.SetFromDCB(dcbExtracted);
            this.currentState = state;
        }

        public HeatmiserState SendMessageToThermostat(byte[] message) {
            byte[] recBuffer = SocketsHelper.SendMessageToSocketAndReadSynchroniously(this.localEndPoint, this.targetEndPoint,
                message, ProtocolType.Tcp, false);

            if (recBuffer != null) {
                HeatmiserState state = new HeatmiserState();
                byte[] dcbExtracted = new byte[292];
                Array.Copy(recBuffer, 7, dcbExtracted, 0, 292);

                state.SetFromDCB(dcbExtracted);

                return state;
            }
            return null;
        }

        private byte[] CRC16_4Bit(byte CRC16_High, byte CRC16_Low, byte nibble) {
            byte[] CRC16LookupHigh = new byte[]{
                0x00, 0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x70,
                0x81, 0x91, 0xA1, 0xB1, 0xC1, 0xD1, 0xE1, 0xF1
                };
            byte[] CRC16LookupLow = new byte[] {
                0x00, 0x21, 0x42, 0x63, 0x84, 0xA5, 0xC6, 0xE7,
                0x08, 0x29, 0x4A, 0x6B, 0x8C, 0xAD, 0xCE, 0xEF
                };

            byte t = (byte)(CRC16_High >> 4);
            t = (byte)(t ^ nibble);

            CRC16_High = (byte)((byte)(CRC16_High << 4) | (byte)(CRC16_Low >> 4));
            CRC16_Low = (byte)(CRC16_Low << 4);

            CRC16_High = (byte)(CRC16_High ^ CRC16LookupHigh[t]);
            CRC16_Low = (byte)(CRC16_Low ^ CRC16LookupLow[t]);

            return new byte[] { CRC16_High, CRC16_Low };
        }

        private ushort CRC(byte[] message) {
            byte CRC16_High = 0xff;
            byte CRC16_Low = 0xff;
            byte[] ret;

            foreach (byte chr in message) {
                byte var1 = (byte)(chr >> 4);
                byte var2 = (byte)(chr & 0x0f);

                ret = CRC16_4Bit(CRC16_High, CRC16_Low, var1);
                CRC16_High = ret[0];
                CRC16_Low = ret[1];
                ret = CRC16_4Bit(CRC16_High, CRC16_Low, var2);
                CRC16_High = ret[0];
                CRC16_Low = ret[1];
            }

            return BitConverter.ToUInt16(new byte[] { CRC16_Low, CRC16_High }, 0);
        }

        private byte[] w2b(ushort word) {
            byte[] bytes = BitConverter.GetBytes(word);
            if (!BitConverter.IsLittleEndian) {
                Array.Reverse(bytes);
            }
            return bytes;
        }

        private byte[] Command(byte op, byte[] data) {
            ushort len = (ushort)(7 + data.Length);

            byte[] cmdTemp = new byte[len - 2];
            byte[] cmd = new byte[len];

            cmdTemp[0] = cmd[0] = op;
            cmdTemp[1] = cmd[1] = w2b(len)[0];
            cmdTemp[2] = cmd[2] = w2b(len)[1];
            cmdTemp[3] = cmd[3] = w2b(pinCode)[0];
            cmdTemp[4] = cmd[4] = w2b(pinCode)[1];

            for (int i = 5; i < data.Length + 5; i++) {
                cmdTemp[i] = cmd[i] = data[i - 5];
            }

            byte[] crc = w2b(CRC(cmdTemp));

            cmd[len - 2] = crc[0];
            cmd[len - 1] = crc[1];

            return cmd;
        }

        private byte[] ReadDCBCommand() {
            byte[] message = new byte[4] { 0x00, 0x00, 0xff, 0xff };

            return Command(0x93, message);
        }

        private byte[] WriteDCBCommand(HeatmiserStateValueChange valueChange) {
            List<byte> toWrite = new List<byte>();
            toWrite.Add(1);
            toWrite.Add(this.w2b(valueChange.Address)[0]);
            toWrite.Add(this.w2b(valueChange.Address)[1]);
            toWrite.Add(valueChange.NumberOfBytes);

            foreach (byte b in valueChange.Contents) {
                toWrite.Add(b);
            }
            return Command(0xa3, toWrite.ToArray());
        }

        public void SetDateTime() {
            this.RequestData();
            this.CurrentState.SystemTime = DateTime.Now;
            this.PostData(CurrentState);
        }

        public void SetHoliday(PlannedHoliday holiday) {
            CancelAwayMode();
            this.RequestData();
            this.currentState.SetHoliday(holiday.ToDate);
            this.PostData(CurrentState);
            this.RequestData();
            StateChanged(this, new DeviceStateChangedEventArgs(this.currentState));
        }

        public void CancelHoliday() {
            this.RequestData();
            if (this.currentState.IsOnHoliday) {
                this.currentState.CancelHoliday();
                this.PostData(CurrentState);
                StateChanged(this, new DeviceStateChangedEventArgs(this.currentState));
            }
        }

        public void ToggleHotWater() {
            this.RequestData();
            if (this.CurrentState.IsHotWater) {
                this.currentState.SetHotWaterState(2);
            }
            else {
                this.currentState.SetHotWaterState(1);
            }
            this.PostData(CurrentState);
            StateChanged(this, new DeviceStateChangedEventArgs(this.currentState));
        }

        public void SwitchOnHotWater() {
            this.RequestData();
            this.currentState.SetHotWaterState(1);
            this.PostData(CurrentState);
            StateChanged(this, new DeviceStateChangedEventArgs(this.currentState));
        }

        public void SetAwayMode(int durationMins) {
            this.RequestData();
            if (!currentState.IsOnHoliday) {
                SwitchOffHotWater();
                HoldTemperature(CalculateAwayHoldDuration(durationMins), 17);
            }
        }

        private ushort CalculateAwayHoldDuration(int distanceMins) {
            DateTime targetDate = DateTime.Now.AddMinutes(distanceMins);
            int targetTemp = this.currentState.FindSetTemperatureByTimeAndDay(targetDate.DayOfWeek, targetDate.TimeOfDay);

            int tempDelta = targetTemp - (currentState.CurrentAirTemp / 10);

            if (tempDelta <= 0) {
                return (ushort)distanceMins;
            } else {
                return (ushort)(distanceMins - tempDelta * currentState.RateOfChange + 30);
            }
        }

        public void CancelAwayMode() {
            this.RequestData();
            if (!currentState.IsOnHoliday && currentState.TempHold > 0) {
                HoldTemperature(0, 0);
            }
        }

        public void HoldTemperature(ushort duration, byte temp) {
            this.RequestData();
            this.currentState.TempHold = (ushort)duration;
            if (temp != 0) {
                this.SetTemperature(temp);
            } else {
                PostData(this.currentState);
            }
            StateChanged(this, new DeviceStateChangedEventArgs(this.currentState));
        }

        public void SwitchOffHotWater() {
            this.RequestData();
            this.currentState.SetHotWaterState(2);
            this.PostData(CurrentState);
            StateChanged(this, new DeviceStateChangedEventArgs(this.currentState));
        }
    }
}
