
using System.Windows;


namespace InventoryPro
{
    /// <summary>
    /// Interaction logic for AddContactWindow.xaml
    /// </summary>
    public partial class AddContactWindow : Window
    {
        public AddContactWindow()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            MongoRepository mongoRepository = new MongoRepository();
            if (nameText.Text != "")
            {
                mongoRepository.AddContact(nameText.Text, addressText.Text, phoneNumberText.Text, emailText.Text);
                ContactsWindow contactsWindow = new ContactsWindow();
                contactsWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Morate upisati ime kontakta!");
            }
            
        }

        private void returnButton_Click(object sender, RoutedEventArgs e)
        {
            ContactsWindow contactsWindow = new ContactsWindow();
            contactsWindow.Show();
            this.Close();
        }
    }
}
