using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Contexts;
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
using Chat.DataFolder;

namespace Chat
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ChatListBox.ItemsSource = DBEntities.GetContext()
              .Messages.ToList().OrderBy(u => u.IdMessages);

            //InitializeUser();
            //LoadMessages();
        }
        //private void InitializeUser()
        //{
        //    // Заглушка для демонстрации. Замените на механизм аутентификации.
        //    using (var context = new DBEntities())
        //    {
        //        user = context.User.FirstOrDefault(u => u.UserName == "Alice");
        //        if (user == null)
        //        {
        //            user = new User { UserName = "Alice" };
        //            context.User.Add(user);
        //            DBEntities.GetContext().SaveChanges();
        //        }
        //    }
        //}
        //private void LoadMessages()
        //{
        //    using (var context = new DBEntities())
        //    {
        //        var messages = context.Messages.Include(m => m.User).ToList();
        //        foreach (var message in messages)
        //        {
        //            ChatListBox.Items.Add($"{message.User.UserName}: {message.MesContent}");
        //        }
        //    }
        //}
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string messageText = MessageTextBox.Text;
            if (!string.IsNullOrWhiteSpace(messageText))
            {
                DBEntities.GetContext().Messages.Add(new Messages()
                {
                    MesContent = MessageTextBox.Text
                });
                DBEntities.GetContext().SaveChanges();
                ChatListBox.ItemsSource = DBEntities.GetContext()
              .Messages.ToList().OrderBy(u => u.IdMessages);
                //ChatListBox.ItemsSource = DBEntities.GetContext().
                //    Messages.ToList();
                MessageTextBox.Clear();
            }
        }
    }
}

