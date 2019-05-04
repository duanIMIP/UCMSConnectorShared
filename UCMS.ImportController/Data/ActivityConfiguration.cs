using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UCMS.ImportController.Data
{
    public class ActivityConfiguration
    {
        public string mVersion { get; set; }
        public List<IMIP.UniversalScan.Data.Branch> Branches { get; set; }
        [XmlElement("BatchNamingProfileSetting")]
        public IMIP.UniversalScan.Profile.BatchNamingProfile BatchNamingProfile { get; set; }
        [XmlElement("DocumentTypeProfileSetting")]
        public IMIP.UniversalScan.Profile.DocumentTypeProfile DocumentTypeProfile { get; set; }
        [XmlElement("SettingReference")]
        public String SettingReference { get; set; }
    }    
}
