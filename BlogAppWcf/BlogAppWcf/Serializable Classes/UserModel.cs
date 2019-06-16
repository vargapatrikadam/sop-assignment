using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BlogAppWcf
{
    [DataContract]
    [KnownType(typeof(PostModel))]
    public class UserModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public System.DateTime Created_At { get; set; }
        [DataMember]
        public Nullable<System.DateTime> Last_Login { get; set; }

        [DataMember]
        public ICollection<PostModel> Post { get; set; }
    }
}