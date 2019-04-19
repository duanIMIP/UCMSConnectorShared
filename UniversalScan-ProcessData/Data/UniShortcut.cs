using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IMIP.UniversalScan.Data
{
    public class UniShortcut
    {
        [XmlAttribute]
        public string id { get; set; }

        [XmlAttribute]
        public string version { get; set; }

        [XmlElement]
        public string Client { get; set; }

        [XmlElement]
        public string Process { get; set; }

        [XmlElement]
        public string ProcessStep { get; set; }

        [XmlElement]
        public string FormType { get; set; }

        public UniShortcut() {
            id = "";
            version = "";
            Client = "";
            Process = "";
            ProcessStep = "";
            FormType = "";
        }
    }

    public class Shortcuts
    {
        [XmlElement]
        public List<UniShortcut> UniShortcut { get; set; }

        public Shortcuts()
        {
            UniShortcut = new List<UniShortcut>();
        }
    }
}
