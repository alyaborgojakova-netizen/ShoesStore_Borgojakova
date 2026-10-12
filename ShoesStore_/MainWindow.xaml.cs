using ShoesStore_.AppConnect;
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

namespace ShoesStore_
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

        private void GuestVhod_Click(object sender, RoutedEventArgs e)
        {
            MainMenuWindow guestwin = new MainMenuWindow();
            guestwin.Show();
            this.Close();
        }
        //public string FIO
        //{
        //    get
        //    {
        //        return LastName + " " + FirstName + " " + SecondName;
        //    }
        //}
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userObj = Shoes2026ABEntities4.GetContext().Users.FirstOrDefault(x => x.Login == LoginBox.Text && x.Password == PasswordBox.Password);
                if (userObj == null)
                {
                    MessageBox.Show("Такого пользователя нет!", "Ошибка при авторизации!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {

                    switch (userObj.Id_Role)
                    {
                        case 1:
                            MessageBox.Show("Здравствуйте,Администратор " + userObj.FIO + "!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            AdminWindow win1 = new AdminWindow(userObj);
                            win1.Show();
                            this.Close();
                            break;
                        case 2:
                            MessageBox.Show("Здравствуйте,Менеджер " + userObj.FIO + "!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            ManagerWindow win3 = new ManagerWindow(userObj);
                            win3.Show();
                            this.Close();
                            break;
                        case 3:
                            MessageBox.Show("Здравствуйте,Клиент " + userObj.FIO + "!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            ClientWindow win2 = new ClientWindow(userObj);
                            win2.Show();
                            this.Close();
                            break;

                        default:
                            MessageBox.Show("Данные не обнаружены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex.Message.ToString());
            }
        }
    }
}
