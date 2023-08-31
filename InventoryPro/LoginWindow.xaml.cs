﻿using System.Windows;


namespace InventoryPro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {
               MongoRepository repository = new MongoRepository();
               bool passwordIsCorrect = await repository.LoginUser(passwordText.Text, usernameText.Text);

               if (passwordIsCorrect)
               {
                   HomeWindow homeWindow = new HomeWindow();
                   homeWindow.Show();
                   this.Close();
               }
               else
               {
                   MessageBox.Show("Netočni podaci tijekom prijave!");

               }

        }

        private void registrationButton_Click(object sender, RoutedEventArgs e)
        {          

            RegistrationWindow regWindow = new RegistrationWindow();
            regWindow.Show();
            this.Close();
        }
    }
}
