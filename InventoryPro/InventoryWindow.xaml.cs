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
    /// Interaction logic for InventoryWindow.xaml
    /// </summary>
    public partial class InventoryWindow : Window
    {
        public InventoryWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MongoRepository mongoRepository = new MongoRepository();
            RefreshData();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddProductWindow addProductWindow = new AddProductWindow();
            addProductWindow.Show();
            this.Close();

        }

        private async void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = dataGrid.SelectedItem as Product;
            MongoRepository mongoRepository = new MongoRepository();
            mongoRepository.DeleteProduct(selected);
            RefreshData();
        }

        private async void RefreshData()
        {
            MongoRepository mongoRepository = new MongoRepository();
            List<Product> result = await mongoRepository.GetProducts();
            dataGrid.ItemsSource = result;
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = dataGrid.SelectedItem as Product;
            if (selectedItem is not null)
            {
                UpdateProductWindow updateProductWindow = new UpdateProductWindow(selectedItem);
                updateProductWindow.Show();
                this.Close();
            }
        }
    }
}
