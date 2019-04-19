using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMIP.UniversalScan.Connector.UCMSConnectorShared
{
    public class UniversalScanVolumeLicense
    {
        long m_lAvailableVolumeLicenses = 0;
        string LicenseName;
        bool m_bCountLicense = true;

        internal UniversalScanVolumeLicense(string licenseName, bool bCounting)
        {
            LicenseName = licenseName;
            m_bCountLicense = bCounting;

        }

        public void CheckLicense(int nVolumneToConsume)
        {
            if (!m_bCountLicense)
                return;
            m_lAvailableVolumeLicenses = GetAvailableVolumeLicenses(LicenseName);

            if (m_lAvailableVolumeLicenses < nVolumneToConsume)
                throw new ApplicationException(string.Format(USMSConnectorShared.Properties.Resources.Msg_InsufficientLicense, LicenseName));
        }

        private long GetAvailableVolumeLicenses(string licenseName)
        {
            return 10000;
            //throw new NotImplementedException();
        }
    }
}
