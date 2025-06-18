using MarketAutomation.Models;
using System;
using System.Collections.Generic;

namespace MarketAutomation.Models
{
    /// <summary>
    /// Satış bilgilerini temsil eden model sınıfı
    /// </summary>
    public class Sale
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public decimal TotalPrice { get; set; }
        public string CashierUsername { get; set; }
        public string PaymentMethod { get; set; }
        public List<SaleItem> Items { get; set; }

        public Sale()
        {
            DateTime = DateTime.Now;
            Items = new List<SaleItem>();
        }

        /// <summary>
        /// Satışa yeni ürün ekler
        /// </summary>
        public void AddItem(Product product, int quantity)
        {
            var existingItem = Items.Find(x => x.Barcode == product.Barcode);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.TotalPrice = existingItem.Quantity * existingItem.UnitPrice;
            }
            else
            {
                Items.Add(new SaleItem
                {
                    Barcode = product.Barcode,
                    Quantity = quantity,
                    UnitPrice = product.Price,
                    TotalPrice = product.Price * quantity
                });
            }
            CalculateTotal();
        }

        /// <summary>
        /// Satıştaki bir ürünün miktarını günceller
        /// </summary>
        public void UpdateItemQuantity(string barcode, int quantity)
        {
            var item = Items.Find(x => x.Barcode == barcode);
            if (item != null)
            {
                item.Quantity = quantity;
                item.TotalPrice = item.Quantity * item.UnitPrice;
                CalculateTotal();
            }
        }

        /// <summary>
        /// Satıştaki bir ürünü kaldırır
        /// </summary>
        public void RemoveItem(string barcode)
        {
            Items.RemoveAll(x => x.Barcode == barcode);
            CalculateTotal();
        }

        /// <summary>
        /// Toplam tutarı hesaplar
        /// </summary>
        private void CalculateTotal()
        {
            TotalPrice = 0;
            foreach (var item in Items)
            {
                TotalPrice += item.TotalPrice;
            }
        }
    }
} 