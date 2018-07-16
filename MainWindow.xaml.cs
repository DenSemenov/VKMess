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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VKMess
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            
        }

        void auth()
        {
            browser.Navigate("https://oauth.vk.com/authorize?client_id=6634607&display=popup&scope=messages&response_type=token&v=5.80");
            browser.LoadCompleted += Browser_LoadCompleted;
        }

        private void Browser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            string url = browser.Source.AbsoluteUri;
            string user = url.Split('#')[1];

            Properties.Settings.Default.token = user.Split('&')[0].Replace("access_token=","");
            Properties.Settings.Default.userID = user.Split('&')[2].Replace("user_id=", "");

            webGrid.Visibility = Visibility.Hidden;
            login.Visibility = Visibility.Hidden;

            messagesFrame.Visibility = Visibility.Visible;
            messagesFrame.Content = new Messages();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            webGrid.Visibility = Visibility.Visible;
            auth();
        }
    }
}
