using ShoesStore_.AppConnect;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace ShoesStore_
{
    /// <summary>
    /// Логика взаимодействия для AddEditOrderWindow.xaml
    /// </summary>
    public partial class AddEditOrderWindow : Window
    {
        //public string Address
        //{
        //    get
        //    {
        //        return City + " " + " " + Street + " " + Number;
        //    }
        //}
        Orders currentorder = new Orders();
        public AddEditOrderWindow(Orders selectedorder)
        {
            InitializeComponent();
            if (selectedorder != null)
            {
                currentorder = selectedorder;
                CmbOrderer.Visibility = Visibility.Collapsed;
                OrderedPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                currentorder = new Orders();
                currentorder.OrderDate = DateTime.Today;
            }
            DataContext = currentorder;
            CmbProduct.ItemsSource = Shoes2026ABEntities4.GetContext().Products.ToList();
            CmbStatus.ItemsSource = Shoes2026ABEntities4.GetContext().OrderStatus.ToList();
            CmbPVZ.ItemsSource = Shoes2026ABEntities4.GetContext().PVZ.ToList();
            CmbOrderer.ItemsSource = Shoes2026ABEntities4.GetContext().Users.Where(x => x.Id_Role == 3).ToList();
        }
        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            if (currentorder.DeliveryDate < currentorder.OrderDate)
            {
                MessageBox.Show("Дата выдачи не может быть раньше даты заказа");
                return;
            }
            if (currentorder.Id == 0)
            {
                currentorder.Code = int.Parse(DateTime.Now.ToString("MMddHHmmss"));
                Shoes2026ABEntities4.GetContext().Orders.Add(currentorder);
                if (currentorder.Id == 0)
                {
                    currentorder.Code = int.Parse(DateTime.Now.ToString("MMddHHmmss"));
                    Shoes2026ABEntities4.GetContext().Orders.Add(currentorder);

                    Shoes2026ABEntities4.GetContext().SaveChanges(); 
                    var item = new OrderItems
                    {
                        Id_Order = currentorder.Id,
                        Amount = 1
                    };
                    Shoes2026ABEntities4.GetContext().OrderItems.Add(item);
                    Shoes2026ABEntities4.GetContext().SaveChanges();
                }
            }
            try
            {
                Shoes2026ABEntities4.GetContext().SaveChanges();
                MessageBox.Show("Заказ добавлен!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
