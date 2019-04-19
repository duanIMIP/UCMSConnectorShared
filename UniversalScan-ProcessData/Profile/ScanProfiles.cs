/****************************************************************************
'*   (c) Copyright IMIP Technology And Solution Consultancy JSC., 2010. All rights reserved.
'*   Unauthorized use, duplication or distribution is strictly prohibited.
'*****************************************************************************
'*
'*   File:       ScanProfiles.cs
'*
'*   Purpose:    scan profile setting
*********************************************************************************/
using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using IMIP.UniversalScan.Profile;
using IMIP.UniversalScan.Common;
using IMIP.UniversalScan.Connector;

namespace IMIP.UniversalScan.Profile
{
    public class ScanConstants
    {
        public const string m_cScanSourceSoftImport = "Import...";

        public const string Rotate = "Rotate";
        public const string RemoveIsolatedDots = "RemoveIsolatedDots";
        public const string AutoDeskew = "AutoDeskew";
        public const string AutoDespecle = "AutoDespecle";
        public const string RemoveBlackBorder = "RemoveBlackBorder";
        public const string Negative = "Negative";
        public const string ScanType = "ScanType";
        public const string Duplex = "Duplex";
        public const string ShowTwainUI = "ShowTwainUI";
        public const string AutoScan = "AutoScan";
        public const string ScanSourceName = "ScanSourceName";
        public const string Resolution = "Resolution";
        public const string ImageMode = "ImageMode";
        public const string PageSize = "PageSize";
        public const string PageWidth = "PageWidth";
        public const string PageHeight = "PageHeight";
        public const string Brightness = "Brightness";
        public const string AutoBrightness = "AutoBrightness";
        public const string Contrast = "Contrast";
        public const string Compression = "Compression";
        public const string ExtractBarcodes = "ExtractBarcodes";
        
    }
     [Serializable]
    [XmlRootAttribute(ElementName = "Profiles")]
    public class ScanProfiles
    {
        public Collection<ScanProfile> Profiles ;

        public ScanProfiles()
        {
            Profiles = new Collection<ScanProfile>();
        }

        public ScanProfile GetProfileByName(string sName)
        {
            foreach (ScanProfile oProfile in Profiles)
            {
                if (oProfile.ProfileName.Equals(sName, StringComparison.OrdinalIgnoreCase))
                    return oProfile;
            }

            return null;
        }

