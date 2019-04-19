/****************************************************************************
'*   (c) Copyright IMIP Technology And Solution Consultancy JSC., 2010. All rights reserved.
'*   Unauthorized use, duplication or distribution is strictly prohibited.
'*****************************************************************************
'*
'*   File:       IConnector.cs
'*
'*   Purpose:    Define interface of connector plugin
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IMIP.UniversalScan.Data;
using IMIP.UniversalScan.Def;
using IMIP.UniversalScan.Profile;

namespace IMIP.UniversalScan.Connector
{
    public interface IParentApp : System.Windows.Forms.IWin32Window
    {
        void ShowStatusForm();
        void CloseStatusForm();
        void SetMainStatusMessage(string sMessage);
        void SetSubStatusMessage(string sMessage);
        void SetException(Exception oException);
        bool IsCancelled();
       
    }

    public interface IIndexPanel : System.Windows.Forms.IContainerControl
    {
        event EventHandler FieldSelected;
        event EventHandler FieldValueChanged;
        event EventHandler CustomAction;
        bool BuildControl(UniDocument oDoc, List<UniPreDefineField> colPreDefList);
        List<UniField> GetFieldValues();
        void SetFieldValue(UniField oUniField);
    }

    public interface IConnector
    {
        System.Windows.Forms.DialogResult ShowSettingDialog();
        int GetRescanBatchCount();
        UniBatch GetBatchToRescan();
        void SetParentApp(IParentApp oParent);
        IIndexPanel GetIndexPanel();
        Collection<UniFieldDefinition> GetFieldDefinitions(string sFormType, UniBatch oBatch);
        void RefreshMetadata();
        void ClearCachedMetadata();
        bool DoesBatchHasType();
        void PublishBatchToRepository(UniBatch oBatch);
        UniBatch CreateNewBatch(bool bShowFormWhenCreateNew);
        void SerializeToFile(string fileName, UniBatch oBatch);
        UniBatch DeserializeFromFile(string fileName);
        void Initialize();
        void SetWorkingFolder(string sSettingFolder, string sWorkingFolder);
        ConnectorInfo GetConnectorInfo();
        string BuildBatchName(UniDocument oBatch);
        bool Validated(UniBatch oBatch);
        void Dispose();
    }

    
}
