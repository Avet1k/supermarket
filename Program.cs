namespace Market;
class Program
{
    static void Main(string[] args)
    {
        Supermarket supermarket;
        Queue<Client> queue = new Queue<Client>();

        queue.Enqueue(new Client(1500));
        queue.Enqueue(new Client(1000));
        queue.Enqueue(new Client(20));

        supermarket = new Supermarket(queue);
        
        supermarket.Work();
    }
}

abstract class Product
{
    private string _name;

    protected Product(string name, int price)
    {
        _name = name;
        Price = price;
    }
    
    public int Price { get; }
    
    public void ShowInfo()
    {
        Console.WriteLine($"{_name} - {Price} руб.");
    }
}

class Apples : Product
{
    public Apples() : base(name: "Яблоки", price: 50) { }
}

class Bacon : Product
{
    public Bacon() : base(name: "Бекон", price: 200) { }
}

class Caviar : Product
{
    public Caviar() : base(name: "Икра", price: 1000) { }
}

class Client
{
    private int _money;
    private List<Product> _cart;
    
    public Client(int money)
    {
        _money = money;
        _cart = new List<Product>();
        
        AddToCart(new Apples());
        AddToCart(new Bacon());
        AddToCart(new Caviar());
    }

    public void AddToCart(Product product)
    {
        _cart.Add(product);
    }

    public void RemoveRandomProduct()
    {
        Random random = new Random();
        int maxIndex = _cart.Count;
        
        _cart.RemoveAt(random.Next(maxIndex));
    }

    public bool TryBuy(int cost)
    {
        if (_money < cost)
            return false;
        
        _money -= cost;
        return true;
    }

    public void ShowCart()
    {
        foreach (var product in _cart)
            product.ShowInfo();
    }

    public int GetProductPriceByIndex(int index)
    {
        return _cart[index].Price;
    }

    public int GetCartCount()
    {
        return _cart.Count;
    }
}

class Supermarket
{
    private int _money;
    private Queue<Client> _queue;

    public Supermarket(Queue<Client> queue)
    {
        _queue = queue;
    }

    public void Work()
    {
        while (_queue.Count > 0)
        {
            Console.Clear();
            Console.WriteLine($"Супермаркет работает!\nКасса: {_money}\n\n");

            Console.WriteLine("Нажмите любую кнопку, чтобы принять покупателя...");
            Console.ReadKey();
            
            AcceptBuyer(_queue.Dequeue());
        }
        
        Console.Clear();
        Console.WriteLine($"Очередь подошла к концу. Выручка за рабочий день: {_money} руб.");
    }

    public void AcceptBuyer(Client client)
    {
        bool isPurchaseDone = false;

        while (isPurchaseDone == false)
        {
            Console.WriteLine("\nКорзина клиента:");
            client.ShowCart();
            
            Thread.Sleep(1000);

            int cost = CalculateCost(client);

            if (cost > 0 && client.TryBuy(cost))
            {
                Console.WriteLine($"\nКлиент купил товаров на сумму {cost} руб.\n");
                _money += cost;
                isPurchaseDone = true;
            }
            else if (cost == 0)
            {
                Console.WriteLine("\nКорзина пуста, покупатель уходит ни с чем.\n");
                isPurchaseDone = true;
            }
            else
            {
                Console.WriteLine("\nУ клиента недостаточно денег. Он выкладывает случайный товар.\n");
                client.RemoveRandomProduct();
            }

            Console.WriteLine("Нажмите любую кнопку, чтобы продолжить...");
            Console.ReadKey();
        }
    }

    private int CalculateCost(Client client)
    {
        int cost = 0;

        for (int i = 0; i < client.GetCartCount(); i++)
            cost += client.GetProductPriceByIndex(i);

        return cost;
    }
}