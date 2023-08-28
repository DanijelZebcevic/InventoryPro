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
    /// Interaction logic for UpdateContactWindow.xaml
    /// </summary>
    public partial class UpdateContactWindow : Window
    {
        Contact passedContact;
        public UpdateContactWindow(Contact contact)
        {
            InitializeComponent();
            passedContact = contact;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            nameText.Text = passedContact.Name;
            addressText.Text = passedContact.Address;
            emailText.Text = passedContact.EmailAddress;
            phoneNumberText.Text = passedContact.PhoneNumber;
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            Contact newContact = new Contact();
            newContact.Name = nameText.Text;
            newContact.Address = addressText.Text;
            newContact.PhoneNumber = phoneNumberText.Text;
            newContact.EmailAddress = emailText.Text;
            MongoRepository mongoRepository = new MongoRepository();
            mongoRepository.UpdateContact(passedContact, newContact);
            ContactsWindow contactsWindow = new ContactsWindow();
            contactsWindow.Show();
            this.Close();
        }

        private void returnButton_Click(object sender, RoutedEventArgs e)
        {
            ContactsWindow contactsWindow = new ContactsWindow();
            contactsWindow.Show();
            this.Close();
        }
    }
}
