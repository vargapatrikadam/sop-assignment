using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogAppWcf
{
    class Host
    {
        public static Object thisLock = new Object();
        public static List<ClientInfo> loggedin = new List<ClientInfo>();
    }
}