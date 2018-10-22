using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmartHomeControl.Devices.Generic.Events;
using SmartHomeControl.Devices.Generic.Gateways;
using SmartHomeControl.Devices.HolidayManager;
using System.Threading;
using SmartHomeControl.Helpers;
using SmartHomeControl.EventProcessor;

namespace SmartHomeControl.Devices.Generic.Devices
{
    public class GenericDevice
    {
        public event FeedbackReceivedFromDeviceDelegate DeviceEventRaised;
        protected string _deviceName;
        private GenericZone parentZone;
        public GenericZone ParentZone {
            get {
                return parentZone;
            }
        }

        protected void RaiseDeviceEvent(object sender, FeedbackReceivedFromDeviceEventArgs e) {
            DeviceEventRaised(sender, e);
        }

        private List<GenericTrigger> triggers = new List<GenericTrigger>();
        public List<GenericTrigger> Triggers {
            get {
                return triggers;
            }
        }


        public GenericTrigger this[int triggerNumber] {
            get {
                return triggers[triggerNumber];
            }
        }


        public string deviceName
        {
            get
            {
                return _deviceName;
            }
        }

        protected int _deviceNumber;
        public int deviceNumber
        {
            get
            {
                return _deviceNumber;
            }
        }

        public GenericDevice(XmlNode settings, GenericZone parentZone, GenericDeviceGateway gateway)
        {
            this._deviceName = settings.Attributes["name"].InnerText;
            this._deviceNumber = int.Parse(settings.Attributes["number"].InnerText);
            this.parentZone = parentZone;

            foreach (XmlNode node in settings.ChildNodes) {
                triggers.Add(new GenericTrigger(node, this));
            }
        }

        public virtual void ToggleDeviceState(int state) {
            throw new Exception("This method must be implemented in child classes!");
        }

        public void TurnOnIfDark() {
            string[] coords = LocalSettings.HomeCoordinates.Split(';');
            SolarInfo si = SolarInfo.ForDate(double.Parse(coords[0]), double.Parse(coords[1]), DateTime.Now);

            if (si.Sunset <= DateTime.Now || si.Sunrise >= DateTime.Now) {
                ToggleDeviceState(1);
            }
        }

        public virtual void SetHolidayMode(PlannedHoliday plannedHoliday) {
            ToggleDeviceState(0);
        }

        public virtual void CancelHolidayMode() {
            ToggleDeviceState(1);
        }

        public void InvokeAction(GenericTriggerAction triggerAction, object[] parameters) {
            if (triggerAction.GetParametersForInvoke() != null && triggerAction.GetParametersForInvoke().Length > 0) {
                this.GetType().GetMethod(triggerAction.Action).Invoke(this, triggerAction.GetParametersForInvoke());
            }
            else {
                //First we try to call the method with parameters that are passed by the trigger
                //If that fails, we try to see if there is a method without parameters that matches the name
                System.Reflection.ParameterInfo[] parInfo = this.GetType().GetMethod(triggerAction.Action).GetParameters();
                try {
                    if (parInfo.Length > 0) {
                        this.GetType().GetMethod(triggerAction.Action).Invoke(this, parameters);
                    } else {
                        this.GetType().GetMethod(triggerAction.Action).Invoke(this, null);
                    }
                } catch (Exception ex) {
                    LoggingHelper.WriteExceptionLogEntry(ex.Message, ex);
                }
            }
        }

        public List<GenericTrigger> GetListOfTriggers() {
            List<GenericTrigger> triggerList = new List<GenericTrigger>();
            foreach (GenericTrigger trigger in Triggers) {
                triggerList.Add(trigger);
            }
            return triggerList;
        }

        public virtual void RegisterWithController() {
        }

        public void Pause(int delay) {
            Thread.Sleep(delay);
        }
    }
}
