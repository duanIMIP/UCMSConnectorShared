using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UCMS.ImportController.Data
{
    public class UniColumn
    {
        [XmlAttribute]
        public string ColumnName;

        [XmlAttribute]
        public Common.UniFieldDataType ColumnType;
    }

    public class UniColumns
    {
        [XmlAttribute("UniColumn")]
        public UniColumn[] Items { get; set; }
    }
}
