using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UCMS.ImportController.Data
{
    [XmlRoot("BatchNamingSetting")]
    public class BatchNamingSetting
    {
        [XmlElement("DocumentTypeName")]
        public string DocumentTypeName { get; set; }
        [XmlElement("DateFormat")]
        public string DateFormat { get; set; }
        [XmlElement("TimeFormat")]
        public string TimeFormat { get; set; }
        [XmlElement("BatchNamingTemplate")]
        public BatchNamingTemplate BatchNamingTemplate { get; set; }
    }

    public class BatchNamingTemplate
    {
        [XmlElement("SourceField")]
        public SourceField[] Items { get; set; }
    }
}
