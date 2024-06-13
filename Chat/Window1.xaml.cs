using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using Chat.DataFolder;

namespace Chat
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            LoadSavedCredentials();
        }
        private void LoadSavedCredentials()
        {
            if (SettingsManager.IsRememberMeChecked)
            {
                LoginTb.Text = SettingsManager.SavedLogin;
                PasswordTb.Password = SettingsManager.SavedPassword;
                checkbox.IsChecked = true;
            }
        }
        //
        public string GenerateToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
        //
        public void SaveRememberMeToken(int userId, string token)
        {
            using (var context = new DBEntities())
            {
                var rememberMeToken = new RemembersTokken
                {
                    IdUser = userId,
                    Tokken = token,
                    ExpiryDate = DateTime.Now.AddDays(30) // Токен действителен 30 дней
                };
                context.RemembersTokken.Add(rememberMeToken);
                context.SaveChanges();
            }
        }
        //
        private void SetRememberMeToken(string token)
        {
            Properties.Settings.Default.RememberMeToken = token;
            Properties.Settings.Default.Save();
        }
        //
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginTb.Text))
            {
                MBClass.ErrorMB("Введите логин");
                LoginTb.Focus();
            }
            else if (string.IsNullOrWhiteSpace(PasswordTb.Password))
            {
                MBClass.ErrorMB("Введите пароль");
                PasswordTb.Focus();
                return;
            }
            else if (!string.IsNullOrWhiteSpace(LoginTb.Text) && !string.IsNullOrWhiteSpace(PasswordTb.Password))
            {
                try
                {
                    var user = DBEntities.GetContext()
                        .User
                        .FirstOrDefault(u => u.UserName == LoginTb.Text);


                    // Сохранение токена "Запомнить меня"
                    if (checkbox.IsChecked == true)
                    {
                        var token = GenerateToken();
                        SaveRememberMeToken(user.IdUser, token);
                        SetRememberMeToken(token);

                        SettingsManager.SavedLogin = LoginTb.Text;
                        SettingsManager.SavedPassword = PasswordTb.Password;
                        SettingsManager.IsRememberMeChecked = true;
                    }
                    else
                    {
                        // Очистка сохраненных данных
                        SettingsManager.SavedLogin = string.Empty;
                        SettingsManager.SavedPassword = string.Empty;
                        SettingsManager.IsRememberMeChecked = false;
                    }
                    // Переход к соответствующему меню в зависимости от роли пользователя
                    switch (user.IdRole)
                    {
                        case 1:
                            new MainWindow().Show();
                            Close();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MBClass.ErrorMB (ex.Message);
                }
            }
        }
        //private void SetRememberMeToken(string token)
        //{
        //    SettingsManager.RememberMeToken = token;
        //}

        //private string GetRememberMeToken()
        //{
        //    return SettingsManager.RememberMeToken;
        //}
    }
}
