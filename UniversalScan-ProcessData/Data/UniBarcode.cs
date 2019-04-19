/****************************************************************************
'*   (c) Copyright IMIP Technology And Solution Consultancy JSC., 2014. All rights reserved.
'*   Unauthorized use, duplication or distribution is strictly prohibited.
'*****************************************************************************
'*
'*   File:       UniBarcode.cs
'*
'*   Purpose:    Implement generic structure of Barcode
*********************************************************************************/
using System.Xml.Serialization;

namespace IMIP.UniversalScan.Data
{
    public class UniBarcode
    {
        [XmlAttribute]
        public string Type { get; set; }

        [XmlAttribute]
        public string Value { get; set; }

        [XmlAttribute]
        public int Width { get; set; }

        [XmlAttribute]
        public int Height { get; set; }

        [XmlAttribute]
        public int Left { get; set; }

        [XmlAttribute]
        public int Top { get; set; }

        public UniBarcode()
        {
            Type = "";
            Value = "";
            Width = 0;
            Height = 0;
            Left = 0;
            Width = 0;
        }
    }
}
