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
    /// Interaction logic for ContactsWindow.xaml
    /// </summary>
    public partial class ContactsWindow : Window
    {
        public ContactsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }


        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddContactWindow addContactWindow = new AddContactWindow();
            addContactWindow.Show();
            this.Close();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dataGrid.SelectedItem as Contact;
            MongoRepository mongoRepository = new MongoRepository();
            mongoRepository.DeleteContact(selectedItem);
            RefreshData();
        }


        private async void RefreshData()
        {
            MongoRepository mongoRepository = new MongoRepository();
            dataGrid.ItemsSource = await mongoRepository.GetContacts();
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
        }
    }
}
