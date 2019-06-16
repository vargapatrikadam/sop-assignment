using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BlogAppWcf
{
    [DataContractAttribute]
    public class PasswordsNotEqualFault
    {
        private string report;

        public PasswordsNotEqualFault(string message)
        {
            this.report = message;
        }

        [DataMemberAttribute]
        public string Message
        {
            get { return this.report; }
            set { this.report = value; }
        }
    }
}