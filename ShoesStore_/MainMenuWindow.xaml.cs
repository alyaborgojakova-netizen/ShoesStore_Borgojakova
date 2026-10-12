using ShoesStore_.AppConnect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для MainMenuWindow.xaml
    /// </summary>
    public partial class MainMenuWindow : Window
    {
        public MainMenuWindow()
        {
            InitializeComponent();
            //public bool is_discount => Sale > 15;
            //public bool is_on_sale => Sale > 0;
            //public decimal? price_discount
            //{
            //    get
            //    {
            //        return Price - (Price * Sale / 100);
            //    }
            //}
            //public bool is_quantity => QuantityInStock <= 0;
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
