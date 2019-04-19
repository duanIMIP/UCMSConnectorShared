/****************************************************************************
'*   (c) Copyright IMIP Technology And Solution Consultancy JSC., 2010. All rights reserved.
'*   Unauthorized use, duplication or distribution is strictly prohibited.
'*****************************************************************************
'*
'*   File:       ITwainScanning.cs
'*
'*   Purpose:    Define generic interface for scan
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMIP.UniversalScan.Profile;

namespace IMIP.UniversalScan.Twain
{
    public delegate void OnePageScannedEvent(string sFilePath);
    public delegate void FinishScanEvent();

    public interface ITwainScanning
    {
        event OnePageScannedEvent OnePageScanned;
        event FinishScanEvent FinishScan;

        void StartScan(IntPtr nHandle, string sScanSource, ScanSetting oSetting, string sTempFolder);
        void StopScan(IntPtr nHandle);
        string[] GetScanSources(IntPtr nHandle);
        void ScanOnePage(IntPtr nHandle, string sScanSource, ScanSetting oSetting, string sTempFolder);
        void Dispose();
    }
}
