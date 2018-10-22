using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Data.SqlClient;
using SmartHomeControl.EventProcessor;
using SmartHomeControl.Devices.Generic.Devices;
using SmartHomeControl.Devices.Onkyo;
using SmartHomeControl.Devices.Heatmiser;
using SmartHomeControl.Devices.PC;
using SmartHomeControl.Devices.HolidayManager;
using SmartHomeControl.Devices.Generic.Events;

namespace SmartHomeControl.ServerUI {

    public partial class MainForm : Form {

        private Processor processor;
        private ButtonPressSimulator simulator;
        private List<string> radioStations = new List<string>();

        public MainForm() {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e) {
            simulator.RegisterWithLightwaveRF();
        }


        private void button6_Click(object sender, EventArgs e) {
            simulator.DiningAreaLightOn();
        }

        private void button8_Click(object sender, EventArgs e) {
            simulator.SittingAreaLightOn();
        }

        private void button10_Click(object sender, EventArgs e) {
            simulator.CornerLightOn();
        }

        private void diningBar_MouseCaptureChanged(object sender, EventArgs e) {
            simulator.DiningAreaLightDim(diningBar.Value);
        }

        private void trackBar2_MouseCaptureChanged(object sender, EventArgs e) {
            simulator.SittingAreaLightDim(sittingBar.Value);
        }

        private void cornerBar_MouseCaptureChanged(object sender, EventArgs e) {
            simulator.CornerLightDim(cornerBar.Value);
        }

        private void button7_Click(object sender, EventArgs e) {
            simulator.DiningAreaLightOff();
        }

        private void button9_Click(object sender, EventArgs e) {
            simulator.SittingAreaLightOff();
        }

        private void button11_Click(object sender, EventArgs e) {
            simulator.CornerLightOff();
        }

        private void button12_Click(object sender, EventArgs e) {
            simulator.HDDSocketOn();
        }

        private void button13_Click(object sender, EventArgs e) {
            simulator.HDDSocketOff();
        }

        private void button14_Click(object sender, EventArgs e) {
            simulator.SonosSocketOn();
        }

        private void button15_Click(object sender, EventArgs e) {
            simulator.SonosSocketOff();
        }

        private void button16_Click(object sender, EventArgs e) {
            simulator.KitchenSonosSocketOn();
        }

        private void button17_Click(object sender, EventArgs e) {
            simulator.KitchenSonosSocketOff();
        }

        private void button18_Click(object sender, EventArgs e) {
            simulator.KitchenCabinetLightsOn();
        }

        private void button19_Click(object sender, EventArgs e) {
            simulator.KitchenCabinetLightsOff();
        }

        private void Form1_Load(object sender, EventArgs e) {
            processor = new Processor("settings.xml", processor_messagesOccured, processor_DeviceStateChanged, processor_irReceivedUpdate);
            simulator = new ButtonPressSimulator(processor);
        }

        private void processor_DeviceStateChanged(object sender, DeviceStateChangedEventArgs e) {
            Form1_stationListUpdated(e.DeviceState);
        }

        void processor_messagesOccured(object sender, string message) {
            MessageBox.Show(message);
        }

        private void processor_irReceivedUpdate(object sender, FeedbackReceivedFromDeviceEventArgs e) {
            string paramString = "";
            if (e.Parameters != null) {
                foreach (object obj in e.Parameters) {
                    paramString = paramString + obj.ToString() + "; ";
                }
            }
            else {
                paramString = "N/A";
            }

            AddItemToReceivedBox("Remote: " + sender.ToString() + "; Command: " + e.Command +
                "; Parameters: " + paramString + "; Timestamp: " + DateTime.Now.ToString("dd/MM HH:mm"));
        }

