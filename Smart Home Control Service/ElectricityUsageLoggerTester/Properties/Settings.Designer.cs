﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmartHomeControlServerUI.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=192.168.0.58;Initial Catalog=EnergyLogging;Integrated Security=True")]
        public string EnergyLoggingConnectionString {
            get {
                return ((string)(this["EnergyLoggingConnectionString"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("192.168.0.61")]
        public string WifiLinkAddress {
            get {
                return ((string)(this["WifiLinkAddress"]));
            }
            set {
                this["WifiLinkAddress"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("9760")]
        public int WifiLinkPort {
            get {
                return ((int)(this["WifiLinkPort"]));
            }
            set {
                this["WifiLinkPort"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("192.168.0.54")]
        public string LocalIPAddress {
            get {
                return ((string)(this["LocalIPAddress"]));
            }
            set {
                this["LocalIPAddress"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("192.168.0.60")]
        public string HeatmiserAddress {
            get {
                return ((string)(this["HeatmiserAddress"]));
            }
            set {
                this["HeatmiserAddress"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("8068")]
        public int HeatmiserPort {
            get {
                return ((int)(this["HeatmiserPort"]));
            }
            set {
                this["HeatmiserPort"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://datapoint.metoffice.gov.uk/public/data/")]
        public string WebWeatherAddress {
            get {
                return ((string)(this["WebWeatherAddress"]));
            }
            set {
                this["WebWeatherAddress"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("val/wxobs/all/xml/3740?res=hourly&key=b08fb588-2bf3-4e27-9f81-3356e3215962")]
        public string WebWeatherParams {
            get {
                return ((string)(this["WebWeatherParams"]));
            }
            set {
                this["WebWeatherParams"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int WifiLinkPollInterval {
            get {
                return ((int)(this["WifiLinkPollInterval"]));
            }
            set {
                this["WifiLinkPollInterval"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("900")]
        public int HeatmiserPollInterval {
            get {
                return ((int)(this["HeatmiserPollInterval"]));
            }
            set {
                this["HeatmiserPollInterval"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3600")]
        public int WebWeatherPollInterval {
            get {
                return ((int)(this["WebWeatherPollInterval"]));
            }
            set {
                this["WebWeatherPollInterval"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5325")]
        public ushort HeatmiserPin {
            get {
                return ((ushort)(this["HeatmiserPin"]));
            }
            set {
                this["HeatmiserPin"] = value;
            }
        }
    }
}