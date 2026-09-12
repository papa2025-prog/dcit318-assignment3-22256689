using System;
using System.Collections.Generic;

namespace WarehouseManagement
{
    // a. Marker Interface
    public interface IInventoryItem
    {
        int Id { get; }
        string Name { get; }
        int Quantity { get; set; }
    }

    // b. ElectronicItem Class
    public class ElectronicItem : IInventoryItem
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Quantity { get; set; }
        public string Brand { get; private set; }
        public int WarrantyMonths { get; private set; }

        public ElectronicItem(int id, string name, int quantity, string brand, int warrantyMonths)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            Brand = brand;
            WarrantyMonths = warrantyMonths;
        }
    }

    // c. GroceryItem Class
    public class GroceryItem : IInventoryItem
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; private set; }

        public GroceryItem(int id, string name, int quantity, DateTime expiryDate)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            ExpiryDate = expiryDate;
        }
    }

    // e. Custom Exceptions
    public class DuplicateItemException : Exception
    {
        public DuplicateItemException(string message) : base(message) { }
    }

    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string message) : base(message) { }
    }

    public class InvalidQuantityException : Exception
    {
        public InvalidQuantityException(string message) : base(message) { }
    }

    // d. Generic Inventory Repository
    public class InventoryRepository<T> where T : IInventoryItem
    {
        private Dictionary<int, T> _items = new Dictionary<int, T>();

        public void AddItem(T item)
        {
            if (_items.ContainsKey(item.Id))
            {
                throw new DuplicateItemException($"Item with ID {item.Id} already exists.");
            }
            _items[item.Id] = item;
        }

        public T GetItemById(int id)
        {
            if (!_items.ContainsKey(id))
            {
                throw new ItemNotFoundException($"Item with ID {id} was not found.");
            }
            return _items[id];
        }

        public void RemoveItem(int id)
        {
            if (!_items.ContainsKey(id))
            {
                throw new ItemNotFoundException($"Cannot remove. Item with ID {id} not found.");
            }
            _items.Remove(id);
        }

        public List<T> GetAllItems()
        {
            return new List<T>(_items.Values);
        }

        public void UpdateQuantity(int id, int newQuantity)
        {
            if (newQuantity < 0)
            {
                throw new InvalidQuantityException("Quantity cannot be negative.");
            }
            var item = GetItemById(id);
            item.Quantity = newQuantity;
        }
    }

    // f. Warehouse Manager Class
    public class WareHouseManager
    {
        private InventoryRepository<ElectronicItem> _electronics = new InventoryRepository<ElectronicItem>();
        private InventoryRepository<GroceryItem> _groceries = new InventoryRepository<GroceryItem>();

        public void SeedData()
        {
            _electronics.AddItem(new ElectronicItem(1, "Laptop", 10, "Dell", 24));
            _electronics.AddItem(new ElectronicItem(2, "Smartphone", 25, "Samsung", 12));

            _groceries.AddItem(new GroceryItem(101, "Milk", 50, DateTime.Now.AddDays(7)));
            _groceries.AddItem(new GroceryItem(102, "Bread", 30, DateTime.Now.AddDays(3)));
        }

        public void PrintAllItems<T>(InventoryRepository<T> repo) where T : IInventoryItem
        {
            foreach (var item in repo.GetAllItems())
            {
                Console.WriteLine($"[ID: {item.Id}] Name: {item.Name}, Quantity: {item.Quantity}");
            }
        }

        public void IncreaseStock<T>(InventoryRepository<T> repo, int id, int quantity) where T : IInventoryItem
        {
            try
            {
                var item = repo.GetItemById(id);
                repo.UpdateQuantity(id, item.Quantity + quantity);
                Console.WriteLine($"Successfully updated stock for ID {id}. New quantity: {item.Quantity}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error increasing stock: {ex.Message}");
            }
        }

        public void RemoveItemById<T>(InventoryRepository<T> repo, int id) where T : IInventoryItem
        {
            try
            {
                repo.RemoveItem(id);
                Console.WriteLine($"Successfully removed item with ID {id}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing item: {ex.Message}");
            }
        }
        
        public InventoryRepository<ElectronicItem> ElectronicsRepo => _electronics;
        public InventoryRepository<GroceryItem> GroceriesRepo => _groceries;
    }

    class Program
    {
        static void Main(string[] args)
        {
            WareHouseManager manager = new WareHouseManager();
            manager.SeedData();

            Console.WriteLine("--- Grocery Items ---");
            manager.PrintAllItems(manager.GroceriesRepo);

            Console.WriteLine("\n--- Electronic Items ---");
            manager.PrintAllItems(manager.ElectronicsRepo);

            Console.WriteLine("\n--- Exception Handling Tests ---");
            
            // Try to add duplicate item
            try
            {
                manager.GroceriesRepo.AddItem(new GroceryItem(101, "Duplicate Milk", 10, DateTime.Now));
            }
            catch (DuplicateItemException ex)
            {
                Console.WriteLine($"Caught Expected Exception: {ex.Message}");
            }

            // Try to remove a non-existent item
            manager.RemoveItemById(manager.ElectronicsRepo, 999);

            // Try to update with invalid quantity
            try
            {
                manager.GroceriesRepo.UpdateQuantity(101, -5);
            }
            catch (InvalidQuantityException ex)
            {
                Console.WriteLine($"Caught Expected Exception: {ex.Message}");
            }
        }
    }
}
