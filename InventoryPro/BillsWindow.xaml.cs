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
    /// Interaction logic for BillsWindow.xaml
    /// </summary>
    public partial class BillsWindow : Window
    {
        public BillsWindow()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddBillWindow addBillWindow = new AddBillWindow();
            addBillWindow.Show();
            this.Close();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
            MongoRepository mongoRepository = new MongoRepository();
            dataGrid.ItemsSource = await mongoRepository.GetBills();
        }
    }
}
