using BlogApp.BlogService;
using BlogApp.BusinessLogic;
using BlogApp.Command;
using BlogApp.UI;
using BlogApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BlogApp.ViewModel
{
    class MainContainerWindowViewModel : ObservableObject
    {
        public static void UserLogout()
        {
            try
            {
                UIRepository.Instance.LocalClient.UserLogout(UIRepository.Instance.Client);     
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show("Hiba a szolgáltatóval!");
            }
            finally
            {
                UIRepository.Instance.Client = null;
                new LoginRegContainer().Show();
                Application.Current.Windows[0].Close();
            }
        }
        private ICommand _Logout;
        public ICommand Logout
        {
            get
            {
                if (_Logout == null)
                {
                    _Logout = new RelayCommand(p => true, p => UserLogout());
                }
                return _Logout;
            }
        }
        private ICommand _Refresh;
        public ICommand Refresh
        {
            get
            {
                if (_Refresh == null)
                {
                    _Refresh = new RelayCommand(p => true, p => RefreshPosts());
                }
                return _Refresh;
            }
        }
        private void RefreshPosts()
        {
            EventAggregator.BroadCast("Posts changed");
        }
        private string _PostBody;
        public string PostBody
        {
            set
            {
                this._PostBody = value;
            }
            get
            {
                return _PostBody;
            }
        }
        private ICommand _AddPost;
        public ICommand AddPost
        {
            get
            {
                if (_AddPost == null)
                {
                    _AddPost = new RelayCommand(p => true, p => AddNewPost(PostBody));
                }
                return _AddPost;
            }
        }
        public void AddNewPost(string body)
        {
            if (body != null && body.Length < 100)
            {
                try
                { 
                    UIRepository.Instance.LocalClient.AddPost(body, UIRepository.Instance.Client);
                    EventAggregator.BroadCast("Posts changed");
                }
                catch (FaultException<UserNotFoundFault>)
                {
                    MessageBox.Show("Hibás kapcsolat!");
                    UserLogout();
                }
                catch (FaultException e)
                {
                    MessageBox.Show("Ismeretlen hiba: " + e.Reason.ToString());
                }
                catch (EndpointNotFoundException)
                {
                    UserLogout();
                }
            }
            else MessageBox.Show("A poszt nem lehet 100 karakternél hosszabb, és minimum 1 karakter!");            
        }
    }
}
