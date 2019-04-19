using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IMIP.UniversalScan.Connector
{
    

    public class ConnectorMenuInfo
    {
        public string MenuName;
        public string MethodName;
        public bool LoadBatch;

        public ConnectorMenuInfo()
        {
            LoadBatch = false;
        }
    }
    [Serializable]
    public class ConnectorInfo
    {
        public string Name { get; set; }
        public string ID { get; set; }
        [XmlIgnore]
        public string Repository { get; set; }
        public string Assembly { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        [XmlIgnore]
        public List<ConnectorMenuInfo> ConnectorMenus;

        public ConnectorInfo()
        {
            Name = string.Empty;
            ID = string.Empty;
            Repository = string.Empty;
            Assembly = string.Empty;
            Description = string.Empty;
            Version = string.Empty;
            ConnectorMenus = new List<ConnectorMenuInfo>();
        }
    }
}
