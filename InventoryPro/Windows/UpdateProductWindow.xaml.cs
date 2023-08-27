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

namespace InventoryPro
{
    /// <summary>
    /// Interaction logic for UpdateProductWindow.xaml
    /// </summary>
    public partial class UpdateProductWindow : Window
    {
        private static Product product;
        public UpdateProductWindow(Product p)
        {
            InitializeComponent();
            product = p;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            nameText.Text = product.Name;
            priceText.Text = product.PricePerUnit.ToString();
            amountText.Text = product.Amount.ToString();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            MongoRepository mongoRepository = new MongoRepository();
            Product newProduct = new Product { Amount = int.Parse(amountText.Text), Name = nameText.Text, PricePerUnit = float.Parse(priceText.Text) };
            mongoRepository.UpdateProduct(newProduct, product);

            InventoryWindow inventoryWindow = new InventoryWindow();
            inventoryWindow.Show();
            this.Close();
        }

        private void returnButton_Click(object sender, RoutedEventArgs e)
        {
            InventoryWindow inventoryWindow = new InventoryWindow();
            inventoryWindow.Show();
            this.Close();
        }
    }
}
