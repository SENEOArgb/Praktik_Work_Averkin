using Praktik_Work_2.Helper;
using Praktik_Work_2.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Praktik_Work_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var (login, password) = UserSettings.LoadUser();
            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
            {
                if (DataContext is AutorizeViewModel viewModel)
                {
                    viewModel.Login = login;
                    viewModel.Password = password;

                    if (viewModel.CanLogIn())
                    {
                        viewModel.LogIn();
                    }
                }
            }
        }

        private void pasBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is AutorizeViewModel viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }


    }
}