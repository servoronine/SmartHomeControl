using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SmartHomeControl.Remotes.Generic
{
    public class GenericRemote {
        public int CommandLifeTime { get; set; }

        private string _remoteName;
        public string remoteName
        {
            get
            {
                return _remoteName;
            }
        }


        public GenericRemote(XmlNode settings) {
            _remoteName = settings.Attributes["name"].Value;
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(settings.OwnerDocument.NameTable);
            nsMgr.AddNamespace("def", "http://smarthomecontrol.voronin.co.uk");
            CommandLifeTime = int.Parse(settings.Attributes["commandLifeTime"].InnerText);

            foreach (XmlNode node in settings.SelectSingleNode("def:Commands", nsMgr).ChildNodes)
            {
                GenericRemoteCommand com = new GenericRemoteCommand(node, this);
                RemoteCommands.Add(com);
            }
        }



        public RemoteCommandList RemoteCommands = new RemoteCommandList();
        private GenericRemoteCommand this[string commandName] {
            get {
                foreach (GenericRemoteCommand com in RemoteCommands)
                {
                    if (com.commandName == commandName)
                    {
                        return com;
                    }
                }
                return null;
            }
        }

        public override string ToString() {
            return remoteName;
        }
    }
}