        delegate void StateUpdated(GenericDeviceState state);
        void Form1_stationListUpdated(GenericDeviceState state) {
            if (this.stationsListBox.InvokeRequired) {
                StateUpdated d = new StateUpdated(Form1_stationListUpdated);
                this.Invoke(d, new object[] { state });
            }
            else {
                if (state is OnkyoState) {
                    OnkyoState st = (OnkyoState)state;
                    stationsListBox.Items.Clear();
                    stationsListBox.Items.AddRange(st.CurrentList);
                    seekLabel.Text = st.CurrentSeekPosition;
                    volumeLabel.Text = st.CurrentVolume.ToString();
                    powerLabel.Text = st.CurrentPowerState ? "On" : "Off";
                }
                if (state is HeatmiserState) {
                    HeatmiserState st = (HeatmiserState)state;
                    tempValueLabel.Text = st.CurrentAirTemp.ToString().Insert(2, ".") + "C";
                    setTempValueLabel.Text = st.SetRoomTemp.ToString() + "C";
                    hotWaterValueLabel.Text = st.IsHotWater ? "Yes" : "No";
                    heatingValueLabel.Text = st.IsHeating ? "Yes" : "No";
                    holidayValueLabel.Text = st.IsOnHoliday ? "Yes" : "No";
                    rateOfChangeValueLabel.Text = st.RateOfChange.ToString() + "Min";
                }
                if (state is PCState) {
                    if (((PCState)state).PCName == "StudyPC") {
                        studyPCOnOffLabel.Text = ((PCState)state).IsOn ? "On" : "Off";
                    } else if (((PCState)state).PCName == "LoungePC") {
                        mediaCenterOnOffLabel.Text = ((PCState)state).IsOn ? "On" : "Off";
                    }
                }
                if (state is HolidayManagerState) {
                    HolidayManagerState st = (HolidayManagerState)state;

                    //DialogResult dr = DialogResult.Yes;
                    //if (holidaysDataGrid.DataSource != null) {
                    //    dr = MessageBox.Show("Holiday list updated. Refresh the screen?", "Holidays Updated", MessageBoxButtons.YesNo);
                    //}

                    //if (dr == DialogResult.Yes) {
                        List<PlannedHoliday> copy = st.PlannedHolidays.ToList<PlannedHoliday>();
                        System.ComponentModel.BindingList<PlannedHoliday> bl = new System.ComponentModel.BindingList<PlannedHoliday>(copy);
                        bl.AllowNew = bl.AllowRemove = bl.AllowEdit = true;
                        holidaysDataGrid.DataSource = bl;
                    //}
                }
            }
        }

        delegate void AddItemToReceivedBoxCallback(string textToUpdate);
        private void AddItemToReceivedBox(string textToUpdate) {
            if (this.receivedBox.InvokeRequired) {
                AddItemToReceivedBoxCallback d = new AddItemToReceivedBoxCallback(AddItemToReceivedBox);
                this.Invoke(d, new object[] { textToUpdate });
            }
            else {
                this.receivedBox.Items.Add(textToUpdate);
                this.receivedBox.TopIndex = this.receivedBox.Items.Count - 1;
            }
        }

        private void button20_Click(object sender, EventArgs e) {
            simulator.SetHeatmiserDateTime();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e) {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void Form1_Resize(object sender, EventArgs e) {
            if (WindowState == FormWindowState.Minimized) {
                this.Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void button21_Click(object sender, EventArgs e) {
            simulator.ReceiverOn();
        }

        private void button22_Click(object sender, EventArgs e) {
            simulator.ReceiverOff();
        }

        private void button23_Click(object sender, EventArgs e) {
            simulator.ReceiverVolumeUp();
        }

        private void button24_Click(object sender, EventArgs e) {
            simulator.ReceiverVolumeDown();
        }

        private void button27_Click(object sender, EventArgs e) {
            simulator.MusicWithDimmedLights();
        }

        private void button30_Click(object sender, EventArgs e) {
            simulator.AllLightsOff();
        }

        private void button29_Click(object sender, EventArgs e) {
            simulator.AllLightsOn();
        }

        private void button25_Click(object sender, EventArgs e) {
            simulator.PlaySelectedStation(stationsListBox.SelectedIndex);
        }

        private void button26_Click(object sender, EventArgs e) {
            simulator.Stop();
        }

        private void button28_Click(object sender, EventArgs e) {
            simulator.MovieWithDimmedLights();
        }

        private void applyHolidaysButton_Click(object sender, EventArgs e) {
            System.ComponentModel.BindingList<PlannedHoliday> bl = (System.ComponentModel.BindingList<PlannedHoliday>)holidaysDataGrid.DataSource;
            simulator.UpdateHolidays(bl.ToList<PlannedHoliday>());
        }

        private void toggleHotWaterButton_Click(object sender, EventArgs e)
        {
            simulator.ToggleHotWater();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            simulator.Back();
        }

        private void tempDownButton_Click(object sender, EventArgs e) {
            simulator.DecreaseTemp();
        }

        private void tempUpButton_Click(object sender, EventArgs e) {
            simulator.IncreaseTemp();
        }

        private void pcOnButton_Click(object sender, EventArgs e) {
            simulator.SwitchStudyPCOn();
        }

        private void pcOffButton_Click(object sender, EventArgs e) {
            simulator.SwitchStudyPCOff();
        }

        private void button2_Click(object sender, EventArgs e) {
            simulator.DoneWatchingTV();
        }
    }
}
