using MongoDB.Driver;
using BCrypt.Net;
using System.Windows;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Documents;

namespace InventoryPro
{
    public class MongoRepository
    {

        public static string connectionString = "mongodb+srv://dzebcevic:WaivXdKY77dCWhDk@cluster0.jn3wc2c.mongodb.net/";
        public static string databaseName = "InventoryProDB";
        public static MongoClient client = new MongoClient(connectionString);
        public IMongoDatabase database = client.GetDatabase(databaseName);

        public async void AddUser(string username, string email, string password)
        {            
            
            string collectionName = "users";                        
            var collection = database.GetCollection<User>(collectionName);
            
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password + salt);

            var person = new User { Username = username, Email = email, Password = hashedPassword, Salt = salt }; //id obvezno string
            await collection.InsertOneAsync(person);
        }

        public async Task<bool> LoginUser(string userProvidedPassword, string userProvidedUsername)
        {            
            string collectionName = "users";
            var userCollection = database.GetCollection<User>(collectionName);
            var filter = Builders<User>.Filter.Eq("Username", userProvidedUsername);

            var foundUsers = await userCollection.Find(filter).ToListAsync();
            

            string retrievedSalt = foundUsers[0].Salt;            
            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(userProvidedPassword + retrievedSalt, foundUsers[0].Password);
            if(isPasswordCorrect)
            {
                return true;
               
            }
            else
            {
                return false;
            }

        }

        public async void AddProduct(string name, int amount)
        {
            string collectionName = "products";
            var collection = database.GetCollection<Product>(collectionName);

            var product = new Product {Name = name, Amount = amount };
            await collection.InsertOneAsync(product);

        }

        public async Task<List<Product>> GetProducts()
        {
            List<Product> products = new List<Product>();
            string collectionName = "products";
            var collection = database.GetCollection<Product>(collectionName);
            var results = await collection.FindAsync(_ => true);

            foreach (var product in results.ToList())
            {
                products.Add(product);
            }
            return products;
        }
    }
}
