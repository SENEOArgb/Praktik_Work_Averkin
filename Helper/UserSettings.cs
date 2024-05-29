using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praktik_Work_2.Helper
{
    public static class UserSettings
    {
        public static void SaveUser(string login, string password)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings["UserLogin"] == null)
            {
                config.AppSettings.Settings.Add("UserLogin", login);
            }
            else
            {
                config.AppSettings.Settings["UserLogin"].Value = login;
            }

            if (config.AppSettings.Settings["UserPassword"] == null)
            {
                config.AppSettings.Settings.Add("UserPassword", password);
            }
            else
            {
                config.AppSettings.Settings["UserPassword"].Value = password;
            }

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public static (string Login, string Password) LoadUser()
        {
            string login = ConfigurationManager.AppSettings["UserLogin"];
            string password = ConfigurationManager.AppSettings["UserPassword"];
            return (login, password);
        }

        public static void ClearUser()
        {
            SaveUser(string.Empty, string.Empty);
        }
    }
}
