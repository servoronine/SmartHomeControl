using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace SmartHomeWebControl.Models.CommandServer
{
    public class CommandServerProxy
    {
        ChannelFactory<ICommandServer> channelFactory;
        public CommandServerProxy(string endPointUrl) {
            string endpointUrl = endPointUrl;
            WSHttpBinding binding = new WSHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(endpointUrl);
            channelFactory = new ChannelFactory<ICommandServer>(binding, endpoint);
        }


        public string RequestDeviceState(string deviceName) {
            ICommandServer clientProxy = channelFactory.CreateChannel();
            string state = clientProxy.GetDeviceState(deviceName);
            channelFactory.Close();
            return state;
        }

        public string RequestDeviceStateVariable(string deviceName, string variableName) {
            ICommandServer clientProxy = channelFactory.CreateChannel();
            string variableValue = clientProxy.GetDeviceStateVariable(deviceName, variableName);
            channelFactory.Close();
            return variableValue;
        }

        public string SendCommand(string remoteName, string commandName, object[] parameters) {
            ICommandServer clientProxy = channelFactory.CreateChannel();
            string str = clientProxy.TriggerCommand(remoteName, commandName, parameters);
            channelFactory.Close();
            return str;
        }
    }
}
