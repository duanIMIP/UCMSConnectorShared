using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IMIP.UniversalScan.Data
{
    [Serializable]
    public class UniCell
    {
        [XmlAttribute]
        public string Value;

        [XmlAttribute]
        public string ColumnName;

        [XmlAttribute]
        public bool ValidationStatus;

        public CustomFieldData CustomData { get; set; }

        public UniCell()
        {
            CustomData = new CustomFieldData();
        }
    }

    [Serializable]
    public class UniRow
    {
        public List<UniCell> Cells { get; set; }

        public UniRow()
        {
            Cells = new List<UniCell>();
        }
    }
}
