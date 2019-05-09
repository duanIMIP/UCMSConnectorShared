using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UCMS.ImportController.Data
{
    public class MultipleProfileThread
    {
        public Thread MyThread { get; set; }
        public int StopThread { get; set; }

        public MultipleProfileThread()
        {
            StopThread = 0;
        }
    }
}
