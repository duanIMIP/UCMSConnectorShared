using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IMIP.UniversalScan.Def
{
     [Serializable]
    public class UniDateTimeProperty : UniProperty
    {
        [XmlAttribute]
        public DateTime Value { get; set; }

        public UniDateTimeProperty() : base()
        {
            Value = DateTime.Now;
        }
    }
}
