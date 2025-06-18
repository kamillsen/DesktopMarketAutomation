using System;
using System.Windows;
using MarketAutomation.Services;

namespace MarketAutomation.Views
{
    /// <summary>
    /// Stok güncelleme işlemlerini gerçekleştiren pencere.
    /// Kullanıcı barkod ve miktar girerek mevcut ürün stoğunu güncelleyebilir.
    /// </summary>
    public partial class UpdateStockWindow : Window
    {
        // Ürün işlemleri için gerekli servis
        private readonly ProductService _productService;

        /// <summary>
        /// Constructor - pencere başlatılırken gerekli servis içeri alınır
        /// </summary>
        /// <param name="productService">Veritabanıyla ürün işlemlerini yöneten servis</param>
        public UpdateStockWindow(ProductService productService)
        {
            InitializeComponent();
            _productService = productService;
        }

        /// <summary>
        /// "Stok Güncelle" butonuna tıklanınca çalışır.
        /// Barkoda ait ürünün stoğu verilen miktar kadar artırılır veya azaltılır.
        /// </summary>
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Kullanıcıdan alınan barkod ve miktar bilgisi
                string barcode = txtBarcode.Text.Trim();
                int quantity = int.Parse(txtQuantity.Text); // e.g. +10 veya -5

                // Veritabanında stok güncelleme işlemi yapılır
                _productService.UpdateStock(barcode, quantity);

                // Başarılı işlem bildirimi
                MessageBox.Show("Stok başarıyla güncellendi.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);

                // Pencereyi kapat
                this.Close();
            }
            catch (Exception ex)
            {
                // Hatalı giriş veya sistem hatasında kullanıcıya mesaj gösterilir
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
