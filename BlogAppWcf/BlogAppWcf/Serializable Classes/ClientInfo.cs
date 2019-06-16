using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BlogAppWcf
{
    [DataContract]
    public class ClientInfo
    {
        public ClientInfo(int User_Id, string Guid)
        {
            this.User_Id = User_Id;
            this.Guid = Guid;
        }
        [DataMember]
        private int user_Id;
        [DataMember]
        public int User_Id
        {
            get { return user_Id; }
            set { user_Id = value; }
        }
        [DataMember]
        private string guid;
        [DataMember]
        public string Guid
        {
            get { return guid; }
            set { guid = value; }
        }
    }
}