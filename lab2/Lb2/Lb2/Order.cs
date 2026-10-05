using System;
using System.Collections.Generic;

namespace Lb2
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public Customer Customer { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        public Order(int id, Customer customer)
        {
            Id = id;
            Customer = customer;
            OrderDate = DateTime.Now;
        }

        public void AddItem(Product product, int quantity)
        {
            if (product.StockQuantity < quantity)
            {
                Console.WriteLine($"[ПОМИЛКА] Недостатньо товару \"{product.Name}\" на складі!");
                return;
            }

            product.StockQuantity -= quantity;
            Items.Add(new OrderItem(Items.Count + 1, product, quantity));
        }

        public decimal CalculateTotal()
        {
            decimal total = 0;
            foreach (var item in Items)
            {
                total += item.GetSubtotal();
            }
            return total;
        }

        public void PrintReceipt()
        {
            Console.WriteLine($"==========================================");
            Console.WriteLine($"ЗАМОВЛЕННЯ №{Id} від {OrderDate:dd.MM.yyyy HH:mm}");
            Console.WriteLine($"Клієнт: {Customer}");
            Console.WriteLine($"------------------------------------------");
            Console.WriteLine("Товари:");
            foreach (var item in Items)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine($"------------------------------------------");
            Console.WriteLine($"РАЗОМ ДО СПЛАТИ: {CalculateTotal():C2}");
            Console.WriteLine($"==========================================\n");
        }
    }
}