using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMIP.UniversalScan.Data;

namespace IMIP.UniversalScan.Connector
{
    public interface IReleaseAdapter
    {
        bool Configure(BPProcess oBPProcess, ref byte[] env);
        bool ReleaseBatch(ref UniBatch oBatch);
    }
}
