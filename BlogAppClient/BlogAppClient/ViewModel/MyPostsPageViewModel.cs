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
    class MyPostsPageViewModel : ObservableObject
    {
        public MyPostsPageViewModel()
        {
            EventAggregator.OnMessageTransmitted += OnMessageReceived;
        }
        private void OnMessageReceived(string message)
        {
            if (message == "Posts changed" || message == "Posts updated")
            {
                try
                {
                    _MyPosts = UIRepository.Instance.LocalClient.GetUserPosts(UIRepository.Instance.Client);
                    NotifyPropertyChanged("MyPosts");
                }
                catch (EndpointNotFoundException)
                {
                    MainContainerWindowViewModel.UserLogout();
                }
                catch (FaultException<UserNotFoundFault>)
                {
                    MessageBox.Show("Hibás kapcsolat!");
                    MainContainerWindowViewModel.UserLogout();
                }
                catch (FaultException e)
                {
                    MessageBox.Show("Ismeretlen hiba: " + e.Reason.ToString());
                }
            } 
        }
        private ICollection<PostModel> _MyPosts;
        public ICollection<PostModel> MyPosts
        {
            get
            {
                if (_MyPosts == null)
                {
                    try
                    {
                        _MyPosts = UIRepository.Instance.LocalClient.GetUserPosts(UIRepository.Instance.Client);
                        NotifyPropertyChanged("MyPosts");
                        return _MyPosts;
                    }
                    catch (FaultException<UserNotFoundFault>)
                    {
                        MessageBox.Show("Hibás kapcsolat!");
                        MainContainerWindowViewModel.UserLogout();
                    }
                    catch (FaultException e)
                    {
                        MessageBox.Show("Ismeretlen hiba: " + e.Reason.ToString());
                    }
                    catch (EndpointNotFoundException)
                    {
                        MainContainerWindowViewModel.UserLogout();
                    }
                    return null;
                }
                else return _MyPosts;
            }
        }
        private ICommand _ModifyPost;
        public ICommand ModifyPost
        {
            get
            {
                if (_ModifyPost == null)
                {
                    _ModifyPost = new RelayCommand(p => true, p => SetModifyableId(Convert.ToInt32(p)));
                }
                return _ModifyPost;
            }
        }
        private void SetModifyableId(int id)
        {
            UIRepository.Instance.ModifyPost = id;
            new ModifyPostWindow().Show();
        }
        private ICommand _DeletePost;
        public ICommand DeletePost
        {
            get
            {
                if (_DeletePost == null)
                {
                    _DeletePost = new RelayCommand(p => true, p => DelPost(Convert.ToInt32(p)));
                }
                return _DeletePost;
            }
        }
        private void DelPost(int id)
        {
            try
            {
                UIRepository.Instance.LocalClient.DeletePost(id, UIRepository.Instance.Client);
                EventAggregator.BroadCast("Posts changed");
                NotifyPropertyChanged("MyPosts");
            }
            catch (EndpointNotFoundException)
            {
                MainContainerWindowViewModel.UserLogout();
            }
            catch (FaultException<UserNotFoundFault>)
            {
                MessageBox.Show("Hibás kapcsolat!");
                MainContainerWindowViewModel.UserLogout();
            }
            catch (FaultException<PostNotFoundFault> e)
            {
                MessageBox.Show(e.Reason.ToString());
            }
            catch (FaultException e)
            {
                MessageBox.Show("Ismeretlen hiba: " + e.Reason.ToString());
            }
        }
    }
}
