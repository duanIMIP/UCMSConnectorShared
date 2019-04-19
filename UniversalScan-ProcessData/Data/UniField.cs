/****************************************************************************
'*   (c) Copyright IMIP Technology And Solution Consultancy JSC., 2010. All rights reserved.
'*   Unauthorized use, duplication or distribution is strictly prohibited.
'*****************************************************************************
'*
'*   File:       Field.cs
'*
'*   Purpose:    Implement generic structure of field
*********************************************************************************/
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;
using IMIP.UniversalScan.Def;

namespace IMIP.UniversalScan.Data
{
    [Serializable]
    public class UniChar
    {
        [XmlAttribute]
        /// <summary>
        /// code of the char
        /// </summary>
        public int V { get; set; }

        [XmlAttribute]
        /// <summary>
        /// status valid/invalid - true/false
        /// </summary>
        public bool S { get; set; }

        public UniChar(int code, bool valid)
        {
            V = code;
            S = valid;
        }
        public UniChar()
        {
            V = 32;
            S = false;
        }
    }
    [Serializable]
    public class CustomFieldData
    {
        
        [XmlAttribute]
        public int Left { get; set; }
        [XmlAttribute]
        public int Top { get; set; }
        [XmlAttribute]
        public int Width { get; set; }
        [XmlAttribute]
        public int Height { get; set; }
        [XmlAttribute]
        public int PageNo { get; set; }

        public List<UniChar> Chars { get; set; }

        public CustomFieldData()
        {
            Chars = new List<UniChar>();
        }

    }
    public class UniField
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Value { get; set; }

        [XmlAttribute]
        public bool ValidationStatus { get; set; }

        [XmlIgnore]
        public UniFieldDefinition FieldDef { get; set; }

        public CustomFieldData CustomData { get; set; }

        public List<UniRow> Rows { get; set; }

        //[XmlIgnore]
        public object CustomProperties { get; set; }
        public string ValidationResult { get; set; }

        public UniField()
        {
            this.Name = "";
            this.Value= "";
            this.CustomData = new CustomFieldData();
            this.Rows = new List<UniRow>();
        }

        public UniField(string sName, string sValue)
        {
            this.Name = sName;
            this.Value = sValue;
            this.CustomData = new CustomFieldData();
            this.Rows = new List<UniRow>();
        }
    }
}