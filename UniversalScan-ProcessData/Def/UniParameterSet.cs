using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using IMIP.UniversalScan.Profile;
using IMIP.UniversalScan.Common;
using IMIP.UniversalScan.Connector;

namespace IMIP.UniversalScan.Def
{
    [Serializable]
    [XmlRootAttribute(ElementName = "ActivityConfiguration")]
    public class UniParameterSet
    {
        private string m_sSettingReference;

        #region Public properties
        [XmlElement]
        public ScanProfile ScanProfileSetting
        {
            get { return m_oScanProfile; }
            set { m_oScanProfile = value; }
        }

        public BatchNamingProfile BatchNamingProfileSetting
        {
            get { return m_oBatchNamingProfile; }
            set { m_oBatchNamingProfile = value; }
        }

        public SeparationProfile SeparationProfileSetting
        {
            get { return m_oSeparationProfile; }
            set { m_oSeparationProfile = value; }
        }

        public ValidationProfile ValidationProfileSetting
        {
            get { return m_oValidationProfile; }
            set { m_oValidationProfile = value; }
        }

        
        public DocumentTypeProfile DocumentTypeProfileSetting
        {
            get
            {
                if (m_oDocumentTypeProfile == null)
                {
                    m_oDocumentTypeProfile = new DocumentTypeProfile();
                }
                return m_oDocumentTypeProfile;
            }
            set { m_oDocumentTypeProfile = value; }
        }

        public string SettingReference
        {
            get
            {
                if (m_sSettingReference == null)
                {
                    m_sSettingReference = "";

                }

                return m_sSettingReference;
            }
            set
            {
                m_sSettingReference = value;
            }
        }
        

        public bool AllowCreateNewBatch { get; set; }

        public bool AllowEditBatch { get; set; }

        public bool NeedPrint { get; set; }

        public bool FilterRescanByUser { get; set; }

        #endregion

        #region Private fields
        private ScanProfile m_oScanProfile;
        private BatchNamingProfile m_oBatchNamingProfile;
        private SeparationProfile m_oSeparationProfile;
        private ValidationProfile m_oValidationProfile;
        private DocumentTypeProfile m_oDocumentTypeProfile;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public UniParameterSet()
        {
            m_oScanProfile = ScanProfile.CreateDefaultScanProfile();

            m_oBatchNamingProfile = new BatchNamingProfile();

            m_oSeparationProfile = new SeparationProfile();

            m_oValidationProfile = new ValidationProfile();

            m_oDocumentTypeProfile = new DocumentTypeProfile();

            SettingReference = "";

            AllowCreateNewBatch = true;

            AllowEditBatch = true;

            NeedPrint = false;

            FilterRescanByUser = false;
        }

        #endregion
    }
}
