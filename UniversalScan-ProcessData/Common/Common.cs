/****************************************************************************
'*   (c) Copyright IMIP Technology And Solution Consultancy JSC., 2010. All rights reserved.
'*   Unauthorized use, duplication or distribution is strictly prohibited.
'*****************************************************************************
'*
'*   File:       Common.cs
'*
'*   Purpose:    Define common setting for TWAIN scanning
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMIP.UniversalScan.Common
{
    public class ScanCommon
    {
        public const int m_cnBringhtnessMax = 100;
        public const int m_cnBrightnessMin = -100;
        public const int m_cnBrightnessDefaultValue = 0;

        public const int m_cnContrastMax = 100;
        public const int m_cnContrastMin = -100;
        public const int m_cnContrastDefaultValue = 0;

        public const int m_cnDefaultImageQuality = 75;

        public class ConstantString
        {
            public static string RootDoc = "Root Document";
            public static string DocName = "Name";
            public static string DocType = "Type";
            public static string DocSequence = "Sequence";

            public static string SystemDate = "Date";
            public static string SystemTime = "Time";
            public static string SystemMachineName = "Machine Name";
            public static string SystemUserName = "User Name";
            public static string SystemBranchId = "BranchID";
        }
        
        /// <summary>
        /// 
        /// </summary>
        public enum EScanningType
        {
            ADF, // Auto Document Feeder
            FLATBED // FlatBed
        }

        public enum EScannerState
        {
            SCANNER_OPEN_DATASOURCE_ERR,
            SCANNER_OPEN_DATASOURCE_SUCCESS,
            SCANNER_INIT_SETTING_ERR
        }

        public enum EImageMode
        {
            BlackAndWhite,
            Gray,
            Color
        }

        public enum EPaperSize
        {
            A0,
            A1,
            A2,
            A3,
            A4,
            A5,
            B3,
            B4,
            B5,
            USLETTER,
            USLEGAL,
            USLEDGER,
            CustomSize
        }

        public enum ERotate
        {
            Rotate0 = 0,
            Rotate90,
            Rotate180,
            Rotate270
        }

        public enum EImageFileType
        {
            SinglePageTiff,
            SinglePagePdf,
            SinglePageJpeg,
            MultiPageTiff,
            MultiPagePdf
        }

        public enum EValidationLevel
        {
            Batch,
            Document,
            Page
        }

        public enum EValidationResult
        {
            OK,
            Error,
            Warning
        }
    }

    public class ImageViewerCommon
    {
        public enum MouseMode
        {
            Default = 0,
            AreaSelection = 1,
            Pan = 2,
            AreaZooming = 3,
            Magnifier = 4
        }

        public enum MouseWheelMode
        {
            Zoom = 0,
            VerticalScroll = 1,
            PageChange = 2
        }

        public enum ZoomMode
        {
            ActualSize = 1,
            FitToViewer = 2,
            WidthViewer = 3,
            Custom = 4,
            HeightViewer = 5,
            ToViewer = 6
        }
    }

}
