using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BlogAppWcf
{
    [DataContract]
    [KnownType(typeof(UserModel))]
    public class PostModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Body { get; set; }
        [DataMember]
        public DateTime Created_At { get; set; }
        [DataMember]
        public Nullable<DateTime> Modified_At { get; set; }
        [DataMember]
        public int User_Id { get; set; }

        [DataMember]
        public UserModel User { get; set; }
    }
}