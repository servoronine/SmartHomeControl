﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------



[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(Namespace="http://SmartHomeControl.Remotes.CommandServer", ConfigurationName="ICommandServer")]
public interface ICommandServer
{
    
    [System.ServiceModel.OperationContractAttribute(Action="http://SmartHomeControl.Remotes.CommandServer/ICommandServer/GetDeviceState", ReplyAction="http://SmartHomeControl.Remotes.CommandServer/ICommandServer/GetDeviceStateRespon" +
        "se")]
    string GetDeviceState(string deviceName);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://SmartHomeControl.Remotes.CommandServer/ICommandServer/GetDeviceState", ReplyAction="http://SmartHomeControl.Remotes.CommandServer/ICommandServer/GetDeviceStateRespon" +
        "se")]
    System.Threading.Tasks.Task<string> GetDeviceStateAsync(string deviceName);

    [System.ServiceModel.OperationContractAttribute(Action = "http://SmartHomeControl.Remotes.CommandServer/ICommandServer/GetDeviceStateVariable", ReplyAction = "http://SmartHomeControl.Remotes.CommandServer/ICommandServer/GetDeviceStateVariableRespon" +
        "se")]
    string GetDeviceStateVariable(string deviceName, string variableName);

    [System.ServiceModel.OperationContractAttribute(Action = "http://SmartHomeControl.Remotes.CommandServer/ICommandServer/GetDeviceStateVariable", ReplyAction = "http://SmartHomeControl.Remotes.CommandServer/ICommandServer/GetDeviceStateVariableRespon" +
        "se")]
    System.Threading.Tasks.Task<string> GetDeviceStateVariableAsync(string deviceName, string variableName);


    [System.ServiceModel.OperationContractAttribute(Action="http://SmartHomeControl.Remotes.CommandServer/ICommandServer/TriggerCommand", ReplyAction="http://SmartHomeControl.Remotes.CommandServer/ICommandServer/TriggerCommandRespon" +
        "se")]
    [System.ServiceModel.ServiceKnownTypeAttribute(typeof(object[]))]
    string TriggerCommand(string remoteName, string commandName, object[] parameters);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://SmartHomeControl.Remotes.CommandServer/ICommandServer/TriggerCommand", ReplyAction="http://SmartHomeControl.Remotes.CommandServer/ICommandServer/TriggerCommandRespon" +
        "se")]
    System.Threading.Tasks.Task<string> TriggerCommandAsync(string remoteName, string commandName, object[] parameters);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public interface ICommandServerChannel : ICommandServer, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public partial class CommandServerClient : System.ServiceModel.ClientBase<ICommandServer>, ICommandServer
{
    
    public CommandServerClient()
    {
    }
    
    public CommandServerClient(string endpointConfigurationName) : 
            base(endpointConfigurationName)
    {
    }
    
    public CommandServerClient(string endpointConfigurationName, string remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public CommandServerClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public CommandServerClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(binding, remoteAddress)
    {
    }
    
    public string GetDeviceState(string deviceName)
    {
        return base.Channel.GetDeviceState(deviceName);
    }
    
    public System.Threading.Tasks.Task<string> GetDeviceStateAsync(string deviceName)
    {
        return base.Channel.GetDeviceStateAsync(deviceName);
    }

    public string GetDeviceStateVariable(string deviceName, string variableName) {
        return base.Channel.GetDeviceStateVariable(deviceName, variableName);
    }

    public System.Threading.Tasks.Task<string> GetDeviceStateVariableAsync(string deviceName, string variableName) {
        return base.Channel.GetDeviceStateVariableAsync(deviceName, variableName);
    }

    public string TriggerCommand(string remoteName, string commandName, object[] parameters)
    {
        return base.Channel.TriggerCommand(remoteName, commandName, parameters);
    }
    
    public System.Threading.Tasks.Task<string> TriggerCommandAsync(string remoteName, string commandName, object[] parameters)
    {
        return base.Channel.TriggerCommandAsync(remoteName, commandName, parameters);
    }
}
