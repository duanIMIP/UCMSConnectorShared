using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IMIP.UniversalScan.Def
{
    

    [Serializable]
    public class UniColumn
    {
        [XmlAttribute]
        public string ColumnName;

        [XmlAttribute]
        public UniFieldDataType ColumnType;

        public UniColumn()
        { }

        public UniColumn(string name, UniFieldDataType type)
        {
            ColumnName = name;
            ColumnType = type;
        }
    }
}
