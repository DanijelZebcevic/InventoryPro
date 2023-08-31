using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace InventoryPro.Windows
{
    /// <summary>
    /// Interaction logic for UpdateDeliveryWindow.xaml
    /// </summary>
    public partial class UpdateDeliveryWindow : Window
    {
        private List<Product> originalProducts = new List<Product>();
        private List<Product> selectedProducts = new List<Product>();
        private Delivery passedDelivery;
        public UpdateDeliveryWindow(Delivery delivery)
        {
            passedDelivery = delivery;
            InitializeComponent();
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

                dataGrid.Columns[0].Visibility = Visibility.Collapsed;
                dataGrid.Columns[2].Visibility = Visibility.Collapsed;
                dataGrid.Columns[3].Visibility = Visibility.Collapsed;
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

                dataGrid.Columns[0].Visibility = Visibility.Collapsed;
                dataGrid.Columns[2].Visibility = Visibility.Collapsed;
                dataGrid.Columns[3].Visibility = Visibility.Collapsed;
                dataGrid2.Columns[1].Visibility = Visibility.Collapsed;
                dataGrid2.Columns[3].Visibility = Visibility.Collapsed;
                dataGrid2.Columns[4].Visibility = Visibility.Collapsed;

            }
        }

        private async void editButton_Click(object sender, RoutedEventArgs e)
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

            mongoRepository.UpdateDelivery(passedDelivery, delivery);
            DeliveryWindow deliveryWindow = new DeliveryWindow();
            deliveryWindow.Show();
            this.Close();

        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            DeliveryWindow deliveryWindow = new DeliveryWindow();
            deliveryWindow.Show();
            this.Close();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            delivererText.Text = passedDelivery.Deliverer;
            deliveryDateText.SelectedDate = passedDelivery.DeliveryDate;

            for (int i = 0; i < passedDelivery.DeliveredItems.Count; i++)
            {
                var prod = passedDelivery.DeliveredItems[i].Product;
                prod.Amount = passedDelivery.DeliveredItems[i].AmountBought;
                selectedProducts.Add(prod);
            }

            if (passedDelivery.OrderIsDelivered == true)
            {
                deliveredText.SelectedIndex = 0;
            }
            else
            {
                deliveredText.SelectedIndex = 1;
            }
            MongoRepository mongoRepository = new MongoRepository();
            originalProducts = await mongoRepository.GetProductsNotInList(selectedProducts);
            dataGrid.ItemsSource = originalProducts;
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
}
