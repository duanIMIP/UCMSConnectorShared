namespace IMIP.UniversalScan.Connector.UCMSConnectorShared
{
    public interface ICustomTasks
    {
        void OnEvent(string sAction, string path, object obj);
        bool RequirejsSerializer();
    }
}
