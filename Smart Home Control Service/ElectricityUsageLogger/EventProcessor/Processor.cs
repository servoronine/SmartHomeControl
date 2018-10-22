using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.Timers;
using SmartHomeControl.Remotes.Generic;
using SmartHomeControl.Devices.Generic.Gateways;
using SmartHomeControl.Devices.Generic.Events;
using SmartHomeControl.Devices.Generic.Devices;
using SmartHomeControl.Devices.HolidayManager;
using System.ServiceModel;
using System.Threading.Tasks;
using SmartHomeControl.Schedules;

namespace SmartHomeControl.EventProcessor
{
    public class Processor
    {
        public event MessagesOccuredWhenInvokingHandlerDelegate MessagesOccurred;
        public event FeedbackReceivedFromDeviceDelegate DeviceEventOccurred;
        public event DeviceStateChangedDelegate DeviceStateChanged;

        private GatewayList Gateways { get; set; }
        private RemoteList Remotes { get; set; }
        private ZoneList Zones { get; set; }
        private ScheduleList Schedules { get; set; }

        ServiceHost host; 

        private Timer queueCleanUpTimer = new Timer(5000);

        private List<ReceivedCommand> commandQueue = new List<ReceivedCommand>();

        private void CleanUpCommandQueue(object source, ElapsedEventArgs e) {
            lock (commandQueue) {
                for (int i = 0; i < commandQueue.Count; i++) {
                    ReceivedCommand recCommand = commandQueue[i];
                    if ((e.SignalTime - recCommand.TimeReceived).TotalSeconds >= DetermineReceivedCommandLifeTime(recCommand)) {
                        commandQueue.Remove(recCommand);
                        i++;
                    }
                }
            }
        }

        private int DetermineReceivedCommandLifeTime(ReceivedCommand rec) {
            if (rec.Sender is GenericDevice || rec.Sender is Schedule) {
                return 3;
            } else {
                return Remotes[rec.Sender].CommandLifeTime;
            }
        }

        public Processor(string settingsFile, MessagesOccuredWhenInvokingHandlerDelegate messagesHandler, 
            DeviceStateChangedDelegate stateChanged,
            FeedbackReceivedFromDeviceDelegate feedbackReceived) {
            this.MessagesOccurred += messagesHandler;
            this.DeviceStateChanged += stateChanged;
            this.DeviceEventOccurred += feedbackReceived;
            InitializeProcessor(settingsFile);
        }

        public Processor(string settingsFile) {
            InitializeProcessor(settingsFile);
        }

        private void ProcessChangedDeviceState(object sender, DeviceStateChangedEventArgs e) {
            if (DeviceStateChanged != null) {
                DeviceStateChanged(sender, e);
                Schedules.ProcessChangedDeviceState(sender, e);
            }
        }

        private void ProcessDeviceEvent(object sender, FeedbackReceivedFromDeviceEventArgs e) {
            lock (commandQueue) {
                commandQueue.Add(new ReceivedCommand(sender, e.Command, DateTime.Now, e.Parameters));
                if (Zones != null) {
                    InvokeTriggerIfFound();
                }
            }
            if (DeviceEventOccurred != null) {
                DeviceEventOccurred(sender, e);
            }
        }

        private void InitializeProcessor(string settingsFile)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(settingsFile);

            XmlNamespaceManager nsMgr = new XmlNamespaceManager(doc.NameTable);
            nsMgr.AddNamespace("def", "http://smarthomecontrol.voronin.co.uk");

            //Loading local settings first
            XmlNode settingsNode = doc.SelectSingleNode("/def:Definition/def:LocalSettings", nsMgr);
            LocalSettings.InitializeParameters(IPAddress.Parse(settingsNode.Attributes["address"].Value),
                settingsNode.Attributes["dbConnection"].Value, this,
                settingsNode.Attributes["macAddress"].Value,
                settingsNode.Attributes["homeCoordinates"].Value);

            //Now loading gateways
            XmlNode node = doc.SelectSingleNode("/def:Definition/def:Gateways", nsMgr);
            Gateways = new GatewayList(node);

            //Next we load remotes
            node = doc.SelectSingleNode("/def:Definition/def:Remotes", nsMgr);
            Remotes = new RemoteList(node);
            Remotes.ButtonPressed += ProcessDeviceEvent;

            //Loading list of schedules
            node = doc.SelectSingleNode("/def:Definition/def:Schedules", nsMgr);
            Schedules = new ScheduleList(node);
            Schedules.ScheduleFired += ProcessDeviceEvent;

