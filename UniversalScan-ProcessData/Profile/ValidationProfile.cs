using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMIP.UniversalScan.Profile
{
    public interface ICustomValidation
    {
        bool Validated(UniversalScan.Data.UniBatch oBatch);
    }
     [Serializable]
    public class BatchStructureProfile
    {
        public string BatchType;
        public bool EnforceStructureValidation;
        public string BatchStructure;
        public bool RepeatDocuments;
    }
     [Serializable]
    public class ValidationProfile
    {
        public bool EnforceDefaultValidation;
        public bool EnforceCustomValidation;
        public string CustomValidationAssembly;
        public List<BatchStructureProfile> BatchStructureProfiles;

        public ValidationProfile()
        {
            BatchStructureProfiles = new List<BatchStructureProfile>();
        }
    }
}
