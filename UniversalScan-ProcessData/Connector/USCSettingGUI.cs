using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace IMIP.UniversalScan.Connector
{
    public class USCSettingGUI
    {
        public bool ShowIndexPanel { get; set; }

        public bool EnableSoftwareImport { get; set; }
        public List<string> SetScannerSources { get; set; }

        public USCSettingGUI()
        {
            this.ShowIndexPanel = true;
            this.EnableSoftwareImport = true;
            this.SetScannerSources = new List<string>();
        }
    }
}
