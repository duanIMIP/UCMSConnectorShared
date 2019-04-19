using IMIP.UniversalScan.Data;
using System;

namespace IMIP.UniversalScan.Def
{
    [Serializable]
    public class UniBoolProperty : UniProperty
    {
        public bool Value { get; set; }

        private UniBoolProperty() : base()
        {
            Value = true;
        }

        public UniBoolProperty(bool defaultValue) : base()
        {
            Value = defaultValue;
            Privilege.Add(new Account() { Name = Account.Everyone });
        }
    }
}
