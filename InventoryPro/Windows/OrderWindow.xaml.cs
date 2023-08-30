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
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        public OrderWindow()
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

        private void deliveryButton_Click(object sender, RoutedEventArgs e)
        {
            DeliveryWindow deliveryWindow = new DeliveryWindow();
            deliveryWindow.Show();
            this.Close();
        }

        private void contactButton_Click(object sender, RoutedEventArgs e)
        {
            ContactsWindow contactsWindow = new ContactsWindow();
            contactsWindow.Show();
            this.Close();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddOrderWindow addOrderWindow = new AddOrderWindow();
            addOrderWindow.Show();
            this.Close();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = dataGrid.SelectedItem as Order;
            if (selectedOrder != null)
            {
                MongoRepository mongoRepository = new MongoRepository();
                mongoRepository.DeleteOrder(selectedOrder);
            }
            RefreshData();            
        }

        private async void RefreshData()
        {
            MongoRepository mongoRepository = new MongoRepository();
            dataGrid.ItemsSource = await mongoRepository.GetOrders();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshData();      
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = dataGrid.SelectedItem as Order;
            if (selectedItem != null)
            {
                UpdateOrderWindow updateOrderWindow = new UpdateOrderWindow(selectedItem);
                updateOrderWindow.Show();
                this.Close();
            }
        }
    }
}
