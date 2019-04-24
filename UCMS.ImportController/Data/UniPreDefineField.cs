using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UCMS.ImportController.Data
{
    [XmlRoot("UniPreDefineField")]
    public class UniPreDefineField
    {
        public string Name;

        public string UniqueID;

        public List<string> PreDefineValues;
    }
}
