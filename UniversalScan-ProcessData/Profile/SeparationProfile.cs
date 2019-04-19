using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IMIP.UniversalScan.Profile
{
    
    public enum SeparationType { FixPage, BlankPage, Barcode, Custom };

    public enum BarcodeType
    {
        bc128 = 4096,
        bc39 = 32,
        bcCODE93 = 16384,
        bcBcdMatrix = 128,
        bcCodeabar = 64,
        bcEAN8 = 131072,
        bcEAN13 = 32768,
        bcUPCA = 65536,
        bcUPCE = 262144,
        bcADD2 = 1048576,
        bcADD5 = 524288,
        bcIndustrial2of5 = 1,
        bcInverted2of5 = 2,
        bcInterleaved2of5 = 4,
        bcIata2of5 = 8,
        bcMatrix2of5 = 16,
        bcDataLogic2of5 = 256,
        bcQRcode = 5000000
    }


    public enum ComparisonType { startwith, contain, endwith, exact };
     [Serializable]
    public class BarcodeSeparationSetting
    {
        public BarcodeType eBarcode;
        public ComparisonType eComparison;
        public string CompareValue;
        public string FormType;
        public bool DiregardPage;
        public bool CombineCheckWithMinimumPageCount;
        public int MinimumPageCount;
    }
     [Serializable]
    public class FixPageSeparationSetting
    {
        public int PageCount;
        public string FormType;

        public FixPageSeparationSetting()
        {
            this.PageCount = 1;
        }
    }
     [Serializable]
    public class BlankPageSeparationSetting
    {
        public string FormType;
        public bool DiregardPage;
        public float Threshold = 99;//default value 99 means 99% of pixels are blank
    }
     [Serializable]
    public class CustomSeparationSetting
    {
        public string ProjectFile;
    }
     [Serializable]
    [XmlRootAttribute(ElementName = "SeparationProfile")]
    public class SeparationProfile
    {
        [XmlAttribute]
        public bool Active;

        [XmlAttribute]
        public SeparationType eSeparationType;

        [XmlAttribute]
        public string AlternativeFormType;

        public FixPageSeparationSetting FixPageSeparationSetting { get; set; }

        public BlankPageSeparationSetting BlankPageSeparationSetting { get; set; }

        public CustomSeparationSetting CustomSeparationSetting { get; set; }

        public Collection<BarcodeSeparationSetting> BarcodeSeparationSettings { get; set; }

        public SeparationProfile()
        {
            BarcodeSeparationSettings = new Collection<BarcodeSeparationSetting>();
            FixPageSeparationSetting = new FixPageSeparationSetting();
            BlankPageSeparationSetting = new BlankPageSeparationSetting();
            CustomSeparationSetting = new CustomSeparationSetting();

            this.AlternativeFormType = "";
            this.eSeparationType = SeparationType.FixPage;
        }
    }
}
