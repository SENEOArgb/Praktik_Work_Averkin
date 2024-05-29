using Praktik_Work_2.Helper;
using Praktik_Work_2.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace Praktik_Work_2.ViewModels
{
    public class AutorizeViewModel : ViewModelBase
    {
        private string _login;
        private string _password;

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand ExitCommand { get; }

        public AutorizeViewModel()
        {
            LoginCommand = new RelayCommand(LogIn);
            ExitCommand = new RelayCommand(Exit);
        }

        public bool CanLogIn()
        {
            string loginOfficer = "kalashnikovOfficer";
            string passwordOfficer = "kalashnikov848";

            string loginCommander = "agutinCommander";
            string passwordCommander = "agutinn1";

            return (Login == loginOfficer && Password == passwordOfficer) ||
                   (Login == loginCommander && Password == passwordCommander);
        }

        public void LogIn()
        {
            if (CanLogIn())
            {
                Window newWindow = null;

                if (Login == "kalashnikovOfficer" && Password == "kalashnikov848")
                {
                    UserSettings.SaveUser(Login, Password);
                    newWindow = Application.Current.Windows.OfType<OfficerWindow>().FirstOrDefault() ?? new OfficerWindow();
                }
                else if (Login == "agutinCommander" && Password == "agutinn1")
                {
                    UserSettings.SaveUser(Login, Password);
                    newWindow = Application.Current.Windows.OfType<CommanderWindow>().FirstOrDefault() ?? new CommanderWindow();
                }

                var authWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                authWindow?.Close();

                newWindow?.Show();
            }
            else
            {
                MessageBox.Show("Неправильный логин или пароль!");
            }
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }
    }
}
