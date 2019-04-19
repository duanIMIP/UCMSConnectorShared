using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IMIP.UniversalScan.Def
{
     [Serializable]
    public class UniIntProperty : UniProperty
    {
        [XmlAttribute]
        public int Value { get; set; }

        public UniIntProperty() : base()
        {
        }
    }
}
