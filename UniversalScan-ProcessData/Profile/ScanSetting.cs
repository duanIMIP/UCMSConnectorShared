/****************************************************************************
'*   (c) Copyright IMIP Technology And Solution Consultancy JSC., 2010. All rights reserved.
'*   Unauthorized use, duplication or distribution is strictly prohibited.
'*****************************************************************************
'*
'*   File:       ScanSetting.cs
'*
'*   Purpose:    Define scan settings
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using IMIP.UniversalScan.Data;
using IMIP.UniversalScan.Common;
using IMIP.UniversalScan.Connector;

namespace IMIP.UniversalScan.Profile
{
    [Serializable]
    public class ScanSetting
    {
        private string m_strScanSourceName = string.Empty;
        private int m_nResolution;
        private ScanCommon.EImageMode m_EImageMode = ScanCommon.EImageMode.BlackAndWhite;
        private ScanCommon.EPaperSize m_EPaperSize = ScanCommon.EPaperSize.A4;

        private int m_nContrast = 0;
        private int m_nBrightness = 0;
        private bool m_bAutoBrightnes = true;

        private ScanCommon.EScanningType m_EScanType = ScanCommon.EScanningType.ADF;
        private bool m_bDuplex = false;
        private bool m_bAutoScan = true;
        private bool m_bShowTWAINUI = false;
        
        private bool m_bNegative = false;
        private bool m_bRemoveBlackBorder = false;
        private bool m_bAutoDeskew = false;
        private bool m_bAutoDespecle = false;
        private bool m_bRemoveIsolatedDots = false;
        private ScanCommon.ERotate m_eRotate = ScanCommon.ERotate.Rotate0;
        private double m_PageWidth;
        private double m_PageHeight;

        private bool m_Compression;

        private bool m_ExtractBarcode;

        public ScanSetting()
        {
            m_strScanSourceName = string.Empty;
            m_nResolution = 200;
            m_EImageMode = ScanCommon.EImageMode.BlackAndWhite;
            m_EPaperSize = ScanCommon.EPaperSize.A4;

            m_nContrast = 50;
            m_nBrightness = 50;
            m_bAutoBrightnes = false;

            m_EScanType = ScanCommon.EScanningType.ADF;
            m_bDuplex = false;
            
            m_bAutoScan = true;
            m_bShowTWAINUI = false;
            
            //image processing
            m_bNegative = false;
        
            m_bRemoveBlackBorder = false;
            m_bAutoDeskew = false;
            m_bAutoDespecle = false;
            m_bRemoveIsolatedDots = false;
            m_eRotate = ScanCommon.ERotate.Rotate0;

            m_PageWidth = 210;
            m_PageHeight = 297;

            m_Compression = false;

            m_ExtractBarcode = false;
        }

        [XmlAttribute]
        public ScanCommon.ERotate Rotate
        {
            get { return m_eRotate; }
            set { m_eRotate = value; }
        }

        [XmlAttribute]
        public bool RemoveIsolatedDots
        {
            get { return m_bRemoveIsolatedDots; }
            set { m_bRemoveIsolatedDots = value; }
        }

        [XmlAttribute]
        public bool AutoDeskew
        {
            get { return m_bAutoDeskew; }
            set { m_bAutoDeskew = value; }
        }

        [XmlAttribute]
        public bool AutoDespecle
        {
            get { return m_bAutoDespecle; }
            set { m_bAutoDespecle = value; }
        }


        [XmlAttribute]
        public bool RemoveBlackBorder
        {
            get { return m_bRemoveBlackBorder; }
            set { m_bRemoveBlackBorder = value; }
        }

        [XmlAttribute]
        public bool Negative
        {
            get { return m_bNegative; }
            set { m_bNegative = value; }
        }


        [XmlElementAttribute()]
        public ScanCommon.EScanningType ScanType
        {
            set { m_EScanType = value; }
            get { return m_EScanType; }
        }
        [XmlElementAttribute()]
        public bool Duplex
        {
            set { m_bDuplex = value; }
            get { return m_bDuplex; }
        }
        [XmlElementAttribute()]
        public bool AutoScan
        {
            set { m_bAutoScan = value; }
            get { return m_bAutoScan; }
        }

        [XmlElementAttribute()]
        public bool ShowTWAINUI
        {
            set { m_bShowTWAINUI = value; }
            get { return m_bShowTWAINUI; }
        }

        
        [XmlElementAttribute()]
        public string ScanSourceName
        {
            get
            {
                return m_strScanSourceName;
            }
            set
            {
                m_strScanSourceName = value;
            }
        }

        [XmlElementAttribute()]
        public int Resolution
        {
            get
            {
                return m_nResolution;
            }
            set
            {
                m_nResolution = value;
            }
        }
        [XmlElementAttribute()]
        public ScanCommon.EImageMode ImageMode
        {
            get
            {
                return m_EImageMode;
            }
            set
            {
                m_EImageMode = value;
            }
        }
        [XmlElementAttribute()]
        public ScanCommon.EPaperSize PageSize
        {
            get
            {
                return m_EPaperSize;
            }
            set
            {
                m_EPaperSize = value;
            }
        }
        [XmlElementAttribute()]
        public int Brightness
        {
            get
            {
                return m_nBrightness;
            }
            set
            {
                m_nBrightness = value;
            }
        }

        [XmlAttribute]
        public bool AutoBrightness
        {
            get { return m_bAutoBrightnes; }
            set { m_bAutoBrightnes = value; }
        }

        [XmlElementAttribute()]
        public int Contrast
        {
            get
            {
                return m_nContrast;
            }
            set
            {
                m_nContrast = value;
            }
        }

        [XmlElementAttribute()]
        public double PageWidth
        {
            get { return m_PageWidth; }
            set { m_PageWidth = value; }
        }

        [XmlElementAttribute()]
        public double PageHeight
        {
            get { return m_PageHeight; }
            set { m_PageHeight = value; }
        }


        [XmlElementAttribute()]
        public bool Compression
        {
            get { return m_Compression; }
            set { m_Compression = value; }
        }

        [XmlElementAttribute()]
        public bool ExtractBarcodes
        {
            get { return m_ExtractBarcode; }
            set { m_ExtractBarcode = value; }
        }
    }
}
