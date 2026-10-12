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
using System.Windows.Shapes;

namespace ShoesStore_
{
    /// <summary>
    /// Логика взаимодействия для ClientWindow.xaml
    /// </summary>

    public partial class ClientWindow : Window
    {
        public ClientWindow( Users user)
        {
            InitializeComponent();
            fullname.Text = user.FIO;
            ListTovar.ItemsSource = Shoes2026ABEntities4.GetContext().Products.ToList();
        }

        private void BackToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = new MainWindow();
            win.Show();
            this.Close();
        }
    }
}
