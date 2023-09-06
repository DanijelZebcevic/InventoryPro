using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

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

            if (foundUsers.Count > 0)
            {
                string retrievedSalt = foundUsers[0].Salt;
                bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(userProvidedPassword + retrievedSalt, foundUsers[0].Password);
                if (isPasswordCorrect)
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            

        }

        public async void AddProduct(string name, int amount, float price)
        {
            string collectionName = "products";
            var collection = database.GetCollection<Product>(collectionName);

            var product = new Product {Name = name, PricePerUnit = price, Amount = amount };
            await collection.InsertOneAsync(product);

        }

        public async void UpdateProduct(Product newProduct, Product oldProduct)
        {
            string collectionName = "products";
            var collection = database.GetCollection<Product>(collectionName);

            var updateDefinition = Builders<Product>.Update
                .Set(p => p.Name, newProduct.Name)  
                .Set(p => p.PricePerUnit, newProduct.PricePerUnit)  
                .Set(p => p.Amount, newProduct.Amount);

            var updateResult = await collection.UpdateOneAsync(
                    p => p.Id == oldProduct.Id,
                    updateDefinition);

        
        }

        public async void DeleteProduct(Product p)
        {
            var id = p.Id;
            var collection = database.GetCollection<Product>("products");
            var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            collection.DeleteOne(filter);

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

        public async Task<List<Product>> GetProductsNotInList(List<Product> passedList)
        {

            IMongoCollection<Product> productsCollection = database.GetCollection<Product>("products");
            List<string> selectedProductNames = passedList.Select(p => p.Name).ToList();
            var filter = Builders<Product>.Filter.Not(
                    Builders<Product>.Filter.Where(p => selectedProductNames.Contains(p.Name)));
            var prodsNotInlist=productsCollection.Find(filter).ToList();
            return prodsNotInlist;

        }

        public async Task<List<Product>> GetAvailableProducts()
        {
            var collection = database.GetCollection<Product>("products");
            var results = await collection.FindAsync(_ => true);
            List<Product> all = new List<Product>();

            foreach (var item in results.ToList())
            {
                if (item.Amount > 0)
                {
                    all.Add(item);
                }
            }

            return all;

        }



        public async Task<List<Contact>> GetContacts()
        {
            List<Contact> contacts = new List<Contact>();
            var collection = database.GetCollection<Contact>("contacts");
            var results = await collection.FindAsync(_ => true);

            foreach (var contact in results.ToList())
            {
                contacts.Add(contact);
            }
            return contacts;
        }

        public async void AddContact(string name, string address, string phoneNumber, string emailAddress)
        {
            var collection = database.GetCollection<Contact>("contacts");
            var contact = new Contact { Address = address, EmailAddress = emailAddress, Name = name, PhoneNumber = phoneNumber };

            await collection.InsertOneAsync(contact);
        }

        public async void DeleteContact(Contact c)
        {
            var collection = database.GetCollection<Contact>("contacts");
            var filter = Builders<Contact>.Filter.Eq(contact => contact.Id, c.Id);
            collection.DeleteOne(filter);
        }

        public async void UpdateContact(Contact oldContact, Contact newContact)
        {
            var collection = database.GetCollection<Contact>("contacts");

            var updateDefinition = Builders<Contact>.Update
                .Set(c => c.Name, newContact.Name)
                .Set(c => c.Address, newContact.Address)
                .Set(c => c.PhoneNumber, newContact.PhoneNumber)
                .Set(c => c.EmailAddress, newContact.EmailAddress);


            var updateResult = await collection.UpdateOneAsync(
                    c => c.Id == oldContact.Id,
                    updateDefinition);

        }

        public async void AddBill(Bill bill)
        {
            var collection = database.GetCollection<Bill>("bills");
            
            await collection.InsertOneAsync(bill);
        }

        public async Task<List<Bill>> GetBills()
        {
            List<Bill> bills = new List<Bill>();
            var collection = database.GetCollection<Bill>("bills");
            var results = await collection.FindAsync(_ => true);

            foreach (var bill in results.ToList())
            {
                bills.Add(bill);
            }
            return bills;
        }

        public async Task<List<Bill>> GetPaidBills()
        {
            List<Bill> paidBills = new List<Bill>();
            var collection = database.GetCollection<Bill>("bills");
            var filter = Builders<Bill>.Filter.Eq(bill => bill.IsPaid, true);

            var results = await collection.FindAsync(filter);

            await results.ForEachAsync(bill => paidBills.Add(bill));

            return paidBills;
        }

        public async void DeleteBill(Bill bill)
        {
            var id = bill.Id;
            var collection = database.GetCollection<Bill>("bills");
            var filter = Builders<Bill>.Filter.Eq(b => b.Id, id);
            collection.DeleteOne(filter);
        }

        public async void UpdateBill(Bill oldBill, Bill newBill) 
        {
            var collection = database.GetCollection<Bill>("bills");

            var updateDefinition = Builders<Bill>.Update
                .Set(b => b.Buyer, newBill.Buyer)
                .Set(b => b.DateOfPurchase, newBill.DateOfPurchase)
                .Set(b => b.Items, newBill.Items)
                .Set(b => b.IsPaid, newBill.IsPaid)
                .Set(b => b.Sum, newBill.Sum);

            var updateResult = await collection.UpdateOneAsync(
                    b => b.Id == oldBill.Id,
                    updateDefinition);
        }

        public async void AddOrder(Order order)
        {
            var collection = database.GetCollection<Order>("orders");
            await collection.InsertOneAsync(order);

        }

        public async Task<List<Order>> GetOrders()
        {
            List<Order> orders = new List<Order>();
            var collection = database.GetCollection<Order>("orders");
            var results = await collection.FindAsync(_ => true);

            foreach (var ord in results.ToList())
            {
                orders.Add(ord);
            }
            return orders;
        }

        public async void UpdateOrder(Order oldOrder, Order newOrder)
        {
            var collection = database.GetCollection<Order>("orders");

            var updateDefinition = Builders<Order>.Update
                .Set(or => or.OrderIsDelivered, newOrder.OrderIsDelivered)
                .Set(or => or.OrderedItems, newOrder.OrderedItems)
                .Set(or => or.Customer, newOrder.Customer)
                .Set(or => or.DeliveryDate, newOrder.DeliveryDate);


            var updateResult = await collection.UpdateOneAsync(
                    or => or.Id == oldOrder.Id,
                    updateDefinition);
        }

        public async void DeleteOrder(Order order)
        {
            var collection = database.GetCollection<Order>("orders");
            var filter = Builders<Order>.Filter.Eq(o => o.Id, order.Id);
            collection.DeleteOne(filter);
        }

        public async Task<int> GetNumberOfDeliveredOrders()
        {
            List<Order> orders = new List<Order>();
            var collection = database.GetCollection<Order>("orders");
            var filter = Builders<Order>.Filter.Eq(del => del.OrderIsDelivered, true);

            var results = await collection.FindAsync(filter);

            await results.ForEachAsync(bill => orders.Add(bill));

            return orders.Count;
        }

        public async Task<int> GetNumberOfIncomingOrders()
        {
            List<Order> orders = new List<Order>();
            var collection = database.GetCollection<Order>("orders");
            var filter = Builders<Order>.Filter.Eq(del => del.OrderIsDelivered, false);

            var results = await collection.FindAsync(filter);

            await results.ForEachAsync(bill => orders.Add(bill));

            return orders.Count;
        }

        public async Task<float> GetSumOfOrdersCost()
        {
            var collection = database.GetCollection<Order>("orders");
            var results = await collection.FindAsync(_ => true);
            float sum = 0;

            foreach (var del in results.ToList())
            {
                foreach (var item in del.OrderedItems)
                {
                    sum += item.Sum;
                }
            }

            return sum;
        }


        public async void AddDelivery(Delivery delivery)
        {
            var collection = database.GetCollection<Delivery>("deliveries");
            await collection.InsertOneAsync(delivery);
        }

        public async Task<List<Delivery>> GetDeliveries()
        {
            List<Delivery> deliveries = new List<Delivery>();
            var collection = database.GetCollection<Delivery>("deliveries");
            var results = await collection.FindAsync(_ => true);

            foreach (var del in results.ToList())
            {
                deliveries.Add(del);
            }
            return deliveries;
        }

        public async Task<int> GetNumberOfDeliveredDeliveries()
        {
            List<Delivery> deliveries = new List<Delivery>();
            var collection = database.GetCollection<Delivery>("deliveries");
            var filter = Builders<Delivery>.Filter.Eq(del => del.OrderIsDelivered, true);

            var results = await collection.FindAsync(filter);

            await results.ForEachAsync(bill => deliveries.Add(bill));

            return deliveries.Count;
        }

        public async Task<int> GetNumberOfIncomingDeliveries()
        {
            List<Delivery> deliveries = new List<Delivery>();
            var collection = database.GetCollection<Delivery>("deliveries");
            var filter = Builders<Delivery>.Filter.Eq(del => del.OrderIsDelivered, false);

            var results = await collection.FindAsync(filter);

            await results.ForEachAsync(bill => deliveries.Add(bill));

            return deliveries.Count;
        }

        public async Task<float> GetSumOfDeliveriesCost()
        {
            var collection = database.GetCollection<Delivery>("deliveries");
            var results = await collection.FindAsync(_ => true);
            float sum = 0;

            foreach (var del in results.ToList())
            {
                foreach (var item in del.DeliveredItems)
                {
                    sum += item.Sum;
                }
            }

            return sum;
        }


        public async void UpdateDelivery(Delivery oldDelivery, Delivery newDelivery)
        {
            var collection = database.GetCollection<Delivery>("deliveries");

            var updateDefinition = Builders<Delivery>.Update
                .Set(del => del.DeliveredItems, newDelivery.DeliveredItems)
                .Set(del => del.Deliverer, newDelivery.Deliverer)
                .Set(del => del.DeliveryDate, newDelivery.DeliveryDate)
                .Set(del => del.OrderIsDelivered, newDelivery.OrderIsDelivered);



            var updateResult = await collection.UpdateOneAsync(
                    del => del.Id == oldDelivery.Id,
                    updateDefinition);
        }

        public async void DeleteDelivery(Delivery delivery)
        {
            var collection = database.GetCollection<Delivery>("deliveries");
            var filter = Builders<Delivery>.Filter.Eq(del => del.Id, delivery.Id);
            collection.DeleteOne(filter);
        }
    }
}
