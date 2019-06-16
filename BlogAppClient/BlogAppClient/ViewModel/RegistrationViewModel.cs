using BlogApp.BusinessLogic;
using BlogApp.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BlogApp.BlogService;
using BlogApp.UI;

namespace BlogApp.ViewModel
{
    class RegistrationViewModel : ObservableObject
    {
        private string _FirstName;
        [Required(ErrorMessage = "Keresztnév kötelező!")] 
        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                _FirstName = value;
                ValidateProperty("FirstName", value);
            }
        }
        private string _LastName;
        [Required(ErrorMessage = "Vezetéknév kötelező!")]
        public string LastName
        {
            get { return _LastName; }
            set
            {
                _LastName = value;
                ValidateProperty("LastName", value);
            }
        }
        private string _UserName;
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
        private string _userPassword;
        [Required(ErrorMessage = "Jelszó kötelező!")]
        public string UserPassword
        {
            get { return _userPassword; }
            set
            {
                _userPassword = value;
                ValidateProperty("UserPassword", value);
            }
        }
        private string _retryPassword;
        [Required(ErrorMessage = "Kérem erősítse meg jelszavát!")]
        public string RetryPassword
        {
            get { return _retryPassword; }
            set
            {
                _retryPassword = value;
                ValidateProperty("RetryPassword", value);
            }
        }
        private ICommand _doRegistration;
        public ICommand DoRegistration
        {
            get
            {
                if (_doRegistration == null)
                {
                    _doRegistration = new RelayCommand(p => true, p => RegisterUser(UserName, UserPassword, RetryPassword, FirstName, LastName));
                }
                return _doRegistration;
            }
        }
        public void RegisterUser(string username, string password, string RetryPassword, string FirstName, string LastName)
        {
            Validate();
            if (IsValid)
            {
                try
                {
                    UIRepository.Instance.LocalClient.RegisterUser(username, password, RetryPassword, FirstName, LastName);
                    MessageBox.Show("Sikeres regisztráció!");
                }
                catch (FaultException<UserAlreadyExistsFault> e)
                {
                    MessageBox.Show(e.Reason.ToString());
                }
                catch (FaultException<PasswordsNotEqualFault> e)
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
                }
            }            
        }
    }
}
