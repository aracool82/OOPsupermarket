using System;
using System.Collections.Generic;

namespace Supermarket
{
    public class Program
    {
        public static Random random = new Random();
        
        public static void Main()
        {
            int countClients = 4;
            List<Client> clients = CreateClients(countClients);
            Supermarket supermarket = new Supermarket(clients);
            supermarket.StartTrade();
        }

        public static List<Client> CreateClients(int count )
        {
            List<Client> clients = new List<Client>();
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Client client = new Client(random.Next(250,401));
                    client.FillBasket(CreateClientBasket());
                    clients.Add(client);
                }
            }
            return clients;
        }

        public static  List<Product> CreateClientBasket()
        {
            List<Product> newProducts = new List<Product>();
            List<string> listProduct = new List<string>(){"Чупа-чупс","Колла","Яблоки","Хлеб","Масло","Колбаса",
                "Мука","Сгущенка","Печенье","Конфеты" ,"Огурцы","Шоколад"};
            
            int count = random.Next(3, 7);
            
            for (int i = 0; i < count; i++)
            {
                string name = listProduct[random.Next(0, listProduct.Count)];
                int price = random.Next(20, 150);
                newProducts.Add(new Product(name,price));
            }
            return newProducts;
        }
    }

    public class Supermarket
    {
        private Queue<Client> _queueCLients= new Queue<Client>();
        public List<Client> Clients { get; }

        public Supermarket(List<Client> clients)
        {
            Clients = clients;
            foreach (var client in Clients)
                _queueCLients.Enqueue(client);
        }

        public void StartTrade()
        {
            while (_queueCLients.Count != 0)
            {
                Client client = _queueCLients.Peek();
                int clientSum = CalculateSum(client);
                Console.WriteLine($"Сумма у клиента {client.Money}, а корзина {clientSum}");
                if (client.Money >= clientSum)
                {
                    _queueCLients.Dequeue();
                    Console.WriteLine("Обслужили клиента");
                }
                else
                {
                    client.PullOutRandomOneProduct();
                }
            }
        }

        private int CalculateSum(Client client)
        {
            List<Product> clientProducts=new List<Product>();
            clientProducts = client.ShowBasketInfo();
            int sum = 0;
            foreach (var product in clientProducts)
                sum += product.Price;
            return sum;
        }
    }

    public class Client
    {
        private List<Product> clientBasket = new List<Product>();
        public int Money { get;private set; }

        public Client(int money)
        {
            Money = money;
        }

        public void PullOutRandomOneProduct()
        {
            if (clientBasket.Count > 0)
            {
                int number = Program.random.Next(0, clientBasket.Count);
                Console.WriteLine(clientBasket[number].Name + " был удален");
                clientBasket.RemoveAt(number);
            }
        }

        public void FillBasket(List<Product> products)
        {
            clientBasket = products;
        }

        public List<Product> ShowBasketInfo()
        {
            foreach (var product in clientBasket)
                Console.WriteLine($"{product.Name} {product.Price}p.");
            Console.WriteLine();
            return clientBasket;
        }
    }

    public class Product
    {
        public string Name { get;private set; }
        public int Price { get;private set; }

        public  Product(string name,int price)
        {
            Name = name;
            Price = price;
        }
    }
}