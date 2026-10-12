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
using System.Windows.Threading;

namespace ShoesStore_
{
    /// <summary>
    /// Логика взаимодействия для ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        private int _sort = 0;
        private void Filtr()
        {
            var SearchList = Shoes2026ABEntities4.GetContext().Products.ToList();
            if (SearchBox.Text != "")
            {
                SearchList = SearchList.Where(x => x.ProductTypes.Name.ToLower().Contains(SearchBox.Text.ToLower())).ToList();
            }
            if (SupplierFilter.SelectedIndex > 0)
            {
                int sup = Convert.ToInt32(SupplierFilter.SelectedValue);
                SearchList = SearchList.Where(x => x.Id_Supplier == sup).ToList();
            }
            if (_sort == 1)
            {
                SearchList = SearchList.OrderBy(x => x.Sale).ToList();
            }
            if (_sort == 2)
            {
                SearchList = SearchList.OrderByDescending(x => x.Sale).ToList();
            }
           
            ListTovar.ItemsSource = SearchList.ToList();
        }
        public void UpdateData(object sender, EventArgs e)
        {
            ListTovar.ItemsSource = Shoes2026ABEntities4.GetContext().Products.ToList();
        }
        public ManagerWindow(Users user)
        {
            InitializeComponent();
            fullname.Text = user.FIO;
            ListTovar.ItemsSource = Shoes2026ABEntities4.GetContext().Products.ToList();

            var sup = Shoes2026ABEntities4.GetContext().Suppliers.ToList();
            sup.Insert(0, new Suppliers { Id = 0, Name = "Все поставщики" });
            SupplierFilter.ItemsSource = sup;
            SupplierFilter.SelectedIndex = 0;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += UpdateData;
            timer.Start();
        }

        private void BackToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            var login = new MainWindow();
            login.Show();
            this.Close();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filtr();
        }

        private void SortAscBtn_Click(object sender, RoutedEventArgs e)
        {
            _sort = 1;
            Filtr();
        }

        private void SortDescBtn_Click(object sender, RoutedEventArgs e)
        {
            _sort = 2;
            Filtr();
        }

        private void ResetFiltersBtn_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = "";
            SupplierFilter.SelectedIndex = 0;
            _sort = 0;
            Filtr();
        }

        private void SupplierFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var sup = Convert.ToInt32(SupplierFilter.SelectedValue);
            Filtr();
        }

        private void OrdersBtn_Click(object sender, RoutedEventArgs e)
        {
            var ord = new OrdersWindow(null, readOnly: true);
            ord.ShowDialog();
        }
    }
}
