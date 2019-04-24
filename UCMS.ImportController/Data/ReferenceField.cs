using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UCMS.ImportController.Data
{
    public class ReferenceField
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string ID { get; set; }

        [XmlAttribute]
        public string FieldValue { get; set; }

        //Use for eDocmanPlus - SharePoint connector, some field have reference to other item collection.
        [XmlElement(IsNullable = true)]
        public object RefObjectData { get; set; }
    }

    public class ReferenceFields
    {
        [XmlAttribute("ReferenceField")]
        public ReferenceField[] Items { get; set; }
    }
}
