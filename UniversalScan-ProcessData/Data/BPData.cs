using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using IMIP.UniversalScan.Def;

namespace IMIP.UniversalScan.Data
{

    public class BPData
    {
        public List<BPClient> bpClients { get; set; }
        public BPData()
        {
            this.bpClients = new List<BPClient>();
        }
    }

    public class BPClient
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public List<BPProcess> bpProcesses { get; set; }

        [XmlIgnore]
        public object Tag { get; set; }

        public BPClient()
        {
            Name = "";
            bpProcesses = new List<BPProcess>();
        }
    }

    public class BPProcess
    {
        public string Name { get; set; }
        public string ID { get; set; }

        [XmlIgnore]
        public object Tag { get; set; }
        public List<BPProcessStep> bpProcessSteps { get; set; }
        public List<UniFormType> uniFormTypes { get; set; }
        public BPProcess()
        {
            Name = "";
            bpProcessSteps = new List<BPProcessStep>();
            uniFormTypes = new List<UniFormType>();
        }
    }

    public class BPProcessStep
    {
        public const string Activity_UniversalScan = "UniversalScan";
        public const string Activity_UniversalRelease = "UniversalRelease";

        public string Name { get; set; }
        public string ID { get; set; }
        public string FeatureName { get; set; }
        public string FeatureID { get; set; }

        [XmlIgnore]
        public object Tag { get; set; }
        public UniParameterSet uniParameterSet { get; set; }
        //public string uniParameterSet { get; set; }
        //public UniVerifierSetting UniVerifierSetting { get; set; }

        public BPProcessStep()
        {
            this.Name = "";
            this.FeatureName = "";
            //uniParameterSet = "";
            uniParameterSet = new UniParameterSet();
        }
    }
}
