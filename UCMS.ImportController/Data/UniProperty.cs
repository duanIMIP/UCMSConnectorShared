using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UCMS.ImportController.Data
{
    public class UniProperty
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute("Privilege")]
        public Accounts Privilege { get; set; }
    }
}
