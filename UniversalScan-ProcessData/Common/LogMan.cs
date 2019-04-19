using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMIP.UniversalScan.Common
{
    public class LogMan
    {
        //NLog.Logger m_oLogger;
        public LogMan()
        {
            //m_oLogger = NLog.LogManager.GetCurrentClassLogger();
        }
        public void Debug(string sMessage)
        {
            //m_oLogger.Debug(sMessage);
        }

        public void Debug(int iNumber)
        {
            Debug(iNumber.ToString());
        }

        public void Debug(Exception oExc)
        {
            Debug(oExc.ToString());
        }
    }
}
