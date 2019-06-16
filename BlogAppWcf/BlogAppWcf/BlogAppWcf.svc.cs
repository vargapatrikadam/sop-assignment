using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BlogAppWcf
{
    public class BlogAppService : IBlogAppService
    {
        public void AddPost(string body, ClientInfo client)
        {
            try
            {
                if (HasGuid(client))
                {
                    using (var context = new BlogDbEntities())
                    {
                        #region nem tárolt eljárással insert
                        //Post newPost = new Post();
                        //newPost.Body = body;
                        //newPost.User_Id = user_id;
                        //newPost.Created_At = DateTime.Now;
                        //context.Post.Add(newPost);
                        //context.SaveChanges();
                        #endregion
                        context.InsertNewPost(body, client.User_Id);
                    }
                }
            }
            catch (NullReferenceException)
            {
                throw new FaultException<UserNotFoundFault>(new UserNotFoundFault("Nem létezik ilyen felhasználó!"), new FaultReason("Nem létezik ilyen felhasználó!"));
            }
            catch (Exception e)
            {
                throw new FaultException(new FaultReason(e.Message));
            }
        }
        public void DeletePost(int id, ClientInfo client)
        {
            try
            {
                if (HasGuid(client))
                {
                    using (var context = new BlogDbEntities())
                    {
                        #region nem tárolt eljárással törlés
                        //Post deleteThis = context.Post.Find(id);
                        //context.Post.Remove(deleteThis);
                        //context.SaveChanges();
                        #endregion
                        try
                        {
                            context.DeletePost(id);
                        }
                        catch (InvalidOperationException)
                        {
                            throw new FaultException<PostNotFoundFault>(new PostNotFoundFault("Ez a poszt nem található!"), new FaultReason("Ez a poszt nem található!"));
                        }
                    }
                }
            }
            catch (NullReferenceException)
            {
                throw new FaultException<UserNotFoundFault>(new UserNotFoundFault("Nem létezik ilyen felhasználó!"), new FaultReason("Nem létezik ilyen felhasználó!"));
            }
            catch (Exception e)
            {
                throw new FaultException(new FaultReason(e.Message));
            }
        }

        public void ModifyPost(int id, string body, ClientInfo client)
        {
            try
            {
                if (HasGuid(client))
                {
                    try
                    {
                        using (var context = new BlogDbEntities())
                        {
                            Post modifyThis = context.Post.Find(id);
                            modifyThis.Body = body;
                            modifyThis.Modified_At = DateTime.Now;
                            context.Entry(modifyThis).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        throw new FaultException<PostNotFoundFault>(new PostNotFoundFault("Ez a poszt nem található!"), new FaultReason("Ez a poszt nem található!"));
                    }
                }
            }
            catch (NullReferenceException)
            {
                throw new FaultException<UserNotFoundFault>(new UserNotFoundFault("Nem létezik ilyen felhasználó!"), new FaultReason("Nem létezik ilyen felhasználó!"));
            }
        }
        private bool HasUserName(string username)
        {
            using (var context = new BlogDbEntities())
            {
                var hasuser = context.User.Any(u => u.UserName.ToLower() == username.ToLower());
                return hasuser;
            }
        }
        public void RegisterUser(string username, string password, string RetryPassword, string FirstName, string LastName)
        {
            using (var context = new BlogDbEntities())
            {
                try
                {
                    if (HasUserName(username))
                    {
                        throw new UserAlreadyExistsException();
                    }
                    else if (password == RetryPassword)
                    {
                        User NewUser = new User();
                        NewUser.UserName = username;
                        NewUser.Password = password;
                        NewUser.FirstName = FirstName;
                        NewUser.LastName = LastName;
                        NewUser.Created_At = DateTime.Now;
                        context.User.Add(NewUser);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new PasswordsNotEqualException();
                    }
                }
                catch (UserAlreadyExistsException)
                {
                    throw new FaultException<UserAlreadyExistsFault>(new UserAlreadyExistsFault("Ez a felhasználó már létezik!"), new FaultReason("Ez a felhasználó már létezik!"));
                }
                catch (PasswordsNotEqualException)
                {
                    throw new FaultException<UserAlreadyExistsFault>(new UserAlreadyExistsFault("A két jelszó nem egyezik!"), new FaultReason("A két jelszó nem egyezik!"));
                }
                catch (Exception e)
                {
                    throw new FaultException(new FaultReason(e.Message));
                }
            }
        }

        public ClientInfo UserLogin(string username, string password)
        {
            using (var context = new BlogDbEntities())
            {
                try
                {
                    var hasUser = context.User.Any(user => user.UserName.ToLower() == username.ToLower());
                    if (!hasUser)
                        throw new UserNotFoundException();
                    else
                    {
                        var user = context.User.Where(u => u.UserName.ToLower() == username.ToLower()).First();
                        if (user.Password != password)
                            throw new NotCorrectPasswordException();
                        else
                        {
                            ClientInfo client = new ClientInfo(user.Id, Guid.NewGuid().ToString());
                            lock (Host.loggedin)
                            {
                                Host.loggedin.Add(client);
                            }
                            return client;
                        }
                    }
                }
                catch (UserNotFoundException)
                {
                    throw new FaultException<UserNotFoundFault>(new UserNotFoundFault("Ez a felhasználó nem létezik!"), new FaultReason("Ez a felhasználó nem létezik!"));
                }
                catch (NotCorrectPasswordException)
                {
                    throw new FaultException<NotCorrectPasswordFault>(new NotCorrectPasswordFault("Helytelen jelszó!"), new FaultReason("Helytelen jelszó!"));
                }
                catch (Exception e)
                {
                    throw new FaultException(new FaultReason(e.Message));
                }
            }

        }
        public void UserLogout(ClientInfo client)
        {
            try
            {
                if (HasGuid(client))
                {
                    using (var context = new BlogDbEntities())
                    {
                        var CurrentUser = context.User.Find(client.User_Id);
                        CurrentUser.Last_Login = DateTime.Now;
                        context.Entry(CurrentUser).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        var item = Host.loggedin.Where(p => p.User_Id == client.User_Id && p.Guid == client.Guid).First();
                        Host.loggedin.Remove(item);
                    }
                }
            }
            catch (NullReferenceException e)
            {
                throw new FaultException<UserNotFoundFault>(new UserNotFoundFault("Nem létezik ilyen felhasználó!"), new FaultReason("Nem létezik ilyen felhasználó!"));
            }
            catch (Exception e)
            {
                throw new FaultException(new FaultReason(e.Message));
            }
        }
        public ICollection<PostModel> GetAllPosts(ClientInfo client)
        {
            try
            {
                if (HasGuid(client))
                {
                    using (var context = new BlogDbEntities())
                    {
                        ICollection<PostModel> coll = PostMapper.EntityCollectionToModelCollection(context.Post.OrderByDescending(p => p.Id).ToList());
                        return coll;
                    }
                }
                else return null;
            }
            catch (NullReferenceException)
            {
                throw new FaultException<UserNotFoundFault>(new UserNotFoundFault("Nem létezik ilyen felhasználó!"), new FaultReason("Nem létezik ilyen felhasználó!"));
            }
            catch (Exception e)
            {
                throw new FaultException(new FaultReason(e.Message));
            }
        }
        public ICollection<PostModel> GetUserPosts(ClientInfo client)
        {
            try
            {
                if (HasGuid(client))
                {
                    using (var context = new BlogDbEntities())
                    {
                        ICollection<PostModel> coll = PostMapper.EntityCollectionToModelCollection(context.Post.Where(p => p.User_Id == client.User_Id).OrderByDescending(p => p.Id).ToList());
                        return coll;
                    }
                }
                else return null;
            }
            catch (NullReferenceException e)
            {
                throw new FaultException<UserNotFoundFault>(new UserNotFoundFault("Nem létezik ilyen felhasználó!"), new FaultReason("Nem létezik ilyen felhasználó!"));
            }
            catch (Exception e)
            {
                throw new FaultException(new FaultReason(e.Message));
            }
        }
        public string GetPostBody(int id, ClientInfo client)
        {
            try
            {
                if (HasGuid(client))
                {
                    try
                    {
                        using (var context = new BlogDbEntities())
                        {
                            var getpost = context.Post.Where(p => p.Id == id).First();
                            return getpost.Body;
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        throw new FaultException<PostNotFoundFault>(new PostNotFoundFault("Ez a poszt nem található!"), new FaultReason("Ez a poszt nem található!"));
                    }
                }
                else return null;
            }
            catch (NullReferenceException)
            {
                throw new FaultException<UserNotFoundFault>(new UserNotFoundFault("Nem létezik ilyen felhasználó!"), new FaultReason("Nem létezik ilyen felhasználó!"));
            }
            catch (Exception e)
            {
                throw new FaultException(new FaultReason(e.Message));
            }
        }
        private bool HasGuid(ClientInfo client)
        {
            //muszáj foreachel végigmenni a listán, ugyanis a klienstől visszakapott szerializált ClientInfo tipusú objektum, amit a loginkor átadtunk, NEM ekvivalens a Host.loggedin-ban tárolt objektummal, a benne tárolt értékek viszont igen, így könnyen megtalálható a keresett objektum
            try
            {
                foreach (var item in Host.loggedin)
                {
                    if (item.Guid == client.Guid && item.User_Id == client.User_Id)
                        return true;
                }
                return false;
            }
            catch (NullReferenceException e)
            {
                throw new FaultException<UserNotFoundFault>(new UserNotFoundFault("Nem létezik ilyen felhasználó!"), new FaultReason("Nem létezik ilyen felhasználó!"));
            }
            catch (Exception e)
            {
                throw new FaultException(new FaultReason(e.Message));
            }
        }
    }
}
