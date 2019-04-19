using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace IMIP.UniversalScan.Connector
{
    public class BackendConnectorConfig
    {
        static IBackendConnector m_sConnector = null;
        static string m_sConnectorLib;
        public static IBackendConnector GetBackendConnector(string sConnectorLib, bool bSingleton = true)
        {
            if (bSingleton)
            {
                if (m_sConnector == null || m_sConnectorLib != sConnectorLib)
                {
                    m_sConnector = Initialize(sConnectorLib);
                    m_sConnectorLib = sConnectorLib;
                }
            }
            else
                m_sConnector = Initialize(sConnectorLib);

            return m_sConnector;
        }

        private static IBackendConnector Initialize(string sConnectorLib)
        {
            Assembly assembly = Assembly.LoadFrom(sConnectorLib);

            foreach (Type oObject in assembly.GetTypes())
            {
                Type oType = oObject.GetInterface("IBackendConnector");

                if (oType != null && oType.IsInterface)
                {
                    return (IBackendConnector)Activator.CreateInstance(oObject);
                }
            }

            return null;
        }
    }
}