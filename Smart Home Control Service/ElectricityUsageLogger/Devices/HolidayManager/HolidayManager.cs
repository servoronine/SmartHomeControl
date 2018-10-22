using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data.SqlClient;
using System.Data;
using System.Timers;
using SmartHomeControl.Devices.Generic.Devices;
using SmartHomeControl.Devices.Generic.Gateways;
using SmartHomeControl.EventProcessor;
using SmartHomeControl.Devices.Generic.Events;

namespace SmartHomeControl.Devices.HolidayManager
{
    public class HolidayManager : GenericDevice, IStatefulDevice
    {
        public HolidayManager(XmlNode settings, GenericZone parentZone, GenericDeviceGateway gateway)
            : base(settings, parentZone, gateway) {
                Timer timer = new Timer();
                timer.Interval = 600000;
                timer.Elapsed += timer_Elapsed;
                timer.Enabled = true;
        }

        private HolidayManagerState currentState = new HolidayManagerState();

        public event DeviceStateChangedDelegate StateChanged;

        public GenericDeviceState GetCurrentState() {
            return currentState;
        }

        public void ForceStateRefresh() {
            timer_Elapsed(this, null);
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e) {
            RefreshPlannedHolidays();
            ValidateIfNextHoliday();
        }

        public void RefreshPlannedHolidays()
        {
            List<PlannedHoliday> plannedHolidays = currentState.PlannedHolidays;

            lock (plannedHolidays) {
                SqlConnection connection = new SqlConnection(LocalSettings.SqlConnectionString);
                connection.Open();

                SqlCommand command = new SqlCommand("EXEC GetPlannedHolidays", connection);
                SqlDataReader reader = command.ExecuteReader();

                plannedHolidays.Clear();
                while (reader.Read()) {
                    plannedHolidays.Add(
                        new PlannedHoliday(
                            reader.GetInt32(0),
                            reader.GetDateTime(1),
                            reader.GetDateTime(2),
                            reader.GetBoolean(3)
                            )
                            );
                }
            }

            TriggerStateUpdatedEvent();
        }

        public void AddHoliday(DateTime fromDate, DateTime toDate) {
            PlannedHoliday holiday = new PlannedHoliday();
            holiday.FromDate = fromDate;
            holiday.ToDate = toDate;

            SqlConnection connection = new SqlConnection(LocalSettings.SqlConnectionString);

            connection.Open();
            UpdateHoliday(holiday, connection);
            connection.Close();

            RefreshPlannedHolidays();
        }

        public void UpdateHolidays(List<PlannedHoliday> plannedHols)
        {
            SqlConnection connection = new SqlConnection(LocalSettings.SqlConnectionString);
            connection.Open();

            List<PlannedHoliday> plannedHolidays = currentState.PlannedHolidays;

            lock (plannedHolidays) {

                foreach (PlannedHoliday holiday in plannedHols) {
                    UpdateHoliday(holiday, connection);
                }

                bool found;

                foreach (PlannedHoliday existingHol in plannedHolidays) {
                    found = false;
                    foreach (PlannedHoliday holiday in plannedHols) {
                        if (holiday.HolidayID == existingHol.HolidayID) {
                            found = true;
                            break;
                        }
                    }
                    if (!found) {
                        DeleteHoliday(existingHol, connection);
                    }
                }

            }
            connection.Close();

            RefreshPlannedHolidays();
        }

        private void UpdateHoliday(PlannedHoliday holiday, SqlConnection connection)
        {
            SqlCommand cmd = new SqlCommand("UpdateHoliday", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@HolidayID", SqlDbType.Int).Value = holiday.HolidayID;
            cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = holiday.FromDate;
            cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = holiday.ToDate;
            cmd.Parameters.Add("@IsActioned", SqlDbType.Bit).Value = holiday.IsActioned;
            cmd.ExecuteNonQuery();
        }

        private void DeleteHoliday(PlannedHoliday holiday, SqlConnection connection)
        {
            SqlCommand cmd = new SqlCommand("DeleteHoliday", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@HolidayID", SqlDbType.Int).Value = holiday.HolidayID;
            cmd.ExecuteNonQuery();
        }

        public void DeleteNextPlannedHoliday() {
            SqlConnection con = new SqlConnection(LocalSettings.SqlConnectionString);
            con.Open();
            DeleteHoliday(currentState.NextPlannedHoliday, con);
            con.Close();
            RefreshPlannedHolidays();
        }

        public void DeletePlannedHolidayStartingOn(DateTime date) {
            PlannedHoliday plh = currentState.GetHolidayStartingOn(date);
            if (plh != null) {
                SqlConnection con = new SqlConnection(LocalSettings.SqlConnectionString);
                con.Open();
                DeleteHoliday(plh, con);
                con.Close();
                RefreshPlannedHolidays();
            }
        }

        public void SetHolidayAsActioned(PlannedHoliday holiday) {
            SqlConnection connection = new SqlConnection(LocalSettings.SqlConnectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand("SetHolidayAsActioned", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@HolidayID", SqlDbType.Int).Value = holiday.HolidayID;
            cmd.ExecuteNonQuery();

            connection.Close();

            RefreshPlannedHolidays();
        }

        public void TriggerStateUpdatedEvent() {
            StateChanged(this, new DeviceStateChangedEventArgs(this.currentState));
        }


        private void ValidateIfNextHoliday() {
            lock (currentState.PlannedHolidays) {
                foreach (PlannedHoliday plannedHoliday in currentState.PlannedHolidays) {
                    if (plannedHoliday.FromDate < DateTime.Now && plannedHoliday.ToDate > DateTime.Now) {
                        if (currentState.CurrentHolidayState != CurrentHolidayStateEnum.OnHoliday ||
                            !plannedHoliday.IsActioned) {
                            currentState.CurrentHolidayState = CurrentHolidayStateEnum.OnHoliday;
                            SetHolidayAsActioned(plannedHoliday);
                            RaiseDeviceEvent(this, new FeedbackReceivedFromDeviceEventArgs("SetHouseInHolidayMode",
                                new object[] { plannedHoliday }));
                        }
                        break;
                    }
                    else if (
                      (plannedHoliday.FromDate > DateTime.Now && plannedHoliday.ToDate > DateTime.Now)
                      ||
                      ((currentState.PlannedHolidays.Count - 1) == currentState.PlannedHolidays.IndexOf(plannedHoliday) &&
                      plannedHoliday.ToDate < DateTime.Now)
                      ) {
                        if (currentState.CurrentHolidayState != CurrentHolidayStateEnum.AtHome) {
                            currentState.CurrentHolidayState = CurrentHolidayStateEnum.AtHome;
                            RaiseDeviceEvent(this, new FeedbackReceivedFromDeviceEventArgs("SetHouseInAtHomeMode", null));
                        }
                        break;
                    }
                }
            }
        }

        public void CancelCurrentHoliday() {
            if (currentState.CurrentHolidayState == CurrentHolidayStateEnum.AtHome) return;

            foreach (PlannedHoliday hol in currentState.PlannedHolidays) {
                if (hol.FromDate < DateTime.Now && hol.ToDate > DateTime.Now) {
                    hol.ToDate = DateTime.Now;
                }
            }
            UpdateHolidays(currentState.PlannedHolidays);
        }
    }
}
