using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using IMIP.UniversalScan.Data;

namespace IMIP.UniversalScan.Connector.UCMSConnectorShared
{
    [Serializable]
    [XmlRootAttribute(ElementName = "ActivityConfiguration")]
    public class GlobalParameterSet
    {
        public List<Branch> Branches { get; set; }
        public List<Report> Reports { get; set; }
		public List<NonADAccount> NonADAccounts { get; set; }

        public string ADAdmin;
        public string ADPassword;
        public bool UseExplicitAccountForAD;
        public const string AllBranch = "All";
        public string mVersion = "1.0.1";

        public GlobalParameterSet()
        {
            this.Branches = new List<Branch>(); 
            this.Reports = new List<Report>();
			this.NonADAccounts = new List<NonADAccount>();
            ADAdmin = "";
            ADPassword = "";
            UseExplicitAccountForAD = true;
        }
    }

    
}
