using InventoryPro.Windows;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;

namespace InventoryPro
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        public HomeWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            MongoRepository mongoRepository = new MongoRepository();
            List<Bill> bills = await mongoRepository.GetPaidBills();
            paidBillsNumberText.Text = bills.Count.ToString();
            List<Product> products = await mongoRepository.GetAvailableProducts();
            productsNumberText.Text = products.Count.ToString();
            int numOfOrderedDeliveries = await mongoRepository.GetNumberOfDeliveredDeliveries();
            int numOfIncomingDeliveries = await mongoRepository.GetNumberOfIncomingDeliveries();
            float totalSum = await mongoRepository.GetSumOfDeliveriesCost();
            orderedDeliveriesText.Text = "Dostavljene dostave: " + numOfOrderedDeliveries.ToString();
            incomingDeliveriesText.Text = "Nadolazeće dostave: " + numOfIncomingDeliveries.ToString();
            deliveriesCostSumText.Text = "Ukupni trošak: " + totalSum.ToString() + " €";

            int numOfOrderedOrders = await mongoRepository.GetNumberOfDeliveredOrders();
            int numOfIncomingOrders = await mongoRepository.GetNumberOfIncomingOrders();
            float totalSum2 = await mongoRepository.GetSumOfOrdersCost();
            orderedOrdersText.Text = "Dostavljene narudžbe: " + numOfOrderedOrders.ToString();
            incomingOrdersText.Text = "Nadolazeće narudžbe: " + numOfIncomingOrders.ToString();
            ordersCostSumText.Text = "Ukupni trošak: " + totalSum2.ToString() + " €";

        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            InventoryWindow inventoryWindow = new InventoryWindow();
            inventoryWindow.Show();
            this.Close();

        }

        private void billsButton_Click(object sender, RoutedEventArgs e)
        {
            BillsWindow billsWindow = new BillsWindow();
            billsWindow.Show();
            this.Close();
        }

        private void contactButton_Click(object sender, RoutedEventArgs e)
        {
            ContactsWindow contactsWindow = new ContactsWindow();
            contactsWindow.Show();
            this.Close();
        }

        private void deliveryButton_Click(object sender, RoutedEventArgs e)
        {
            DeliveryWindow deliveryWindow = new DeliveryWindow();
            deliveryWindow.Show();
            this.Close();
        }

        private void orderButton_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new OrderWindow();
            orderWindow.Show();
            this.Close();
        }
    }
}
