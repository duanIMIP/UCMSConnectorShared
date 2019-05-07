using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UCMS.ImportController.Data
{
    [XmlRoot("XmlRanDomContent")]
    public class XmlRanDomContent
    {
        [XmlElement("oBranch")]
        public DataValue oBranch { get; set; }
        [XmlElement("oFolder")]
        public DataValue oFolder { get; set; }
        [XmlElement("oWorkflow")]
        public DataValue oWorkflow { get; set; }
        [XmlElement("oWorkflowStep")]
        public DataValue oWorkflowStep { get; set; }
        [XmlElement("oContentType")]
        public DataValue oContentType { get; set; }
        [XmlElement("oContentTypeParent")]
        public DataValue oContentTypeParent { get; set; }
        [XmlElement("oContentField")]
        public List<DataValue> oContentField { get; set; }
        [XmlElement("oLibraryField")]
        public List<DataValue> oLibraryField { get; set; }
        [XmlElement("oContentParent")]
        public List<DataValue> oContentParent { get; set; }
        [XmlElement("oLibraryParent")]
        public List<DataValue> oLibraryParent { get; set; }

        public XmlRanDomContent()
        {
            oBranch = new DataValue();
            oFolder = new DataValue();
            oWorkflow = new DataValue();
            oWorkflowStep = new DataValue();
            oContentType = new DataValue();
            oContentTypeParent = new DataValue();
            oContentField = new List<DataValue>();
            oLibraryField = new List<DataValue>();
            oContentParent = new List<DataValue>();
            oLibraryParent = new List<DataValue>();
        }
    }
}
