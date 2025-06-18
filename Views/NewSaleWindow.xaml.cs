using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MarketAutomation.Models;
using MarketAutomation.Services;

namespace MarketAutomation.Views
{
    public partial class NewSaleWindow : Window
    {
        private readonly ProductService _productService;
        private readonly SalesService _salesService;

        // 🔹 SEPETİ burada tutuyoruz
        private List<SaleItem> _cart = new List<SaleItem>(); // ✅ C# 7.3 uyumlu


        public NewSaleWindow(ProductService productService, SalesService salesService)
        {
            InitializeComponent();
            _productService = productService;
            _salesService = salesService;
        }

        // 🔹 EKLE butonuna tıklanınca çalışan METOT
        private void BtnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            string barcode = txtBarcode.Text.Trim();

            if (string.IsNullOrEmpty(barcode))
            {
                MessageBox.Show("Lütfen barkod giriniz.");
                return;
            }

            var product = _productService.GetByBarcode(barcode);
            if (product == null)
            {
                MessageBox.Show("Ürün bulunamadı!");
                return;
            }

            var existing = _cart.FirstOrDefault(p => p.Barcode == barcode);
            if (existing != null)
            {
                existing.Quantity++;
                existing.TotalPrice = existing.Quantity * existing.UnitPrice;
            }
            else
            {
                _cart.Add(new SaleItem
                {
                    Barcode = product.Barcode,
                    ProductName = product.Name,
                    Quantity = 1,
                    UnitPrice = product.Price,
                    TotalPrice = product.Price
                });
            }

            UpdateCart();
            txtBarcode.Clear();
        }

        // 🔹 DataGrid ve toplamı günceller
        private void UpdateCart()
        {
            dgCart.ItemsSource = null;
            dgCart.ItemsSource = _cart;

            txtTotal.Text = _cart.Sum(i => i.TotalPrice).ToString("0.00") + " TL";
        }

        // 🔹 SATIŞI TAMAMLA butonuna basılınca çalışır
        private void BtnCompleteSale_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_cart.Count == 0)
                {
                    MessageBox.Show("Sepet boş.");
                    return;
                }

                // ComboBox'tan seçilen ödeme türünü al
                var selectedMethod = (cmbPaymentMethod.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Cash";

                var sale = new Sale
                {
                    DateTime = DateTime.Now,
                    TotalPrice = _cart.Sum(i => i.TotalPrice),
                    CashierUsername = "admin",
                    PaymentMethod = selectedMethod, // ✅ CHECK constraint ile uyumlu
                    Items = _cart
                };

                _salesService.CreateSale(sale);
                MessageBox.Show("Satış başarıyla tamamlandı.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"HATA: {ex.Message}", "Uygulama Hatası", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgCart_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Düzenleme bitince toplamı güncelle
            Dispatcher.Invoke(() => UpdateCart());
        }


    }
}
