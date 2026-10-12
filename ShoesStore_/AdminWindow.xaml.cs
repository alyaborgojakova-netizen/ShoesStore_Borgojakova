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
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private int _sort = 0;
        private Users _currentUser;
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
        public AdminWindow(Users user)
        {
            InitializeComponent();
            SupplierFilter.ItemsSource = Shoes2026ABEntities4.GetContext().Suppliers.ToList();
            fullname.Text = user.FIO;
            _currentUser = user;
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

        private void SupplierFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            Filtr();
        }

        private void ResetFiltersBtn_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = "";
            SupplierFilter.SelectedIndex = 0;
            _sort = 0;
            Filtr();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            AddEditWindow win = new AddEditWindow(null);
            win.ShowDialog();
        }

        private void ListTovar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddEditWindow win = new AddEditWindow((sender as ListView).SelectedItem as Products);
            win.ShowDialog();
        }
        //public static ShoesAB2026Entities3 GetContext()
        //{
        //    if (_context == null)
        //        _context = new ShoesAB2026Entities3();
        //    return _context;
        //}
        private void DeleteItems_Click(object sender, RoutedEventArgs e)
        {
            var selected = ListTovar.SelectedItems.Cast<Products>().ToList();

            var inOrders = selected.Where(p => Shoes2026ABEntities4.GetContext().Orders.Any(o => o.Id_Product == p.Id)) .ToList();
            if (inOrders.Count > 0)
            {
                var list = string.Join("\n", inOrders.Select(p => p.Article ?? p.Id.ToString()));
                MessageBox.Show("Нельзя удалить товары, которые присутствуют в заказах:\n\n" + list,"Удаление отклонено", MessageBoxButton.OK,MessageBoxImage.Warning);
                return; 
            }

            if (MessageBox.Show($"Вы точно хотите удалить элементы ({selected.Count})?",
                                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (var item in selected)
                    {
                        Shoes2026ABEntities4.GetContext().Products.Remove(item);
                    }
                    Shoes2026ABEntities4.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    ListTovar.ItemsSource = Shoes2026ABEntities4.GetContext().Products.ToList();
                }
                catch(Exception ex) 
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }

        }

        private void OrdersBtn_Click(object sender, RoutedEventArgs e)
        {
            var win = new OrdersWindow(_currentUser,readOnly: false);
            win.ShowDialog();
        }
    }
}
