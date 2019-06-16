using BlogApp.BlogService;
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
using System.Windows.Controls;
using System.Windows.Input;

namespace BlogApp.ViewModel
{
    class LoginRegContainerViewModel : ObservableObject
    {
        //Kapcsolatlétesítés a WCF Serviceel
        public LoginRegContainerViewModel()
        {
            try
            {
                UIRepository.Instance.LocalClient = new BlogAppServiceClient();
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show("Hiba a szolgáltatóval!");
            }
        }
    }
}
