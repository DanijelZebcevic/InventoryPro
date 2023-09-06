using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace InventoryPro.Windows
{
    /// <summary>
    /// Interaction logic for AddDeliveryWindow.xaml
    /// </summary>
    public partial class AddDeliveryWindow : Window
    {
        private List<Product> originalProducts = new List<Product>();
        private List<Product> selectedProducts = new List<Product>();
        public AddDeliveryWindow()
        {
            InitializeComponent();
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

            Delivery delivery = new Delivery();
            delivery.DeliveredItems = items;
            delivery.Deliverer = delivererText.Text;
            DateTime? selectedDate = deliveryDateText.SelectedDate;
            if (selectedDate.HasValue)
            {
                delivery.DeliveryDate = selectedDate.Value.Date;
            }


            ComboBoxItem selectedComboBoxItem = (ComboBoxItem)deliveredText.Items[deliveredText.SelectedIndex];
            string selectedContent = selectedComboBoxItem.Content.ToString();

            if (selectedContent == "Dostavljeno")
            {
                delivery.OrderIsDelivered = true;
            }
            else
            {
                delivery.OrderIsDelivered = false;
            }
            if (delivery.DeliveredItems.Count < 1)
            {
                MessageBox.Show("Mora biti unesen barem 1 proizvod u dostavu!");
            }
            else
            {
                mongoRepository.AddDelivery(delivery);
                DeliveryWindow deliveryWindow = new DeliveryWindow();
                deliveryWindow.Show();
                this.Close();
            }
            
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            DeliveryWindow deliveryWindow = new DeliveryWindow();
            deliveryWindow.Show();
            this.Close();
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
    }
}
