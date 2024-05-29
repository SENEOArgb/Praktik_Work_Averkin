using Praktik_Work_2.Helper;
using Praktik_Work_2.ViewModels;
using Praktik_Work_2.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Praktik_Work_2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var (login, password) = UserSettings.LoadUser();
            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
            {
                var authViewModel = new AutorizeViewModel
                {
                    Login = login,
                    Password = password
                };

                if (authViewModel.CanLogIn())
                {
                    // Запуск окна OfficerWindow или CommanderWindow напрямую
                    if (login == "kalashnikovOfficer" && password == "kalashnikov848")
                    {
                        var officerWindow = new OfficerWindow();
                        officerWindow.Show();
                    }
                    else if (login == "agutinCommander" && password == "agutinn1")
                    {
                        var commanderWindow = new CommanderWindow();
                        commanderWindow.Show();
                    }

                    return; // Предотвращение открытия MainWindow
                }
            }
        }
    }
}