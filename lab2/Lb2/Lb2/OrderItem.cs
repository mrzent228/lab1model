using System;

namespace Lb2
{
    public class OrderItem
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }

        public OrderItem(int id, Product product, int quantity)
        {
            Id = id;
            Product = product;
            Quantity = quantity;
            PriceAtPurchase = product.Price;
        }

        public decimal GetSubtotal() => Quantity * PriceAtPurchase;

        public override string ToString()
        {
            return $"   • {Product.Name,-12} x {Quantity} шт. = {GetSubtotal():C2} (по {PriceAtPurchase:C2}/шт)";
        }
    }
}