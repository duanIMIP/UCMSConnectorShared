/****************************************************************************
'*   (c) Copyright IMIP Technology And Solution Consultancy JSC., 2010. All rights reserved.
'*   Unauthorized use, duplication or distribution is strictly prohibited.
'*****************************************************************************
'*
'*   File:       FormType.cs
'*
'*   Purpose:    Implement generic structure of form type
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Net;
using IMIP.UniversalScan.Profile;

namespace IMIP.UniversalScan.Def
{
     [Serializable]
    public class UniFormType
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public bool Visible { get; set; }

        //this is to keep the synchronization with external document type, for example xBound document type
        [XmlAttribute]
        public string ExternalID { get; set; }

        [XmlAttribute]
        public bool Root { get; set; }

        public List<UniFieldDefinition> FieldDefs { get; set; }

        public BatchNamingSetting DocumentNamingSetting { get; set; }

        public UniFormType()
        {
            Name = "";
            Visible = true;
            Root = true;
            FieldDefs = new List<UniFieldDefinition>();
            this.DocumentNamingSetting = new BatchNamingSetting();
        }

        public bool EnableDocumentNaming()
        {
            return this.DocumentNamingSetting.BatchNamingTemplate.Count > 0;
        }
    }
}
