/****************************************************************************
'*   (c) Copyright IMIP Technology And Solution Consultancy JSC., 2010. All rights reserved.
'*   Unauthorized use, duplication or distribution is strictly prohibited.
'*****************************************************************************
'*
'*   File:       Document.cs
'*
'*   Purpose:    Implement generic structure of document
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using IMIP.UniversalScan.Def;

namespace IMIP.UniversalScan.Data
{
    public class UniDocument
    {
        [XmlIgnore]
        public static string cDeleteDoc = "[IS_DELETED]";

        [XmlIgnore]
        public static string cNoFormType = "[No Form Type]";

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string ID { get; set; }

        [XmlAttribute]
        public string FormTypeName { get; set; }

        [XmlAttribute]
        public DateTime CreationDate { get; set; }

        [XmlAttribute]
        public bool IsRescan { get; set; }
        
        [XmlAttribute]
        public bool Rejected { get; set; }

        [XmlAttribute]
        public string RejectedNote { get; set; }

        [XmlAttribute]
        public int Index { get; set; }

        [XmlAttribute]
        public string USC_Barcode { get; set; }

        //[XmlIgnore]
        public object CustomProperties { get; set; }

        public UniFormType UniFormType { get; set; }

        public List<UniMedium> Media { get; set; }
        public List<UniPage> Pages { get; set; }
        public List<UniField> Fields { get; set; }
        

        public UniDocument()
        {
            Name = "";
            FormTypeName = cNoFormType;
            ID = "";
            IsRescan = false;
            Rejected = false;
            RejectedNote = "";
            USC_Barcode = "";
            Media = new List<UniMedium>();
            Pages = new List<UniPage>();
            Fields = new List<UniField>();
            
        }

        public UniField GetFieldByName(string Name)
        {
            
            foreach (UniField oField in Fields)
            {
                if (oField.Name.Equals(Name))
                    return oField;
            }
            
            return null;
        }
    }
}