            //Finally we load zones
            node = doc.SelectSingleNode("/def:Definition/def:Zones", nsMgr);
            Zones = new ZoneList(node, Gateways, ProcessChangedDeviceState, ProcessDeviceEvent);

            TriggerStartupCommands();

            queueCleanUpTimer.Elapsed += new ElapsedEventHandler(CleanUpCommandQueue);
            queueCleanUpTimer.Enabled = true;

            //Creating command server for external commands (Alexa, Mobile App, etc - received from IIS gateway)
            CommandServer.CommandServer comSrv = new CommandServer.CommandServer(GetCurrentDeviceState, SimulateSpecificCommand);
            host = new ServiceHost(comSrv);
            host.Open();
        }

        private void TriggerStartupCommands() {
            foreach (GenericRemote rem in Remotes) {
                foreach (GenericRemoteCommand com in rem.RemoteCommands) {
                    if (com.TriggerOnStartUp) {
                        Remotes.SimulateRemoteCommand(com);
                    }
                }
            }
        }

        public string SimulateSpecificCommand(string remote, string command, object[] parameters) {
            return Remotes.TriggerSpecificCommand(remote, command, parameters);
        }

        private void InvokeTriggerIfFound() {
            List<TriggerToBeInvoked> triggersToBeInvoked = new List<TriggerToBeInvoked>();
            List<GenericTrigger> listOfTriggers = Zones.GetListOfTriggers();
            foreach (GenericTrigger trigger in listOfTriggers) {
                try {
                    TriggerToBeInvoked triggerToInvoke = SearchForTriggerInCommandQueue(trigger);
                    if (triggerToInvoke != null) {
                        triggersToBeInvoked.Add(triggerToInvoke);
                    }
                }
                catch (Exception e) {
                    if (MessagesOccurred != null) {
                        MessagesOccurred(this, e.Message);
                    }
                }              
            }
            triggersToBeInvoked.Sort();
            InvokeTriggers(triggersToBeInvoked);
        }

        private void InvokeTriggers(List<TriggerToBeInvoked> triggersList) {
            Task.Factory.StartNew(() => {
                foreach (TriggerToBeInvoked trigger in triggersList) {
                    trigger.Trigger.InvokeAction(trigger.TriggerParameters);
                    System.Threading.Thread.Sleep(300);
                }
            });
        }

        private SenderTypeEnum DetermineSenderType(object sender) {
            if (sender is GenericRemote) {
                return SenderTypeEnum.Remote;
            }
            else if (sender is Schedule) {
                return SenderTypeEnum.Schedule;
            }
            else {
                return SenderTypeEnum.Device;
            }
        }

        private TriggerToBeInvoked SearchForTriggerInCommandQueue(GenericTrigger trigger) {
            TriggerToBeInvoked triggerToBeInvoked = null;
            for (int listIndex = 0; listIndex < commandQueue.Count - trigger.TriggerCommands.Count + 1; listIndex++) {
                int count = 0;
                while (count < trigger.TriggerCommands.Count && 
                    trigger.SenderType == DetermineSenderType(commandQueue[listIndex + count].Sender) &&
                    commandQueue[listIndex + count].Sender.ToString().Contains(trigger.Sender) &&
                    trigger.TriggerCommands[count].Equals(commandQueue[listIndex + count].Command)) {
                    count++;
                    if (count == trigger.TriggerCommands.Count) {
                        if (!commandQueue[listIndex].ProcessedTriggers.Contains(trigger)) {
                            List<object> paramList = new List<object>();
                            for (int i = listIndex; i < listIndex + trigger.TriggerCommands.Count; i++) {
                                if (commandQueue[i].Parameters != null) {
                                    paramList.AddRange(commandQueue[i].Parameters);
                                }
                            }

                            triggerToBeInvoked = new TriggerToBeInvoked();
                            triggerToBeInvoked.Trigger = trigger;
                            if (paramList.Count > 0) {
                                triggerToBeInvoked.TriggerParameters = paramList.ToArray();
                            }

                            MarkCommandsAsProcessed(trigger, listIndex, listIndex + trigger.TriggerCommands.Count - 1);
                        }
                    }
                }
            }
            return triggerToBeInvoked;
        }

        private void MarkCommandsAsProcessed(GenericTrigger processedTrigger, int startButton, int endButton) {
            for (int i = startButton; i <= endButton; i++) {
                this.commandQueue[i].ProcessedTriggers.Add(processedTrigger);
            }
        }

        public GenericDeviceState GetCurrentDeviceState(string deviceName) {
            return Zones.GetCurrentState(deviceName);
        }
    }
}
