using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMIP.UniversalScan.Def;

namespace IMIP.UniversalScan.Profile
{
     [Serializable]
    public class UniPreDefineField
    {
        private List<string> m_PreDefineValues;

        public string Name;

        public string UniqueID;

        public List<string> PreDefineValues
        {
            get
            {
                if (m_PreDefineValues == null)
                    m_PreDefineValues = new List<string>();
                return m_PreDefineValues;
            }
            set
            {
                m_PreDefineValues = value;
            }
        }

        public UniPreDefineField()
        {
            UniqueID = Guid.NewGuid().ToString();
            PreDefineValues = new List<string>();
        }
    }

     [Serializable]
    public class DocumentTypeProfile
    {
        //this contains the list of pre-define field list
        public List<UniPreDefineField> PreDefineFieldList;

        //this contains the list of Uni FormType defined for this profile
        public List<UniFormType> UniFormtypeList;


        public DocumentTypeProfile()
        {
            PreDefineFieldList = new List<UniPreDefineField>();

            UniFormtypeList = new List<UniFormType>();
        }

    }
}
