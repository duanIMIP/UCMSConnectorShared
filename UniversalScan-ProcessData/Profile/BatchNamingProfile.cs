using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using IMIP.SharedComponent.CustomMapping;

namespace IMIP.UniversalScan.Profile
{
    [Serializable]
    public class BatchNamingProfile
    {
        public bool Enabled { get; set; }

        public Collection<BatchNamingSetting> BatchNamingSettings { get; set; }

        public string ReplaceIllegalChar { get; set; }

        public BatchNamingProfile()
        {
            this.Enabled = false;
            this.ReplaceIllegalChar = "";
            this.BatchNamingSettings = new Collection<BatchNamingSetting>();
        }

        public BatchNamingProfile Clone()
        {
            BatchNamingProfile oBatchNamingProfile = new BatchNamingProfile();
            oBatchNamingProfile.Enabled = this.Enabled;
            oBatchNamingProfile.ReplaceIllegalChar = this.ReplaceIllegalChar;
            foreach (BatchNamingSetting oBatchNamingSetting in this.BatchNamingSettings)
            {
                BatchNamingSetting newBatchNamingSetting = new BatchNamingSetting();
                newBatchNamingSetting.DocumentTypeName = oBatchNamingSetting.DocumentTypeName;
                newBatchNamingSetting.DateFormat = oBatchNamingSetting.DateFormat;
                newBatchNamingSetting.TimeFormat = oBatchNamingSetting.TimeFormat;
                foreach (SourceField oExportValue in oBatchNamingSetting.BatchNamingTemplate)
                {
                    newBatchNamingSetting.BatchNamingTemplate.Add(oExportValue);
                }
                oBatchNamingProfile.BatchNamingSettings.Add(oBatchNamingSetting);
            }

            return oBatchNamingProfile;
        }

        public BatchNamingSetting GetBatchNamingSetting(string sDocumentTypeName)
        {
            foreach (BatchNamingSetting oBatchNamingSetting in this.BatchNamingSettings)
            {
                if (sDocumentTypeName.Equals(oBatchNamingSetting.DocumentTypeName, StringComparison.OrdinalIgnoreCase))
                    return oBatchNamingSetting;
            }

            return null;
        }
    }

    [Serializable]
    public class BatchNamingSetting
    { 
        public string DocumentTypeName { get; set; }

        public string DateFormat { get; set; }

        public string TimeFormat { get; set; }

        public Collection<SourceField> BatchNamingTemplate { get; set; }

        public BatchNamingSetting()
        {
            this.DateFormat = "ddMMyyyy";
            this.TimeFormat = "HHmmss";
            this.BatchNamingTemplate = new Collection<SourceField>();
        }
    }
}
