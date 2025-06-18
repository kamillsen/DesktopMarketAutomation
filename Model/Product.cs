using System;

namespace MarketAutomation.Models
{
    /// <summary>
    /// Ürün bilgilerini temsil eden model sınıfı
    /// </summary>
    public class Product
    {
        public string Barcode { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Product()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
} 