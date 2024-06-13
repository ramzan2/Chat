using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Chat.DataFolder
{
    public static class SettingsManager
    {
        public static string RememberMeToken
        {
            get => ConfigurationManager.AppSettings["RememberMeToken"];
            set => SaveSetting("RememberMeToken", value);
        }

        public static string SavedLogin
        {
            get => ConfigurationManager.AppSettings["SavedLogin"];
            set => SaveSetting("SavedLogin", value);
        }

        public static string SavedPassword
        {
            get => ConfigurationManager.AppSettings["SavedPassword"];
            set => SaveSetting("SavedPassword", value);
        }

        public static bool IsRememberMeChecked
        {
            get => bool.Parse(ConfigurationManager.AppSettings["IsRememberMeChecked"] ?? "false");
            set => SaveSetting("IsRememberMeChecked", value.ToString());
        }

        private static void SaveSetting(string key, string value)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove(key);
            config.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
