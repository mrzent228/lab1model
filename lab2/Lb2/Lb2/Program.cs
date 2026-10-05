using System;
using System.Collections.Generic;
using System.Text;

namespace Lb2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            // 1. Инициализация категорий
            List<Category> categories = new List<Category>
            {
                new Category(1, "Хлібобулочні вироби"),
                new Category(2, "Молочні продукти"),
                new Category(3, "Напої")
            };

            // 2. Инициализация товаров
            List<Product> products = new List<Product>
            {
                new Product(1, "Батон", 25.00m, 50, 1),
                new Product(2, "Молоко", 42.50m, 30, 2),
                new Product(3, "Сосиски", 140.00m, 15, 2),
                new Product(4, "Йогурт", 65.00m, 20, 3)
            };

            // 3. Инициализация клиентов
            List<Customer> customers = new List<Customer>
            {
                new Customer(1, "СинявськийНазар", "0671234567"),
                new Customer(2, "Шевченко Тарас", "0509876543")
            };

            // Вывод категорий
            Console.WriteLine("=== КАТЕГОРІЇ ТОВАРІВ ===");
            foreach (var category in categories)
            {
                Console.WriteLine(category);
            }
            Console.WriteLine();

            // Вывод клиентов
            Console.WriteLine("=== БАЗА КЛІЄНТІВ ===");
            foreach (var customer in customers)
            {
                Console.WriteLine(customer);
            }
            Console.WriteLine();

            // Вывод каталога товаров
            Console.WriteLine("=== КАТАЛОГ ТОВАРІВ МАГАЗИНУ ===");
            foreach (var product in products)
            {
                Console.WriteLine(product);
            }
            Console.WriteLine();

            // 4. Оформление заказов
            Console.WriteLine("=== ОФОРМЛЕНІ ЗАМОВЛЕННЯ ===");
            Order order1 = new Order(101, customers[0]);
            order1.AddItem(products[0], 2); 
            order1.AddItem(products[3], 1); 
            order1.PrintReceipt();

            Order order2 = new Order(102, customers[1]);
            order2.AddItem(products[2], 2); 
            order2.AddItem(products[1], 3); 
            order2.PrintReceipt();

            // 5. Вывод обновленного остатка
            Console.WriteLine("=== ОНОВЛЕНИЙ ЗАЛИШОК НА СКЛАДІ ===");
            foreach (var product in products)
            {
                Console.WriteLine(product);
            }
        }
    }
}