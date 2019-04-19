using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMIP.UniversalScan.Common
{
    public class AuthResult
    {
        public bool Authenticated { get; set; }
        public string UserName { get; set; }
        public string Domain { get; set; }
        public string Password { get; set; }
        public string Error { get; set; }

        public AuthResult(string sName, string sPassword, string sDomain, bool bAuthenticated)
        {
            UserName = sName;
            Password = sPassword;
            Domain = sDomain;
            Authenticated = bAuthenticated;
        }

        public AuthResult()
        {
        }
    }

    
}
