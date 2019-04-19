using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System;
using IMIP.UniversalScan.Data;

namespace IMIP.UniversalScan.Def
{
    [Serializable]
    public abstract class UniProperty
    {
        [XmlAttribute]
        public string Name { get; set; }

        public Collection<Account> Privilege { get; set; }

        public UniProperty()
        {
            Name = string.Empty;
            Privilege = new Collection<Account>();
        }
    }
}
