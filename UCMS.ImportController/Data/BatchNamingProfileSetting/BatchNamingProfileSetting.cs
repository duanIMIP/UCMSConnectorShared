using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UCMS.ImportController.Data
{
    [XmlRoot("BatchNamingProfileSetting")]
    public class BatchNamingProfileSetting
    {
        [XmlElement("Enabled")]
        public bool Enabled { get; set; }
        [XmlElement("BatchNamingSettings")]
        public BatchNamingSettings BatchNamingSettings { get; set; }
        [XmlElement("ReplaceIllegalChar")]
        public string ReplaceIllegalChar { get; set; }
    }

    public class BatchNamingSettings {
        [XmlElement("BatchNamingSetting")]
        public BatchNamingSetting[] Items { get; set; }
    }
}
