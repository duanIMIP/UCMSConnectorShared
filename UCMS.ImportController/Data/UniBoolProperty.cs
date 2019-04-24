using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UCMS.ImportController.Data
{
    public class UniBoolProperty
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute("Privilege")]
        public Accounts Privilege { get; set; }

        public bool Value { get; set; }

    }
}
