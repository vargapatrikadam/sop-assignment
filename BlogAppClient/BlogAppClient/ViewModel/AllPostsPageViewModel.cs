using BlogApp.BlogService;
using BlogApp.BusinessLogic;
using BlogApp.Command;
using BlogApp.UI;
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
    class AllPostsPageViewModel : ObservableObject
    {
        public AllPostsPageViewModel()
        {
            EventAggregator.OnMessageTransmitted += OnMessageReceived;
        }
        private void OnMessageReceived(string message)
        {
            
            if (message == "Posts changed" || message == "Posts updated")
            {
                try
                {
                    _AllPosts = UIRepository.Instance.LocalClient.GetAllPosts(UIRepository.Instance.Client);
                    NotifyPropertyChanged("AllPosts");
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
        private ICollection<BlogService.PostModel> _AllPosts;
        public ICollection<BlogService.PostModel> AllPosts
        {
            get
            {
                if (_AllPosts == null)
                {
                    try
                    {
                        _AllPosts = UIRepository.Instance.LocalClient.GetAllPosts(UIRepository.Instance.Client);
                        NotifyPropertyChanged("AllPosts");
                        return _AllPosts;
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
                else return _AllPosts; 
            }
        }
    }
}
