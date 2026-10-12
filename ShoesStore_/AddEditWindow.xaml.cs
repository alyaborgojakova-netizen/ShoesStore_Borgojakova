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
using System.IO;
using Microsoft.Win32;
using System.Data.Entity.Infrastructure;

namespace ShoesStore_
{
    /// <summary>
    /// Логика взаимодействия для AddEditWindow.xaml
    /// </summary>
    public partial class AddEditWindow : Window
    {
        private Products _currentproduct = new Products();
        private string _photoPath;
        public AddEditWindow(Products selectedproduct)
        {
            InitializeComponent();
            if (selectedproduct != null)
            {
                _currentproduct = selectedproduct;
                IdProductPanel.Visibility = Visibility.Visible;
            }
            DataContext = _currentproduct;
            CmbCategory.ItemsSource = Shoes2026ABEntities4.GetContext().Categories.ToList();
            CmbSupplier.ItemsSource = Shoes2026ABEntities4.GetContext().Suppliers.ToList();
            CmbManufacturer.ItemsSource = Shoes2026ABEntities4.GetContext().Manufacturer.ToList();
            CmbProduct.ItemsSource = Shoes2026ABEntities4.GetContext().ProductTypes.ToList();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(TxbPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Введите корректную цену (неотрицательную)");
                return;
            }
            _currentproduct.Price = price;

            if (!int.TryParse(TxbQuantity.Text, out int qty) || qty < 0)
            {
                MessageBox.Show("Введите корректное количество (неотрицательное)");
                return;
            }
            _currentproduct.QuantityInStock = qty;

            var ctx = Shoes2026ABEntities4.GetContext();

            if (_currentproduct.Id == 0)
            {
                if (string.IsNullOrWhiteSpace(_currentproduct.Article))
                {
                    _currentproduct.Article = "ART" + DateTime.Now.Ticks.ToString().Substring(8);
                }
                ctx.Products.Add(_currentproduct);
            }

            try
            {
                ctx.SaveChanges();
                MessageBox.Show("Данные сохранены!");
                DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ChooseImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string targetDirectory = @"E:\wpf 3 курс\ShoesStore_\ShoesStore_\Photos";
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.DefaultExt = ".png";
                dialog.Filter = "Image Files|*.jpg; *.jpeg;*.png;*.bmp;*.gif";
                if(dialog.ShowDialog() == true)
                {
                    string sourceFilePath = dialog.FileName;
                    _photoPath = dialog.SafeFileName.ToString();
                    TxbPhotoPath.Text = _photoPath;

                    string newPath = Path.Combine(targetDirectory, _photoPath);
                    File.Copy(sourceFilePath, newPath, overwrite: true);
                    _currentproduct.Photo = "Photos\\" + _photoPath;
                    SourcePhoto.Source = new BitmapImage(new Uri(System.IO.Path.Combine(targetDirectory, _photoPath), UriKind.Absolute));
                    Shoes2026ABEntities4.GetContext().SaveChanges();
                    MessageBox.Show("Изображение успешно установлено!");
                }
            
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
