using SmartHomeControl.Devices.Generic.Devices;
using SmartHomeControl.Devices.HolidayManager;
using SmartHomeControl.EventProcessor;
using SmartHomeControl.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;

namespace SmartHomeControl.ServerUI {
    public class ButtonPressSimulator {
        private Processor processor;
        public ButtonPressSimulator(Processor processor) {
            this.processor = processor;
        }

        private void TriggerSpecificCommand(string remote, string command) {
            processor.SimulateSpecificCommand(remote, command, null);
        }

        private void TriggerSpecificCommand(string remote, string command, object[] parameters) {
            processor.SimulateSpecificCommand(remote, command, parameters);
        }

        #region Lounge
        public void SittingAreaLightOn() {
            TriggerSpecificCommand("LightwaveRFRemote", "SittingAreaLightOn");
        }

        public void SittingAreaLightOff() {
            TriggerSpecificCommand("LightwaveRFRemote", "SittingAreaLightOff");
        }

        public void DiningAreaLightOn() {
            TriggerSpecificCommand("LightwaveRFRemote", "DiningAreaLightOn");
        }

        public void DiningAreaLightOff() {
            TriggerSpecificCommand("LightwaveRFRemote", "DiningAreaLightOff");
        }

        public void CornerLightOn() {
            TriggerSpecificCommand("LightwaveRFRemote", "CornerLightOn");
        }

        public void CornerLightOff() {
            TriggerSpecificCommand("LightwaveRFRemote", "CornerLightOff");
        }

        public void InitiateEnergyConsumptionLogging() {
            TriggerSpecificCommand("TimerRemote", "LogEnergyConsumption");
        }

        public void SittingAreaLightDim(int dimLevel) {
            TriggerSpecificCommand("LightwaveRFRemote", "SittingAreaLightDim", new object[] { dimLevel });
        }

        public void CornerLightDim(int dimLevel) {
            TriggerSpecificCommand("LightwaveRFRemote", "CornerLightDim", new object[] { dimLevel });
        }

        public void DiningAreaLightDim(int dimLevel) {
            TriggerSpecificCommand("LightwaveRFRemote", "DiningAreaLightDim", new object[] { dimLevel });
        }

        public void HDDSocketOn() {
            TriggerSpecificCommand("LightwaveRFRemote", "HDDSocketOn");
        }

        public void HDDSocketOff() {
            TriggerSpecificCommand("LightwaveRFRemote", "HDDSocketOff");
        }

        public void SonosSocketOn() {
            TriggerSpecificCommand("LightwaveRFRemote", "SonosSocketOn");
        }

        public void SonosSocketOff() {
            TriggerSpecificCommand("LightwaveRFRemote", "SonosSocketOff");
        }

        public void ReceiverOn() {
            TriggerSpecificCommand("OnkyoRemote", "ReceiverOn");
        }

        public void ReceiverOff() {
            TriggerSpecificCommand("OnkyoRemote", "ReceiverOff");
        }

        public void ReceiverVolumeUp() {
            TriggerSpecificCommand("OnkyoRemote", "VolumeUp");
        }

        public void ReceiverVolumeDown() {
            TriggerSpecificCommand("OnkyoRemote", "VolumeDown");
        }

        public void PlaySelectedStation(int stationNumber) {
            TriggerSpecificCommand("OnkyoRemote", "PlaySelectedStation", new object[] { stationNumber });
        }

        public void Stop() {
            TriggerSpecificCommand("OnkyoRemote", "Stop");
        }

        public void Back() {
            TriggerSpecificCommand("OnkyoRemote", "Back");
        }

        public void MusicWithDimmedLights() {
            TriggerSpecificCommand("MoodsRemote", "MusicWithDimmedLights");
        }

        public void MovieWithDimmedLights() {
            TriggerSpecificCommand("MoodsRemote", "MovieWithDimmedLights");
        }

        public void AllLightsOff() {
            TriggerSpecificCommand("LightwaveRFRemote", "AllLoungeLightsOff");
        }

        public void AllLightsOn() {
            TriggerSpecificCommand("LightwaveRFRemote", "AllLoungeLightsOn");
        }

        public void DoneWatchingTV() {
            TriggerSpecificCommand("MoodsRemote", "TVOff");
        }

        #endregion

        #region Kitchen

        public void KitchenSonosSocketOn() {
            TriggerSpecificCommand("LightwaveRFRemote", "KitchenSonosSocketOn");
        }

        public void KitchenSonosSocketOff() {
            TriggerSpecificCommand("LightwaveRFRemote", "KitchenSonosSocketOff");
        }

        public void KitchenCabinetLightsOn() {
            TriggerSpecificCommand("LightwaveRFRemote", "KitchenCabinetLightsOn");
        }

        public void KitchenCabinetLightsOff() {
            TriggerSpecificCommand("LightwaveRFRemote", "KitchenCabinetLightsOff");
        }

        #endregion

        #region Hallway
        public void RegisterWithLightwaveRF() {
            TriggerSpecificCommand("LightwaveRFRemote", "RegisterWithLightwaveRF");
        }

        public void SetHeatmiserDateTime() {
            TriggerSpecificCommand("HeatmiserRemote", "SetDateTime");
        }

        public void ToggleHotWater()
        {
            TriggerSpecificCommand("HeatmiserRemote", "ToggleHotWater");
        }

        public void IncreaseTemp() {
            TriggerSpecificCommand("HeatmiserRemote", "IncreaseTemp");
        }

        public void DecreaseTemp() {
            TriggerSpecificCommand("HeatmiserRemote", "DecreaseTemp");
        }

        public void InitiateHeatmiserLogging() {
            TriggerSpecificCommand("TimerRemote", "LogHeatmiserState");
        }

        public void InitiateWebWeatherLogging() {
            TriggerSpecificCommand("TimerRemote", "LogWebWeather");
        }

        public void SetHouseInHolidayMode(PlannedHoliday plannedHoliday) {
            TriggerSpecificCommand("MoodsRemote", "SetHouseInHolidayMode", new object[] { plannedHoliday });
        }

        public void SetHouseInAtHomeMode()
        {
            TriggerSpecificCommand("MoodsRemote", "SetHouseInAtHomeMode");
        }

        public void UpdateHolidays(List<PlannedHoliday> plannedHolidays) {
            TriggerSpecificCommand("HolidayManagerRemote", "UpdateHolidays", new object[] { plannedHolidays });
        }

        public void AddHoliday(DateTime toDate) {
            TriggerSpecificCommand("HolidayManagerRemote", "AddHoliday", new object[] { toDate });
        }

        public void SetHolidayAsActioned(PlannedHoliday holiday) {
            TriggerSpecificCommand("HolidayManagerRemote", "SetHolidayAsActioned", new object[] { holiday });
        }
        #endregion

        #region Study
        public void SwitchStudyPCOn() {
            TriggerSpecificCommand("PCRemote", "TurnOnStudyPC");
        }

        public void SwitchStudyPCOff() {
            TriggerSpecificCommand("PCRemote", "TurnOffStudyPC");
        }
        #endregion
    }
}
