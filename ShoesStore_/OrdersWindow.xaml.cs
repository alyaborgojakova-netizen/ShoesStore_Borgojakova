using ShoesStore_.AppConnect;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ShoesStore_
{
    /// <summary>
    /// Логика взаимодействия для OrdersWindow.xaml
    /// </summary>
    public partial class OrdersWindow : Window
    {
        private Users _currentUser;
        private bool _readOnly;
        DispatcherTimer timer = new DispatcherTimer();
        public void UpdateData(object sender, EventArgs e)
        {
            ListOrder.ItemsSource = Shoes2026ABEntities4.GetContext().Orders.ToList();
        }
        public OrdersWindow(Users currentUser, bool readOnly = false)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _readOnly = readOnly;

            ListOrder.ItemsSource = Shoes2026ABEntities4.GetContext().Orders.ToList();
            if (_readOnly)
            {
                ButtonPanel.Visibility = Visibility.Collapsed;
                ListOrder.MouseDoubleClick -= ListOrder_MouseDoubleClick;
                this.Title = "Просмотр заказов";
            }

            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += UpdateData;
            timer.Start();

        }

        private void BackTo_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            var ord = new AddEditOrderWindow(null);
            ord.ShowDialog();
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            var selected = ListOrder.SelectedItems.Cast<Orders>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить элементы ({selected.Count})?","Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (var item in selected)
                    {
                        var relatedItems = Shoes2026ABEntities4.GetContext().OrderItems.Where(oi => oi.Id_Order == item.Id).ToList();

                        foreach (var relatedItem in relatedItems)
                        {
                            Shoes2026ABEntities4.GetContext().OrderItems.Remove(relatedItem);
                        }

                        Shoes2026ABEntities4.GetContext().Orders.Remove(item);
                    }

                    Shoes2026ABEntities4.GetContext().SaveChanges();

                    MessageBox.Show("Данные удалены!");
                    ListOrder.ItemsSource = Shoes2026ABEntities4.GetContext().Orders.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void ListOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var ord = new AddEditOrderWindow((sender as ListView).SelectedItem as Orders);
            ord.ShowDialog();
        }
    }
}
