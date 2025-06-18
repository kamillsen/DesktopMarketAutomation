using System.ComponentModel;

namespace MarketAutomation.Models
{
    /// <summary>
    /// Satış detaylarını temsil eden model sınıfı
    /// </summary>
    public class SaleItem : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public string Barcode { get; set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    TotalPrice = _quantity * UnitPrice;
                    OnPropertyChanged(nameof(Quantity));
                    OnPropertyChanged(nameof(TotalPrice));
                }
            }
        }

        public decimal UnitPrice { get; set; }

        private decimal _totalPrice;
        public decimal TotalPrice
        {
            get => _totalPrice;
            set
            {
                if (_totalPrice != value)
                {
                    _totalPrice = value;
                    OnPropertyChanged(nameof(TotalPrice));
                }
            }
        }

        /// <summary>
        /// Ürün adını döndürür (veritabanından alınır)
        /// </summary>
        public string ProductName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
