using System.Collections.Generic;
using System.Windows;

using MarketAutomation.Models;
using MarketAutomation.Services;

namespace MarketAutomation.Views
{
    public partial class StockWindow : Window
    {
        private readonly ProductService _productService;

        public StockWindow(ProductService productService)
        {
            InitializeComponent();
            _productService = productService;
            LoadStock();
        }

        private void LoadStock()
        {
            List<Product> products = _productService.GetAll();
            dgStock.ItemsSource = products;
        }
    }
}
