using System;
using System.Windows;
using MarketAutomation.Models;
using MarketAutomation.Services;

namespace MarketAutomation.Views
{
    public partial class AddProductWindow : Window
    {
        private readonly ProductService _productService;

        // Yapıcı metod: ProductService bağımlılığı dışarıdan alınır
        public AddProductWindow(ProductService productService)
        {
            InitializeComponent();
            _productService = productService;
        }

        // Kaydet butonuna tıklanınca çalışacak olay
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Form alanlarından alınan bilgilerle yeni ürün nesnesi oluşturulur
                var product = new Product
                {
                    Barcode = txtBarcode.Text.Trim(),
                    Name = txtName.Text.Trim(),
                    Price = decimal.Parse(txtPrice.Text),
                    Stock = int.Parse(txtStock.Text),
                    Category = txtCategory.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                // Veritabanına ürün eklenir
                _productService.Add(product);

                // Kullanıcıya bilgi mesajı gösterilir
                MessageBox.Show("Ürün başarıyla eklendi.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);

                // Pencere kapatılır
                this.Close();
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya bilgi verilir
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
