using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogAppWcf
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException()
        {
        }
        public UserAlreadyExistsException(string message)
        : base(message)
        {
        }
        public UserAlreadyExistsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}