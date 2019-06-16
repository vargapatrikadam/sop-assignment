using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BlogAppWcf
{
    [ServiceContract]
    public interface IBlogAppService
    {
        [OperationContract]
        [FaultContract(typeof(UserNotFoundFault))]
        [FaultContract(typeof(NotCorrectPasswordFault))]
        ClientInfo UserLogin(string username, string password);

        [OperationContract]
        [FaultContract(typeof(UserNotFoundFault))]
        void UserLogout(ClientInfo client);

        [OperationContract]
        [FaultContract(typeof(UserNotFoundFault))]
        void AddPost(string body, ClientInfo client);

        [OperationContract]
        [FaultContract(typeof(PostNotFoundFault))]
        [FaultContract(typeof(UserNotFoundFault))]
        void ModifyPost(int id, string body, ClientInfo client);

        [OperationContract]
        [FaultContract(typeof(PostNotFoundFault))]
        [FaultContract(typeof(UserNotFoundFault))]
        void DeletePost(int id, ClientInfo client);

        [OperationContract]
        [FaultContract(typeof(UserAlreadyExistsFault))]
        [FaultContract(typeof(PasswordsNotEqualFault))]
        void RegisterUser(string username, string password, string RetryPassword, string FirstName, string LastName);

        [OperationContract]
        [FaultContract(typeof(UserNotFoundFault))]
        ICollection<PostModel> GetAllPosts(ClientInfo client);

        [OperationContract]
        [FaultContract(typeof(UserNotFoundFault))]
        ICollection<PostModel> GetUserPosts(ClientInfo client);

        [OperationContract]
        [FaultContract(typeof(PostNotFoundFault))]
        [FaultContract(typeof(UserNotFoundFault))]
        string GetPostBody(int id, ClientInfo client);
    }
}