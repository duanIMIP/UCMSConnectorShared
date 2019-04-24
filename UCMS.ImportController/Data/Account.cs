using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UCMS.ImportController.Data
{
    public class Account
    {
        public const string Everyone = "Everyone";
        public string Name { get; set; }
        public string UPN { get; set; }
        public string Class { get; set; }
    }

    public class Accounts
    {
        [XmlAttribute("Account")]
        public Account[] Items { get; set; }
    }
}
