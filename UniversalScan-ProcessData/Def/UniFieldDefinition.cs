/****************************************************************************
'*   (c) Copyright IMIP Technology And Solution Consultancy JSC., 2010. All rights reserved.
'*   Unauthorized use, duplication or distribution is strictly prohibited.
'*****************************************************************************
'*
'*   File:       FieldDefinition.cs
'*
'*   Purpose:    Implement generic structure of field definition
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IMIP.UniversalScan.Def
{
    public enum UniFieldDataType { StringType = 1, IntegerType = 2, DateTimeType = 3, LookupType = 4, BoolType = 5, FloatType = 6, BarcodeLookup = 7, Table = 8};

     [Serializable]
    public class ReferenceField
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string ID { get; set; }

        [XmlAttribute]
        public string FieldValue { get; set; }

        //Use for eDocmanPlus - SharePoint connector, some field have reference to other item collection.
        [XmlElement(IsNullable = true)]
        public object RefObjectData { get; set; }

        public ReferenceField()
        {
            this.Name = "";
            this.ID = "";
            this.FieldValue = "";
        }
    }

     [Serializable]
    public class UniFieldDefinition
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string DisplayName;

        [XmlAttribute]
        public bool Mandatory;

        [XmlAttribute]
        public bool ReadOnly;

        [XmlAttribute]
        public int MaxLength;

        [XmlAttribute]
        public string Format;

        [XmlAttribute]
        public UniFieldDataType DataType;

        [XmlAttribute]
        public string DefaultValue;

        [XmlAttribute]
        public string CustomProperty;

        [XmlAttribute]
        public bool MultiLine;

        [XmlAttribute]
        public bool MultiChoice;

        [XmlAttribute]
        public bool EDocManField;

        //this indicates that the field takes value from a pre-defined list named PreDefineListName
        [XmlAttribute]
        public string PreDefineListID;

        //this contains regular expression to validate the field content
        [XmlAttribute]
        public string ValidationFormat;

        [XmlAttribute]
        public string ExternalID;

        [XmlAttribute]
        public bool ValueFromBarcode;

        public List<UniColumn> Columns { get; set; }

        public Collection<ReferenceField> ReferenceFields { get; set; }

        public UniBoolProperty Visible { get; set; }

        public UniBoolProperty Editable { get; set; }

        public UniZone FieldZone { get; set; }

        [XmlAttribute]
        public int Index { get; set; }

        public UniFieldDefinition()
        {
            this.Name = "";
            this.DisplayName = "";
            this.DefaultValue = "";
            DataType = UniFieldDataType.StringType;

            MultiChoice = false;
            EDocManField = false;

            ReferenceFields = new Collection<ReferenceField>();

            Visible = new UniBoolProperty(true);
            Editable = new UniBoolProperty(true);

            PreDefineListID = "";
            ExternalID = "";
            ValidationFormat = "";
            CustomProperty = "";

            ValueFromBarcode = false;
            Columns = new List<UniColumn>();

            FieldZone = new UniZone();
        }

        public UniFieldDefinition(string sName)
        {
            this.Name = sName;
            this.DisplayName = "";
            this.DefaultValue = "";
            DataType = UniFieldDataType.StringType;

            MultiChoice = false;
            EDocManField = false;

            ReferenceFields = new Collection<ReferenceField>();

            Visible = new UniBoolProperty(true);
            Editable = new UniBoolProperty(true);

            PreDefineListID = "";
            ExternalID = "";
            ValidationFormat = "";
            CustomProperty = "";

            Columns = new List<UniColumn>();

            FieldZone = new UniZone();
        }

        public string GetReferenceID(string sName)
        {
            foreach (ReferenceField oField in ReferenceFields)
            {
                if (oField.Name == sName)
                    return oField.ID.ToString();
            }
            return "";
        }
    }

}
