using MongoDB.Bson.Serialization.Serializers;
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

namespace InventoryPro.Windows
{
    /// <summary>
    /// Interaction logic for AddOrderWindow.xaml
    /// </summary>
    public partial class AddOrderWindow : Window
    {
        private List<Product> originalProducts = new List<Product>();
        private List<Product> selectedProducts = new List<Product>();
        public AddOrderWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MongoRepository mongoRepository = new MongoRepository();
            originalProducts = await mongoRepository.GetProducts();
            foreach (var item in originalProducts)
            {
                item.Amount = 0;
            }
            dataGrid.ItemsSource = originalProducts;
            dataGrid.Columns[2].Visibility = Visibility.Collapsed;
            dataGrid.Columns[3].Visibility = Visibility.Collapsed;
            dataGrid.Columns[4].Visibility = Visibility.Collapsed;
            dataGrid.Columns[5].Visibility = Visibility.Collapsed;

        }

        private void toRightButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dataGrid.SelectedItem as Product;
            if (selectedItem is not null)
            {
                originalProducts.Remove(selectedItem);
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = originalProducts;
                selectedProducts.Add(selectedItem);
                dataGrid2.ItemsSource = null;
                dataGrid2.ItemsSource = selectedProducts;

                dataGrid.Columns[2].Visibility = Visibility.Collapsed;
                dataGrid.Columns[3].Visibility = Visibility.Collapsed;
                dataGrid.Columns[4].Visibility = Visibility.Collapsed;
                dataGrid.Columns[5].Visibility = Visibility.Collapsed;

                dataGrid2.Columns[1].Visibility = Visibility.Collapsed;
                dataGrid2.Columns[3].Visibility = Visibility.Collapsed;
                dataGrid2.Columns[4].Visibility = Visibility.Collapsed;
            }
        }

        private void toLeftButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dataGrid2.SelectedItem as Product;
            if (selectedItem is not null)
            {
                selectedProducts.Remove(selectedItem);
                dataGrid2.ItemsSource = null;
                dataGrid2.ItemsSource = selectedProducts;
                originalProducts.Add(selectedItem);
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = originalProducts;

                dataGrid.Columns[2].Visibility = Visibility.Collapsed;
                dataGrid.Columns[3].Visibility = Visibility.Collapsed;
                dataGrid.Columns[4].Visibility = Visibility.Collapsed;
                dataGrid.Columns[5].Visibility = Visibility.Collapsed;

                dataGrid2.Columns[1].Visibility = Visibility.Collapsed;
                dataGrid2.Columns[3].Visibility = Visibility.Collapsed;
                dataGrid2.Columns[4].Visibility = Visibility.Collapsed;

            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new OrderWindow();
            orderWindow.Show();
            this.Close();
        }

        private async void addButton_Click(object sender, RoutedEventArgs e)
        {
            MongoRepository mongoRepository = new MongoRepository();
            List<Product> oldProducts = await mongoRepository.GetProducts();
            List<Items> items = new List<Items>();
            foreach (var product in dataGrid2.Items)
            {
                if (product is Product helpProduct)
                {
                    foreach (var item in oldProducts)
                    {
                        if (item.Id == helpProduct.Id)
                        {
                            items.Add(new Items { AmountBought = helpProduct.Amount, Product = item, Sum = helpProduct.Amount * item.PricePerUnit });
                            break;
                        }
                    }
                }

            }

            Order order = new Order();
            order.OrderedItems = items;
            order.Customer = customerText.Text;
            DateTime? selectedDate = orderDateText.SelectedDate;
            if (selectedDate.HasValue)
            {
                order.DeliveryDate = selectedDate.Value.Date;
            }
            
                            
            ComboBoxItem selectedComboBoxItem = (ComboBoxItem)delivered.Items[delivered.SelectedIndex];
            string selectedContent = selectedComboBoxItem.Content.ToString();

            if (selectedContent == "Dostavljeno")
            {
                order.OrderIsDelivered = true;
            }
            else
            {
                order.OrderIsDelivered = false;
            }

            mongoRepository.AddOrder(order);
            OrderWindow orderWindow = new OrderWindow();
            orderWindow.Show();
            this.Close();

        }
    }
}
