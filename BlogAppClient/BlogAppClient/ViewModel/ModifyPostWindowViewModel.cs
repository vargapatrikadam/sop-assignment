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
    class ModifyPostWindowViewModel : ObservableObject
    {
        public void SendMessage(string message)
        {
            EventAggregator.BroadCast(message);
        }
        private int GetPostId
        {
            get { return UIRepository.Instance.ModifyPost; }
        }
        private string _PostBody;
        public string PostBody
        {
            get
            {
                if (_PostBody == null)
                {
                    try
                    {
                        _PostBody = UIRepository.Instance.LocalClient.GetPostBody(GetPostId, UIRepository.Instance.Client);
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
                    catch (EndpointNotFoundException)
                    {
                        MainContainerWindowViewModel.UserLogout();
                        return null;
                    }
                }
                return _PostBody;                
            }
            set
            {
                _PostBody = value;
            }
        }
        public void modifyPost(int id, string body)
        {
            if (body != null && body.Length < 100)
            {
                try
                {
                    UIRepository.Instance.LocalClient.ModifyPost(id, PostBody, UIRepository.Instance.Client);
                    SendMessage("Posts updated");
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
                catch (EndpointNotFoundException)
                {
                    MainContainerWindowViewModel.UserLogout();
                }
            }
            else MessageBox.Show("A poszt nem lehet 100 karakternél hosszabb, és minimum 1 karakter!");
            
        }
        private ICommand _ModifyPost;
        public ICommand ModifyPost
        {
            get
            {
                if (_ModifyPost == null)
                {
                    _ModifyPost = new RelayCommand(p => true, p => modifyPost(GetPostId,PostBody));
                }
                return _ModifyPost;
            }
        }
    }
}
