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
    /// Interaction logic for EditBillWindow.xaml
    /// </summary>
    public partial class EditBillWindow : Window
    {
        private List<Product> originalProducts = new List<Product>();
        private List<Product> selectedProducts = new List<Product>();
        private Bill passedBill;
        public EditBillWindow(Bill bill)
        {
            passedBill = bill;
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            buyerText.Text = passedBill.Buyer;
            dateOfPurchaseText.SelectedDate = passedBill.DateOfPurchase;

            if (passedBill.IsPaid == true)
            {
                billIsPaidText.SelectedIndex = 0;
            }
            else
            {
                billIsPaidText.SelectedIndex = 1;
            }
            
          
            for (int i = 0; i < passedBill.Items.Count(); i++)
            {
                var prod = passedBill.Items[i].Product;
                prod.Amount = passedBill.Items[i].AmountBought;
                selectedProducts.Add(prod);
            }
            MongoRepository mongoRepository = new MongoRepository();
            originalProducts = await mongoRepository.GetProductsNotInList(selectedProducts);
            dataGrid.ItemsSource = originalProducts;
            dataGrid2.ItemsSource = selectedProducts;
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
            dataGrid.Columns[2].Visibility = Visibility.Collapsed;
            dataGrid.Columns[3].Visibility = Visibility.Collapsed;

            dataGrid2.Columns[1].Visibility = Visibility.Collapsed;
            dataGrid2.Columns[3].Visibility = Visibility.Collapsed;
            dataGrid2.Columns[4].Visibility = Visibility.Collapsed;

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
            bill.Buyer = buyerText.Text;

            ComboBoxItem selectedComboBoxItem = (ComboBoxItem)billIsPaidText.Items[billIsPaidText.SelectedIndex];
            string selectedContent = selectedComboBoxItem.Content.ToString();

            if (selectedContent == "Da")
            {
                bill.IsPaid = true;
            }
            else
            {
                bill.IsPaid = false;
            }

            DateTime? selectedDate = dateOfPurchaseText.SelectedDate;
            if (selectedDate.HasValue)
            {
                bill.DateOfPurchase = selectedDate.Value.Date;
            }
            if (bill.Items.Count < 1)
            {
                MessageBox.Show("Račun ne može biti bez proizvoda!");
            }
            else
            {
                mongoRepository.UpdateBill(passedBill, bill);
                BillsWindow billWindow = new BillsWindow();
                billWindow.Show();
                this.Close();
            }
        
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            BillsWindow billsWindow = new BillsWindow();
            billsWindow.Show();
            this.Close();
        }
    }
}
