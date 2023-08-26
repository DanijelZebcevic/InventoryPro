
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
          /*  string connectionString = "mongodb+srv://dzebcevic:WaivXdKY77dCWhDk@cluster0.jn3wc2c.mongodb.net/";
            string databaseName = "InventoryProDB";
            string collectionName = "users";

            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(databaseName);
            var collection = db.GetCollection<User>(collectionName);

            var person = new User { Username = usernameText.Text.ToString(), Email = emailText.Text.ToString(), Password = passwordText.Text.ToString() }; //id obvezno string
            await collection.InsertOneAsync(person);*/

            MongoRepository repository = new MongoRepository();
            repository.AddUser(usernameText.Text, emailText.Text, passwordText.Text);



            LoginWindow newWindow = new LoginWindow();
            newWindow.Show();
            this.Close();

        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
