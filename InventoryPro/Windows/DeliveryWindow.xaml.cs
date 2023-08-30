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
    /// Interaction logic for DeliveryWindow.xaml
    /// </summary>
    public partial class DeliveryWindow : Window
    {
        public DeliveryWindow()
        {
            InitializeComponent();
        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow homeWindow = new HomeWindow();
            homeWindow.Show();
            this.Close();
        }

        private void inventoryButton_Click(object sender, RoutedEventArgs e)
        {
            InventoryWindow inventoryWindow = new InventoryWindow();
            inventoryWindow.Show();
            this.Close();
        }

        private void billButton_Click(object sender, RoutedEventArgs e)
        {
            BillsWindow billsWindow = new BillsWindow();
            billsWindow.Show();
            this.Close();
        }

        private void orderButton_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new OrderWindow();
            orderWindow.Show();
            this.Close();
        }

        private void contactButton_Click(object sender, RoutedEventArgs e)
        {
            ContactsWindow contactWindow = new ContactsWindow();
            contactWindow.Show();
            this.Close();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddDeliveryWindow addDeliveryWindow = new AddDeliveryWindow();
            addDeliveryWindow.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }
        private async void RefreshData()
        {
            MongoRepository mongoRepository = new MongoRepository();
            dataGrid.ItemsSource = await mongoRepository.GetDeliveries();
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = dataGrid.SelectedItem as Delivery;
            if (selectedItem != null)
            {
                UpdateDeliveryWindow updateDeliveryWindow = new UpdateDeliveryWindow(selectedItem);
                updateDeliveryWindow.Show();
                this.Close();
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            MongoRepository mongoRepository = new MongoRepository();
            var selectedDelivery = dataGrid.SelectedItem as Delivery;
            if (selectedDelivery != null)
            {
                mongoRepository.DeleteDelivery(selectedDelivery);
                RefreshData();
            }
        }
    }
}
