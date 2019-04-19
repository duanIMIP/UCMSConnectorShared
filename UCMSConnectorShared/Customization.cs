using System;
using System.Reflection;

namespace IMIP.UniversalScan.Connector.UCMSConnectorShared
{
    public class Customization
    {
        static ICustomTasks oCustomTasks = null;
        public static ICustomTasks Initialize(string sCustomizationLib)
        {
            if (oCustomTasks == null)
            {
                Assembly assembly = Assembly.LoadFrom(sCustomizationLib);

                foreach (Type oObject in assembly.GetTypes())
                {
                    Type oType = oObject.GetInterface("ICustomTasks");

                    if (oType != null && oType.IsInterface)
                    {
                        oCustomTasks = (ICustomTasks)Activator.CreateInstance(oObject);
                        return oCustomTasks;
                    }
                }

                return null;
            }
            else return oCustomTasks;
        }
    }
}