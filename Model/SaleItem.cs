namespace MarketAutomation.Models
{
    /// <summary>
    /// Satış detaylarını temsil eden model sınıfı
    /// </summary>
    public class SaleItem
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public string Barcode { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Ürün adını döndürür (veritabanından alınacak)
        /// </summary>
        public string ProductName { get; set; }
    }
} 