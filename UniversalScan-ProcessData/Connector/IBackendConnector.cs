using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMIP.UniversalScan.Data;
using IMIP.UniversalScan.Common;

namespace IMIP.UniversalScan.Connector
{

    public interface IBackendConnector
    {

        UniBatch GetBatchContentByInternalId(string batchID, string storagePath);
        string[] GetContentAttachmentByInternalId(string batchID, string storagePath);

        void ProcessBatch(UniBatch oBatch, string sOperation); //ignore, delete, reject, approve    
        void ImportBatch(UniBatch oBatch, bool bNew);
        int GetRescanBatchCount(string strBranchID);
        List<UniBatch> GetBatchesToRescan(string strBranchID, string strProcessStepName, string strProcessName, string strClientName, string strContentName, string sScanUser, int nPageIndex);
        void RunCustomTasks(string sOnEvent, string ApplicationPath, string BatchPath, UniBatch oBatch, object oCustomProps);

        void Initialize(string sApplicationPath);
        AuthResult Login(string sName, string sPassword, string sDomain);
        AuthResult Login(System.Security.Principal.WindowsIdentity identity);

        void Relogin();

        List<Branch> GetBranchs();
        List<BPClient> GetClients(bool ScanMode);
        List<Report> GetReports();
        string RunReport(string reportName, Dictionary<string, string> Params);
    }
    
}
