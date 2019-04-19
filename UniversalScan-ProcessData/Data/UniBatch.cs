/****************************************************************************
'*   (c) Copyright IMIP Technology And Solution Consultancy JSC., 2010. All rights reserved.
'*   Unauthorized use, duplication or distribution is strictly prohibited.
'*****************************************************************************
'*
'*   File:       Batch.cs
'*
'*   Purpose:    Implement generic structure of batch
*********************************************************************************/
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;
using IMIP.UniversalScan.Def;
using IMIP.UniversalScan.Profile;

namespace IMIP.UniversalScan.Data
{
    [XmlRootAttribute(ElementName = "Batch")]
    public class UniBatch : UniDocument
    {
        [XmlAttribute]
        public string BranchID { get; set; }

        [XmlAttribute]
        public string SessionID { get; set; }

        [XmlAttribute]
        public string ExternalID { get; set; }

        [XmlAttribute]
        public string UserID { get; set; }

        [XmlAttribute]
        public string ClientName { get; set; }

        [XmlAttribute]
        public string ProcessName { get; set; }

        [XmlAttribute]
        public string ProcessStepName { get; set; }

        [XmlAttribute]
        public int Priority { get; set; }

        [XmlAttribute]
        public bool ImageCompression { get; set; }

        [XmlAttribute]
        public string ScanUser { get; set; }

        [XmlAttribute]
        public string ScanStation { get; set; }

        [XmlAttribute]
        public bool SupportDocumentStructure { get; set; }

        [XmlAttribute]
        public DateTime PublishDate { get; set; }

        [XmlAttribute]
        public string ErrorMessage { get; set; }

        [XmlAttribute]
        public bool IgnoreBatch { get; set; }

        //[XmlIgnore]
        //public object CustomProperties { get; set; }

        [XmlAttribute]
        public bool AllowDocumentNaming { get; set; }

        [XmlAttribute]
        public string CreationUser { get; set; }

        public UniParameterSet UniParameterSet { get; set; }

        public List<UniDocument> Docs { get; set; }

        public List<UniFormType> FormTypes { get; set; }

        public UniBatch()
        {
            Name = "";
            FormTypeName = cNoFormType;
            ID = Guid.NewGuid().ToString();
            Rejected = false;
            RejectedNote = "";
            CreationDate = System.DateTime.Now;
            Docs = new List<UniDocument>();
            Media = new List<UniMedium>();
            Pages = new List<UniPage>();
            Fields = new List<UniField>();
            FormTypes = new List<UniFormType>();
            UniParameterSet = new UniParameterSet();
            SupportDocumentStructure = true;
            IsRescan = false;
            ErrorMessage = "";
            this.AllowDocumentNaming = false;
            this.IgnoreBatch = false;

            UserID = "";
            SessionID = "";
            ExternalID = "";
            BranchID = "";
            ClientName = "";
            ProcessName = "";
            ProcessStepName = "";
            Priority = 5;
            ImageCompression = false;
            ScanUser = "";
            ScanStation = "";

            CreationUser = "";
        }

        public UniFormType GetFormType(string formTypeName)
        {
            foreach (UniFormType oFormType in FormTypes)
            {
                if (oFormType.Name.Equals(formTypeName))
                    return oFormType;
            }
            return null;
        }

        public List<string> GetListOfFormTypeNames()
        {
            List<string> oList = new List<string>();
            foreach (UniFormType oFormType in FormTypes)
            {
                if (oFormType.Visible)
                    oList.Add(oFormType.Name);
            }
            return oList;
        }
    }
}
