using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UCMS.ImportController.Data
{
    public class UniFieldDefinition
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string DisplayName { get; set; }

        [XmlAttribute]
        public bool Mandatory { get; set; }

        [XmlAttribute]
        public bool ReadOnly { get; set; }

        [XmlAttribute]
        public int MaxLength { get; set; }

        [XmlAttribute]
        public string Format { get; set; }

        [XmlAttribute]
        public Common.UniFieldDataType DataType { get; set; }

        [XmlAttribute]
        public string DefaultValue { get; set; }

        [XmlAttribute]
        public string CustomProperty { get; set; }

        [XmlAttribute]
        public bool MultiLine { get; set; }

        [XmlAttribute]
        public bool MultiChoice { get; set; }

        [XmlAttribute]
        public bool EDocManField { get; set; }

        [XmlAttribute]
        public string PreDefineListID { get; set; }

        [XmlAttribute]
        public string ValidationFormat { get; set; }

        [XmlAttribute]
        public string ExternalID { get; set; }

        [XmlAttribute]
        public bool ValueFromBarcode { get; set; }

        [XmlAttribute("Columns")]
        public UniColumns Columns { get; set; }

        [XmlAttribute("ReferenceFields")]
        public ReferenceFields ReferenceFields { get; set; }

        public UniBoolProperty Visible { get; set; }

        public UniBoolProperty Editable { get; set; }

        public UniZone FieldZone { get; set; }

        [XmlAttribute]
        public int Index { get; set; }
    }
}
