using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCMS.ImportController.Data
{
    public class DataValue: IDisposable
    {
        bool disposed = false;
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

        ~DataValue()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //this.Dispose();
                }
                disposed = true;
            }
        }

    }
}
