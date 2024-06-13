using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Chat.DataFolder;
using System.Data.Entity;

namespace Chat
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var token = GetRememberMeToken();

            if (!string.IsNullOrEmpty(token))
            {
                var user = ValidateRememberMeToken(token);

                if (user != null)
                {
                    // Переход к соответствующему меню в зависимости от роли пользователя
                    switch (user.IdRole)
                    {
                        case 1:
                            new MainWindow().Show();
                            break;
                    }
                    return;
                }
            }

            // Переход к экрану входа
            new Window1().Show();
        }
        private string GetRememberMeToken()
        {
            return SettingsManager.RememberMeToken;
        }

        private User ValidateRememberMeToken(string token)
        {
            using (var context = DBEntities.GetContext())
            {
                var rememberMeToken = context.RemembersTokken
                    .Include(r => r.User)
                    .FirstOrDefault(r => r.Tokken == token && r.ExpiryDate > DateTime.Now);

                return rememberMeToken?.User;
            }
        }
    }
}
