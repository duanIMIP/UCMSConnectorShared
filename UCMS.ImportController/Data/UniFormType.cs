using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UCMS.ImportController.Data
{
    [XmlRoot("UniFormType")]
    public class UniFormType
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public bool Visible { get; set; }

        [XmlAttribute]
        public string ExternalID { get; set; }

        [XmlAttribute]
        public bool Root { get; set; }

        [XmlAttribute("FieldDefs")]
        public List<UniFieldDefinition> UniFieldDefinitions { get; set; }

        [XmlAttribute]
        public BatchNamingSetting DocumentNamingSetting { get; set; }
    }
}
