using Praktik_Work_2.Helper;
using Praktik_Work_2.Views.HelpView;
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

namespace Praktik_Work_2.Views
{
    /// <summary>
    /// Логика взаимодействия для CommanderWindow.xaml
    /// </summary>
    public partial class CommanderWindow : Window
    {
        public CommanderWindow()
        {
            InitializeComponent();

            this.PreviewKeyDown += MainWindow_PreviewKeyDown;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            UserSettings.ClearUser();

            var loginWindow = new MainWindow();
            loginWindow.Show();

            this.Close();
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                WindowHelp winHelp = new WindowHelp();
                winHelp.Show();
            }
        }
    }
}