        public bool IsNameUnique(string sName)
        {
            foreach (ScanProfile oProfile in Profiles)
            {
                if (oProfile.ProfileName.Equals(sName, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }
    }

    [Serializable]
    public class ScanProfile
    {
        private string m_sProfileName;
        private DateTime m_CreationDate;
        private string m_sDescription;
        private string m_sCreator;
        private bool m_bEnabled;
        private Collection<ScanProfileProperty> m_arrProfileProperty;
        private Collection<string> m_arrScannerSources;

        [XmlIgnore]
        public static string DefaultScanProfile = "[Default]";

        [XmlIgnore]
        public static string AdminScanProfile = "Admin Scan Profile";

        [XmlElement]
        public Collection<ScanProfileProperty> ProfileProperties
        {
            get { return m_arrProfileProperty; }
            set { m_arrProfileProperty = value; }
        }

        [XmlElement]
        public Collection<string> ScannerSources
        {
            get { return m_arrScannerSources; }
            set { m_arrScannerSources = value; }
        }

        [XmlAttribute]
        public string ProfileName
        {
            get { return m_sProfileName; }
            set { m_sProfileName = value; }
        }

        [XmlAttribute]
        public DateTime CreationDate
        {
            get { return m_CreationDate; }
            set { m_CreationDate = value; }
        }

        [XmlAttribute]
        public string Description
        {
            get { return m_sDescription; }
            set { m_sDescription = value; }
        }

        [XmlAttribute]
        public string Creator
        {
            get { return m_sCreator; }
            set { m_sCreator = value; }
        }

        [XmlAttribute]
        public bool Enabled
        {
            get { return m_bEnabled; }
            set { m_bEnabled = value; }
        }

        public ScanProfile()
        {
            m_sProfileName = "";
            m_sCreator = "";
            m_CreationDate = DateTime.Now;
            m_sDescription = "";
            m_bEnabled = false;
            m_arrProfileProperty = new Collection<ScanProfileProperty>();
            m_arrScannerSources = new Collection<string>();
        }

        public void UpdateProfileProperty(ScanProfileProperty oProperty)
        {
            bool bUpdated = false;
            for (int nIndex = 0; nIndex < m_arrProfileProperty.Count; nIndex++)
            {
                if (m_arrProfileProperty[nIndex].Name.Equals(oProperty.Name, StringComparison.OrdinalIgnoreCase))
                {
                    //Update property
                    m_arrProfileProperty[nIndex] = oProperty;
                    bUpdated = true;
                }
            }

            //Else insert new propertiy
            if (!bUpdated)
            {
                m_arrProfileProperty.Add(oProperty);
            }
            // NOTE: To ensure scan profile contains all necessary settings, we need to use another profile to verify
            ScanProfile profile = ScanProfile.CreateDefaultScanProfile();
            
            foreach (ScanProfileProperty defaultProperty in profile.ProfileProperties)
            {
                var isExisted = false;
                foreach (ScanProfileProperty property in m_arrProfileProperty)
                {
                    if (property.Name.Equals(defaultProperty.Name))
                    {
                        isExisted = true;
                        break;
                    }
                }

                if (!isExisted)
                {
                    m_arrProfileProperty.Add(defaultProperty);
                }
            }
        }

        public ScanProfileProperty GetProfilePropertyByName(string sName)
        {
            foreach (ScanProfileProperty oProperty in m_arrProfileProperty)
            {
                if (oProperty.Name.Equals(sName, StringComparison.OrdinalIgnoreCase))
                    return oProperty;
            }
            return null;
        }

        public ScanSetting GetScanSetting()
        {
            if (m_arrProfileProperty.Count > 0)
            {
                try
                {
                    ScanSetting oScanSetting = new ScanSetting();
                    oScanSetting.ImageMode = (ScanCommon.EImageMode)GetProfilePropertyByName(ScanConstants.ImageMode).Value;
                    oScanSetting.Resolution = int.Parse(GetProfilePropertyByName(ScanConstants.Resolution).Value.ToString());
                    oScanSetting.PageSize = (ScanCommon.EPaperSize)GetProfilePropertyByName(ScanConstants.PageSize).Value;
                    oScanSetting.ScanType = (ScanCommon.EScanningType)GetProfilePropertyByName(ScanConstants.ScanType).Value;
                    oScanSetting.Contrast = (int)GetProfilePropertyByName(ScanConstants.Contrast).Value;
                    oScanSetting.Brightness = (int)GetProfilePropertyByName(ScanConstants.Brightness).Value;
                    oScanSetting.AutoBrightness = (bool)GetProfilePropertyByName(ScanConstants.AutoBrightness).Value;
                    oScanSetting.Duplex = (bool)GetProfilePropertyByName(ScanConstants.Duplex).Value;
                    oScanSetting.AutoDeskew = (bool)GetProfilePropertyByName(ScanConstants.AutoDeskew).Value;
                    oScanSetting.AutoDespecle = (bool)GetProfilePropertyByName(ScanConstants.AutoDespecle).Value;
                    oScanSetting.RemoveBlackBorder = (bool)GetProfilePropertyByName(ScanConstants.RemoveBlackBorder).Value;
                    oScanSetting.RemoveIsolatedDots = (bool)GetProfilePropertyByName(ScanConstants.RemoveIsolatedDots).Value;
                    oScanSetting.ShowTWAINUI = (bool)GetProfilePropertyByName(ScanConstants.ShowTwainUI).Value;
                    oScanSetting.Compression = GetProfilePropertyByName(ScanConstants.Compression) != null? (bool)GetProfilePropertyByName(ScanConstants.Compression).Value:false;
                    oScanSetting.ExtractBarcodes = GetProfilePropertyByName(ScanConstants.ExtractBarcodes) != null ? (bool)GetProfilePropertyByName(ScanConstants.ExtractBarcodes).Value:false;
                    //Rotate
                    //ScanProfileProperty oProperty = GetProfilePropertyByName(ScanConstants.Rotate);
                    //if (oProperty != null)
                    //    oScanSetting.Rotate = (ScanCommon.ERotate)GetProfilePropertyByName(ScanConstants.Rotate).Value;

                    //paper width
                    ScanProfileProperty pagewidth = GetProfilePropertyByName(ScanConstants.PageWidth);
                    if (pagewidth != null)
                        oScanSetting.PageWidth = (double)pagewidth.Value;

                    //paper height
                    ScanProfileProperty pageheight = GetProfilePropertyByName(ScanConstants.PageHeight);
                    if (pageheight != null)
                        oScanSetting.PageHeight = (double)pageheight.Value;

                    return oScanSetting;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public static ScanProfile CreateDefaultScanProfile()
        {
            ScanProfile oDefault = new ScanProfile();
            oDefault.ProfileName = DefaultScanProfile;

            ScanProfileProperty oPP;

            //Color mode
            oPP = new ScanProfileProperty();
            oPP.Name = ScanConstants.ImageMode;
            oPP.Value = ScanCommon.EImageMode.BlackAndWhite;
            oPP.Shared = false;
            oDefault.ProfileProperties.Add(oPP);

            //Resolution
            oPP = new ScanProfileProperty();
            oPP.Name = ScanConstants.Resolution;
            oPP.Value = (int)200;
            oPP.Shared = false;
            oDefault.ProfileProperties.Add(oPP);

            //Paper size
            oPP = new ScanProfileProperty();
            oPP.Name = ScanConstants.PageSize;
            oPP.Value = ScanCommon.EPaperSize.A4;
            oPP.Shared = false;
            oDefault.ProfileProperties.Add(oPP);

            //Scan Feeding Type
            oPP = new ScanProfileProperty();
            oPP.Name = ScanConstants.ScanType;
            oPP.Value = ScanCommon.EScanningType.ADF;
            oPP.Shared = false;
            oDefault.ProfileProperties.Add(oPP);

            //Contrast
            oPP = new ScanProfileProperty();
            oPP.Name = ScanConstants.Contrast;
            oPP.Value = (int)0;
            oPP.Shared = false;
            oDefault.ProfileProperties.Add(oPP);

            //Brightness
            oPP = new ScanProfileProperty();
            oPP.Name = ScanConstants.Brightness;
            oPP.Value = (int)0;
            oPP.Shared = false;
            oDefault.ProfileProperties.Add(oPP);

            //AutoBrightness
            oPP = new ScanProfileProperty();
            oPP.Name = ScanConstants.AutoBrightness;
            oPP.Value = true;
            oPP.Shared = false;
            oDefault.ProfileProperties.Add(oPP);

            //Duplex
            oPP = new ScanProfileProperty();
            oPP.Name = ScanConstants.Duplex;
            oPP.Value = false;
            oPP.Shared = false;
            oDefault.ProfileProperties.Add(oPP);

            //AutoDeskew
            oPP = new ScanProfileProperty();
            oPP.Name = ScanConstants.AutoDeskew;
            oPP.Value = false;
            oPP.Shared = false;
            oDefault.ProfileProperties.Add(oPP);

            //AutoDespecle
            oPP = new ScanProfileProperty();
            oPP.Name = ScanConstants.AutoDespecle;
            oPP.Value = false;
            oPP.Shared = false;
            oDefault.ProfileProperties.Add(oPP);

            //RemoveBlackBorder
            oPP = new ScanProfileProperty();
            oPP.Name = ScanConstants.RemoveBlackBorder;
            oPP.Value = false;
            oPP.Shared = false;
            oDefault.ProfileProperties.Add(oPP);

            //RemoveIsolatedDots
            oPP = new ScanProfileProperty();
            oPP.Name = ScanConstants.RemoveIsolatedDots;
            oPP.Value = false;
            oPP.Shared = false;
            oDefault.ProfileProperties.Add(oPP);

            //Show Progress UI
            oPP = new ScanProfileProperty();
            oPP.Name = ScanConstants.ShowTwainUI;
            oPP.Value = false;
            oPP.Shared = false;
            oDefault.ProfileProperties.Add(oPP);

            //Compression
            oPP = new ScanProfileProperty();
            oPP.Name = ScanConstants.Compression;
            oPP.Value = false;
            oPP.Shared = false;
            oDefault.ProfileProperties.Add(oPP);

            //ExtractBarcodes
            oPP = new ScanProfileProperty();
            oPP.Name = ScanConstants.ExtractBarcodes;
            oPP.Value = false;
            oPP.Shared = false;
            oDefault.ProfileProperties.Add(oPP);

            //Rotation
            oPP = new ScanProfileProperty();
            oPP.Name = ScanConstants.Rotate;
            oPP.Value = ScanCommon.ERotate.Rotate0;
            oPP.Shared = false;
            oDefault.ProfileProperties.Add(oPP);
           
            return oDefault;
        }

    }
}
