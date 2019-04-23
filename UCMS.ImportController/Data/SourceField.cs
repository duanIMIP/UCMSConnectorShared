using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UCMS.ImportController.Data
{
    [XmlRoot("SourceField")]
    public class SourceField
    {
        [XmlElement("Type")]
        public Common.SourceFieldType Type { get; set; }
        [XmlElement("StaticName")]
        public string StaticName { get; set; }
        [XmlElement("DisplayName")]
        public string DisplayName { get; set; }
    }
}
