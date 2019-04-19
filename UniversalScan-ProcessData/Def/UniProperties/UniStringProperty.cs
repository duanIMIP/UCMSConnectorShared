using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IMIP.UniversalScan.Def
{
     [Serializable]
    public class UniStringProperty : UniProperty
    {
        [XmlAttribute]
        public string Value { get; set; }

        public UniStringProperty() : base()
        {
            Value = string.Empty;
        }
    }
}
