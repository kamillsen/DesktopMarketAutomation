using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using MarketAutomation.Models;
using System.Security.Cryptography;
using System.Windows;

namespace MarketAutomation.Services
{
    /// <summary>
    /// Satış işlemlerini yöneten servis sınıfı
    /// </summary>
    public class SalesService
    {
        private readonly DatabaseService _db;
        private readonly ProductService _productService;

        public SalesService(DatabaseService db, ProductService productService)
        {
            _db = db;
            _productService = productService;
        }

        /// <summary>
        /// Yeni satış kaydı oluşturur
        /// </summary>
        public int CreateSale(Sale sale)
        {
            return _db.ExecuteInTransaction((connection, transaction) =>
            {
                // 1. Satış başlığı oluştur
                var saleQuery = @"
            INSERT INTO sales (datetime, total_price, cashier_username, payment_method)
            VALUES (@datetime, @total_price, @cashier_username, @payment_method)
            RETURNING id";

                var saleParameters = new[]
                {
            new NpgsqlParameter("@datetime", sale.DateTime),
            new NpgsqlParameter("@total_price", sale.TotalPrice),
            new NpgsqlParameter("@cashier_username", sale.CashierUsername),
            new NpgsqlParameter("@payment_method", sale.PaymentMethod)
        };

                int saleId;
                using (var command = new NpgsqlCommand(saleQuery, connection, transaction))
                {
                    command.Parameters.AddRange(saleParameters);
                    saleId = Convert.ToInt32(command.ExecuteScalar());
                }

                // 2. Her ürün için satış detayı ve stok güncelleme
                foreach (var item in sale.Items)
                {
                    // 2.1 Satış detay satırını ekle
                    var itemQuery = @"
                INSERT INTO sale_items (sale_id, barcode, quantity, unit_price, total_price)
                VALUES (@sale_id, @barcode, @quantity, @unit_price, @total_price)";

                    var itemParameters = new[]
                    {
                new NpgsqlParameter("@sale_id", saleId),
                new NpgsqlParameter("@barcode", item.Barcode),
                new NpgsqlParameter("@quantity", item.Quantity),
                new NpgsqlParameter("@unit_price", item.UnitPrice),
                new NpgsqlParameter("@total_price", item.TotalPrice)
            };

                    using (var itemCommand = new NpgsqlCommand(itemQuery, connection, transaction))
                    {
                        itemCommand.Parameters.AddRange(itemParameters);
                        itemCommand.ExecuteNonQuery();
                    }

                    // 2.2 Stok güncelle
                    try
                    {
                        _productService.UpdateStock(item.Barcode, -item.Quantity);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Stok güncellenemedi: {item.Barcode}\n{ex.Message}", "Stok Hatası", MessageBoxButton.OK, MessageBoxImage.Warning);
                        // Hata yönetimi burada devam edebilir (örneğin rollback için throw)
                    }
                }

                return saleId;
            });
        }


        /// <summary>
        /// Satış detaylarını getirir
        /// </summary>
        public Sale GetSaleDetails(int saleId)
        {
            var query = @"
                SELECT s.*, si.*, p.name as product_name
                FROM sales s
                JOIN sale_items si ON s.id = si.sale_id
                JOIN products p ON si.barcode = p.barcode
                WHERE s.id = @sale_id";

            var parameters = new[] { new NpgsqlParameter("@sale_id", saleId) };
            var result = _db.ExecuteQuery(query, parameters);

            if (result.Rows.Count == 0) return null;

            var sale = new Sale
            {
                Id = saleId,
                DateTime = Convert.ToDateTime(result.Rows[0]["datetime"]),
                TotalPrice = Convert.ToDecimal(result.Rows[0]["total_price"]),
                CashierUsername = result.Rows[0]["cashier_username"].ToString(),
                PaymentMethod = result.Rows[0]["payment_method"].ToString(),
                Items = new List<SaleItem>()
            };

            foreach (DataRow row in result.Rows)
            {
                sale.Items.Add(new SaleItem
                {
                    Id = Convert.ToInt32(row["id"]),
                    SaleId = saleId,
                    Barcode = row["barcode"].ToString(),
                    Quantity = Convert.ToInt32(row["quantity"]),
                    UnitPrice = Convert.ToDecimal(row["unit_price"]),
                    TotalPrice = Convert.ToDecimal(row["total_price"]),
                    ProductName = row["product_name"].ToString()
                });
            }

            return sale;
        }

        /// <summary>
        /// Tarih aralığına göre satışları listeler
        /// </summary>
        public List<Sale> GetSalesByDateRange(DateTime startDate, DateTime endDate)
        {
            var query = @"
                SELECT DISTINCT s.*
                FROM sales s
                WHERE s.datetime BETWEEN @start_date AND @end_date
                ORDER BY s.datetime DESC";

            var parameters = new[]
            {
                new NpgsqlParameter("@start_date", startDate),
                new NpgsqlParameter("@end_date", endDate)
            };

            var result = _db.ExecuteQuery(query, parameters);
            var sales = new List<Sale>();

            foreach (DataRow row in result.Rows)
            {
                var saleId = Convert.ToInt32(row["id"]);
                sales.Add(GetSaleDetails(saleId));
            }

            return sales;
        }
    }
} 