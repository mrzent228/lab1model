using System;

namespace Lb2
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }

        public Product(int id, string name, decimal price, int stockQuantity, int categoryId)
        {
            Id = id;
            Name = name;
            Price = price;
            StockQuantity = stockQuantity;
            CategoryId = categoryId;
        }

        public override string ToString()
        {
            return $"ID: {Id,-2} | {Name,-15} | Ціна: {Price,7:C2} | Залишок: {StockQuantity} шт.";
        }
    }
}