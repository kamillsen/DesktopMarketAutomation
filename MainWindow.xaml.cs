using System;
using System.Windows;
using MarketAutomation.Services;
using MarketAutomation.Utils;

namespace MarketAutomation
{
    public partial class MainWindow : Window
    {
        private readonly DatabaseService _db;
        private readonly ProductService _productService;
        private readonly SalesService _salesService;
        private readonly AuthService _authService;
       // private readonly POSDevice _posDevice;

        public MainWindow()
        {
            InitializeComponent();

            // Servisleri başlat
            _db = new DatabaseService(Config.GetConnectionString());
            _productService = new ProductService(_db);
            _salesService = new SalesService(_db, _productService);
            _authService = new AuthService(_db);
            //_posDevice = new POSDevice(Config.PosPort, Config.PosBaudRate);

            // Veritabanı bağlantısını kontrol et
            if (!_db.TestConnection())
            {
                MessageBox.Show("Veritabanı bağlantısı kurulamadı!", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }

            // POS yazıcıyı başlat
            //if (!_posDevice.Connect())
            //{
            //    MessageBox.Show("POS yazıcıya bağlanılamadı!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
            //}
        }

        #region Menü Olayları

        private void NewSale_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Yeni satış penceresini aç
            var saleWindow = new Views.NewSaleWindow(_productService, _salesService);
            saleWindow.ShowDialog();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Ürün ekleme penceresini aç
        }

        private void UpdateStock_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Stok güncelleme penceresini aç
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Çıkış yapmak istediğinize emin misiniz?", "Çıkış",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void DailyReport_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Günlük rapor penceresini aç
        }

        private void WeeklyReport_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Haftalık rapor penceresini aç
        }

        private void MonthlyReport_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Aylık rapor penceresini aç
        }

        private void UserManagement_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Kullanıcı yönetimi penceresini aç
        }

        private void SystemSettings_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Sistem ayarları penceresini aç
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Yardım penceresini aç
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Market Otomasyon Sistemi v1.0\n\n" +
                "© 2024 Tüm hakları saklıdır.",
                "Hakkında",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        #endregion

        #region Sol Menü Olayları

        private void Sales_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Satış işlemleri penceresini aç
        }

        private void Products_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Ürün yönetimi penceresini aç
        }

        private void Stock_Click(object sender, RoutedEventArgs e)
        {
            var stockWindow = new Views.StockWindow(_productService);
            stockWindow.ShowDialog();
        }

        private void Reports_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Raporlar penceresini aç
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Kullanıcılar penceresini aç
        }

        #endregion

        #region Hızlı İşlem Olayları

        private void QuickSale_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Hızlı satış penceresini aç
            var saleWindow = new Views.NewSaleWindow(_productService, _salesService);
            saleWindow.ShowDialog();
        }

        private void QuickAddProduct_Click(object sender, RoutedEventArgs e)
        {
            // Ürün ekleme penceresini oluştur (ProductService örneği ile birlikte)
            var addProductWindow = new Views.AddProductWindow(_productService);

            // Pencereyi modal (ana pencereyi kilitleyerek) olarak göster
            addProductWindow.ShowDialog();
        }


        private void QuickUpdateStock_Click(object sender, RoutedEventArgs e)
        {
            // Hızlı stok güncelleme penceresini başlat
            var updateStockWindow = new Views.UpdateStockWindow(_productService);

            // Modal olarak göster (diğer pencereler kilitlenir)
            updateStockWindow.ShowDialog();
        }




        #endregion

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
          //  _posDevice.Disconnect();
        }
    }
}