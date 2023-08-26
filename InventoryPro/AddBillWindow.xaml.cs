
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace InventoryPro
{
    /// <summary>
    /// Interaction logic for AddBillWindow.xaml
    /// </summary>
    public partial class AddBillWindow : Window
    {
        private List<Product> originalProducts = new List<Product>();
        private List<Product> selectedProducts = new List<Product>();
        public AddBillWindow()
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
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
            dataGrid.Columns[2].Visibility = Visibility.Collapsed;
            dataGrid.Columns[3].Visibility = Visibility.Collapsed;


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
                            items.Add(new Items { Amount = helpProduct.Amount, Product = item, Sum = helpProduct.Amount * item.PricePerUnit });
                        }
                    }
                }
              
            }
            float total = 0;
            Bill bill = new Bill();
            foreach (var item in items)
            {
                total += item.Sum;
            }
            bill.Sum = total;
            bill.Items = items;
            mongoRepository.AddBill(bill);
            BillsWindow billWindow = new BillsWindow();
            billWindow.Show();
            this.Close();
        }

        private void dataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedItem = dataGrid.SelectedItem as Product;
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

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            BillsWindow billsWindow = new BillsWindow();
            billsWindow.Show();
            this.Close();   
        }
    }
}
