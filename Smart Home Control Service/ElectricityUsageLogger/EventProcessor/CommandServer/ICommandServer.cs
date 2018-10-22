using SmartHomeControl.Devices.Generic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeControl.EventProcessor.CommandServer {
    [ServiceContract(Namespace = "http://SmartHomeControl.Remotes.CommandServer")]
    interface ICommandServer {
        [OperationContract]
        string GetDeviceState(string deviceName);

        [OperationContract]
        string TriggerCommand(string remoteName, string commandName, object[] parameters);

        [OperationContract]
        string GetDeviceStateVariable(string deviceName, string variableName);
    }
}
