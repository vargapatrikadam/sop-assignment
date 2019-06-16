using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogAppWcf
{
    static class PostMapper
    {
        public static PostModel EntityToModel(Post post)
        {
            PostModel newPostModel = new PostModel();
            newPostModel.Body = post.Body;
            newPostModel.Created_At = post.Created_At;
            newPostModel.Id = post.Id;
            newPostModel.Modified_At = post.Modified_At;
            newPostModel.User_Id = post.User_Id;
            newPostModel.User = UserMapper.EntityToModel(post.User);
            return newPostModel;
        }
        public static Post ModelToEntity(PostModel postModel)
        {
            Post newPost = new Post();
            newPost.Body = postModel.Body;
            newPost.Created_At = postModel.Created_At;
            newPost.Id = postModel.Id;
            newPost.User_Id = postModel.User_Id;
            return newPost;
        }
        public static ICollection<PostModel> EntityCollectionToModelCollection(ICollection<Post> postCollection)
        {
            ICollection<PostModel> collection = new List<PostModel>();
            foreach (Post item in postCollection)
            {
                collection.Add(EntityToModel(item));
            }
            return collection;
        }
        public static ICollection<Post> ModelCollectionToEntityCollection(ICollection<PostModel> postCollection)
        {
            ICollection<Post> collection = new List<Post>();
            foreach (PostModel item in postCollection)
            {
                collection.Add(ModelToEntity(item));
            }
            return collection;
        }
    }
}