using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogAppWcf
{
    public class NotCorrectPasswordException : Exception
    {
        public NotCorrectPasswordException()
        {
        }
        public NotCorrectPasswordException(string message)
        : base(message)
        {
        }
        public NotCorrectPasswordException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

}