using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UCMS.ImportController.Data
{
    [XmlRoot("DocumentTypeProfileSetting")]
    public class DocumentTypeProfileSetting
    {
        [XmlElement("PreDefineFieldList")]
        public PreDefineFieldList PreDefineFieldList { get; set; }
        [XmlElement("UniFormtypeList")]
        public UniFormtypeList UniFormtypeList { get; set; }
    }
}
