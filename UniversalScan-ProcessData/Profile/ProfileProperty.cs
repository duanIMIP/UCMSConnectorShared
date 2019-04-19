using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IMIP.UniversalScan.Profile
{
    [Serializable]
    public class ScanProfileProperty
    {
        private string m_sName;
        private object m_oValue;
        private bool m_bShared;

        public ScanProfileProperty()
        {
            m_sName = "";
            Value = null;
            Shared = false;
        }

        [XmlAttribute]
        public string Name
        {
            get { return m_sName; }
            set { m_sName = value; }
        }

        [XmlElement]
        public object Value
        {
            get { return m_oValue; }
            set { m_oValue = value; }
        }

        [XmlAttribute]
        public bool Shared
        {
            get { return m_bShared; }
            set { m_bShared = value; }
        }
    }
}
