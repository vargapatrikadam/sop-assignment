using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BlogApp.View
{
    /// <summary>
    /// Interaction logic for MainContainerWindow.xaml
    /// </summary>
    public partial class MainContainerWindow : Window
    {
        Style Primary = Application.Current.FindResource("MaterialDesignRaisedButton") as Style;
        Style Secondary = Application.Current.FindResource("MaterialDesignRaisedDarkButton") as Style;
        //Kicseréli az aktív tab stílusát Primary stílusra, a másik kettőt Secondaryra
        public void ChangeTab(Button active, Button inactive)
        {
            active.Style = Primary;
            active.FontWeight = FontWeights.Heavy;
            inactive.Style = Secondary;
            inactive.FontWeight = FontWeights.Normal;
        }
        public MainContainerWindow()
        {
            InitializeComponent();
            Main.Content = new AllPostsPage();
            ChangeTab(AllPosts, MyPosts);
        }
        private void MyPosts_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new MyPostsPage();
            ChangeTab(MyPosts, AllPosts);
        }

        private void AllPosts_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new AllPostsPage(); 
            ChangeTab(AllPosts, MyPosts);
        }
    }
}
