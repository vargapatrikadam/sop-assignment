using BlogApp.BlogService;
using BlogApp.BusinessLogic;
using BlogApp.Command;
using BlogApp.UI;
using BlogApp.View;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BlogApp.ViewModel
{
    class LoginViewModel : ObservableObject
    {
        string _UserName;
        [Required(ErrorMessage = "Felhasználónév kötelező!")]
        public string UserName
        {
            get { return _UserName; }
            set
            {
                _UserName = value;
                ValidateProperty("UserName", value);
            }
        }
        string _Password;
        [Required(ErrorMessage = "Jelszó kötelező!")]
        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                ValidateProperty("Password", value);
            }
        }
       
        public void UserLogin(string username, string password)
        {
             if (UserLoginMethod(username, password))
             {
                 new MainContainerWindow().Show();
                 Application.Current.Windows[0].Close();
             }
        }
        public bool UserLoginMethod(string username, string password)
        {
            Validate();
            if (IsValid)
            {
                try
                {
                    UIRepository.Instance.Client = UIRepository.Instance.LocalClient.UserLogin(username, password);
                    return true;
                }
                catch (FaultException<UserNotFoundFault> e)
                {
                    MessageBox.Show(e.Reason.ToString());
                }
                catch (FaultException<NotCorrectPasswordFault> e)
                {
                    MessageBox.Show(e.Reason.ToString());
                }
                catch (FaultException e)
                {
                    MessageBox.Show("Ismeretlen hiba: " + e.Reason.ToString());
                }
                catch (EndpointNotFoundException)
                {
                    MessageBox.Show("Hiba a szolgáltatóval!");
                    return false;
                }
            }
            return false;
        }
        private ICommand _Login;
        public ICommand Login
        {
            get
            {
                if (_Login == null)
                {
                    _Login = new RelayCommand(p => true, p => this.UserLogin(UserName, Password));
                }
                return _Login;
            }
        }
    }
}
