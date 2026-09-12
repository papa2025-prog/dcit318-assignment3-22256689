using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace InventorySerializationApp
{
    // b. Marker Interface
    public interface IInventoryEntity
    {
        int Id { get; }
    }

    // a. Immutable Inventory Record
    public record InventoryItem(int Id, string Name, int Quantity, DateTime DateAdded) : IInventoryEntity;

    // c. Generic Inventory Logger
    public class InventoryLogger<T> where T : IInventoryEntity
    {
        private List<T> _log = new List<T>();
        private string _filePath;

        public InventoryLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void Add(T item) => _log.Add(item);

        public List<T> GetAll() => _log;

        // d & e. Save to File with exception handling & using blocks
        public void SaveToFile()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(_log, options);

                using (StreamWriter writer = new StreamWriter(_filePath))
                {
                    writer.Write(jsonString);
                }
                Console.WriteLine("Data successfully saved to file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data to file: {ex.Message}");
            }
        }

        // d & e. Load from File with exception handling & using blocks
        public void LoadFromFile()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    Console.WriteLine("Save file not found.");
                    return;
                }

                string jsonString;
                using (StreamReader reader = new StreamReader(_filePath))
                {
                    jsonString = reader.ReadToEnd();
                }

                var items = JsonSerializer.Deserialize<List<T>>(jsonString);
                if (items != null)
                {
                    _log = items;
                }
                Console.WriteLine("Data successfully loaded from file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data from file: {ex.Message}");
            }
        }
    }

    // f. InventoryApp Integration Layer
    public class InventoryApp
    {
        private InventoryLogger<InventoryItem> _logger;
        private string filePath = "inventory_storage.json";

        public InventoryApp()
        {
            _logger = new InventoryLogger<InventoryItem>(filePath);
        }

        public void SeedSampleData()
        {
            _logger.Add(new InventoryItem(1, "Mechanical Keyboard", 15, DateTime.Now));
            _logger.Add(new InventoryItem(2, "Wireless Mouse", 40, DateTime.Now));
            _logger.Add(new InventoryItem(3, "UltraWide Monitor", 8, DateTime.Now));
            _logger.Add(new InventoryItem(4, "USB-C Hub", 25, DateTime.Now));
        }

        public void SaveData()
        {
            _logger.SaveToFile();
        }

        public void LoadData()
        {
            _logger.LoadFromFile();
        }

        public void PrintAllItems()
        {
            Console.WriteLine("\n--- Recovered Inventory Items ---");
            foreach (var item in _logger.GetAll())
            {
                Console.WriteLine($"ID: {item.Id} | Name: {item.Name} | Qty: {item.Quantity} | Added: {item.DateAdded}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // g. Main Application Flow
            InventoryApp app = new InventoryApp();
            
            Console.WriteLine("1. Seeding sample data...");
            app.SeedSampleData();

            Console.WriteLine("2. Saving data to persistent disk storage...");
            app.SaveData();

            Console.WriteLine("3. Simulating app session reset (clearing memory)...");
            app = null; // simulate clearing memory/new session

            Console.WriteLine("4. Initializing new session and loading data from storage...");
            InventoryApp newSessionApp = new InventoryApp();
            newSessionApp.LoadData();

            Console.WriteLine("5. Printing items to confirm recovery...");
            newSessionApp.PrintAllItems();
        }
    }
}
