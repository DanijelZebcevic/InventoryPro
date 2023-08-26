using System.Windows;


namespace InventoryPro
{
    /// <summary>
    /// Interaction logic for AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();
        }

        private async void addButton_Click(object sender, RoutedEventArgs e)
        {
            MongoRepository mongoRepository = new MongoRepository();
            mongoRepository.AddProduct(nameText.Text,int.Parse(amountText.Text),float.Parse(priceText.Text));
            InventoryWindow inventoryWindow = new InventoryWindow();
            inventoryWindow.Show();            
            this.Close();
        }

        private void returnButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
