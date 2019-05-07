using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCMS.ImportController.Data
{
    public class DataValue
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public DataValue()
        {
            this.Key = "";
            this.Value = "";
        }

        public DataValue(String Key, String Value)
        {
            this.Key = Key;
            this.Value = Value;
        }
    }
}
