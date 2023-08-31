
using System.Threading.Tasks;
using System.Windows;

using MongoDB.Driver;

namespace InventoryPro
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private async void registrationButton_Click(object sender, RoutedEventArgs e)
        {
         
            MongoRepository repository = new MongoRepository();
            if (usernameText.Text != "" && emailText.Text != "" & passwordText.Text != "")
            {
                if (passwordText.Text == password2Text.Text)
                {
                    repository.AddUser(usernameText.Text, emailText.Text, passwordText.Text);
                    LoginWindow newWindow = new LoginWindow();
                    newWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lozinke moraju biti iste!");
                }
            }
            else
            {
                MessageBox.Show("Niste upisali sve podatke!");
            }
       

        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
