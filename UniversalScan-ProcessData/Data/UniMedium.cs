/****************************************************************************
'*   (c) Copyright IMIP Technology And Solution Consultancy JSC., 2010. All rights reserved.
'*   Unauthorized use, duplication or distribution is strictly prohibited.
'*****************************************************************************
'*
'*   File:       Medium.cs
'*
'*   Purpose:    Implement generic structure of medium
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace IMIP.UniversalScan.Data
{
    public class UniMedium
    {
        //[XmlAttribute]
        //public string Name { get; set; }

        //[XmlAttribute]
        //public bool Rear { get; set; }

        [XmlAttribute]
        public string Format { get; set; }

        [XmlAttribute]
        public string Path { get; set; }
    }
}
