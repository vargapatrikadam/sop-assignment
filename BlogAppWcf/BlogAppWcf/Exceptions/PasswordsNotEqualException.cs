using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogAppWcf
{
    public class PasswordsNotEqualException : Exception
    {
        public PasswordsNotEqualException()
        {
        }
        public PasswordsNotEqualException(string message)
        : base(message)
        {
        }
        public PasswordsNotEqualException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

}