/****************************************************************************
'*   (c) Copyright IMIP Technology And Solution Consultancy JSC., 2010. All rights reserved.
'*   Unauthorized use, duplication or distribution is strictly prohibited.
'*****************************************************************************
'*
'*   File:       Page.cs
'*
'*   Purpose:    Implement generic structure of page
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace IMIP.UniversalScan.Data
{
    public class UniPage
    {
        [XmlAttribute]
        public string ID { get; set; }

        [XmlAttribute]
        public string FullFileName { get; set; }

        [XmlAttribute]
        public bool Rejected { get; set; }

        [XmlAttribute]
        public bool IsRescan { get; set; }

        [XmlAttribute]
        public bool IsNew { get; set; }

        [XmlAttribute]
        public string SheetID { get; set; }

        [XmlAttribute]
        public string RejectedNote { get; set; }

        public List<UniBarcode> Barcodes { get; set; }
        //[XmlIgnore]
        public object CustomProperties { get; set; }

        public UniPage()
        {
            ID = "";
            SheetID = "";
            FullFileName = "";
            Rejected = false;
            RejectedNote = "";
            Barcodes = new List<UniBarcode>();
        }
    }
}
