using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogAppWcf
{
    static class UserMapper
    {
        public static UserModel EntityToModel(User user)
        {
            UserModel newUserModel = new UserModel();
            newUserModel.Id = user.Id;
            newUserModel.LastName = user.LastName;
            newUserModel.Last_Login = user.Last_Login;
            newUserModel.Password = user.Password;
            newUserModel.UserName = user.UserName;
            newUserModel.FirstName = user.FirstName;
            newUserModel.Created_At = user.Created_At;
            List<PostModel> posts = new List<PostModel>();
            foreach (var item in user.Post)
            {
                PostModel newPostModel = new PostModel();
                newPostModel.Body = item.Body;
                newPostModel.Created_At = item.Created_At;
                newPostModel.Id = item.Id;
                newPostModel.Modified_At = item.Modified_At;
                newPostModel.User_Id = item.User_Id;
                posts.Add(newPostModel);
            }

            return newUserModel;
        }
        public static User ModelToEntity(UserModel userModel)
        {
            User newUser = new User();
            newUser.Created_At = userModel.Created_At;
            newUser.FirstName = userModel.FirstName;
            newUser.Id = userModel.Id;
            newUser.LastName = userModel.LastName;
            newUser.Last_Login = userModel.Last_Login;
            newUser.Password = userModel.Password;
            newUser.UserName = userModel.UserName;

            return newUser;
        }
        public static ICollection<UserModel> EntityCollectionToModelCollection(ICollection<User> userCollection)
        {
            List<UserModel> collection = new List<UserModel>();
            foreach (User item in userCollection)
            {
                collection.Add(EntityToModel(item));
            }
            return collection;
        }
        public static ICollection<User> ModelCollectionToEntityCollection(ICollection<UserModel> userCollection)
        {
            List<User> collection = new List<User>();
            foreach (UserModel item in userCollection)
            {
                collection.Add(ModelToEntity(item));
            }
            return collection;
        }
    }
}