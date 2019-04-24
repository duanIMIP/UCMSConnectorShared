using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UCMS.ImportController.Data
{
    [XmlRoot("FieldDefs")]
    public class FieldDefs
    {
        [XmlAttribute("UniFieldDefinition")]
        public UniFieldDefinition[] Items { get; set; }
    }
}
