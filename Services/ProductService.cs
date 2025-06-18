using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using MarketAutomation.Models;
using System.Windows;

namespace MarketAutomation.Services
{
    /// <summary>
    /// Ürün işlemlerini yöneten servis sınıfı
    /// </summary>
    public class ProductService
    {
        private readonly DatabaseService _db;

        public ProductService(DatabaseService db)
        {
            _db = db;
        }

        /// <summary>
        /// Barkod numarasına göre ürün getirir
        /// </summary>
        public Product GetByBarcode(string barcode)
        {
            var query = "SELECT * FROM products WHERE barcode = @barcode";
            var parameters = new[] { new NpgsqlParameter("@barcode", barcode) };
            
            var result = _db.ExecuteQuery(query, parameters);
            if (result.Rows.Count == 0) return null;

            return MapToProduct(result.Rows[0]);
        }

        /// <summary>
        /// Tüm ürünleri listeler
        /// </summary>
        public List<Product> GetAll()
        {
            var query = "SELECT * FROM products ORDER BY name";
            var result = _db.ExecuteQuery(query);
            
            var products = new List<Product>();
            foreach (DataRow row in result.Rows)
            {
                products.Add(MapToProduct(row));
            }
            return products;
        }

        /// <summary>
        /// Yeni ürün ekler
        /// </summary>
        public void Add(Product product)
        {
            var query = @"
                INSERT INTO products (barcode, name, price, stock, category, description)
                VALUES (@barcode, @name, @price, @stock, @category, @description)";

            var parameters = new[]
            {
                new NpgsqlParameter("@barcode", product.Barcode),
                new NpgsqlParameter("@name", product.Name),
                new NpgsqlParameter("@price", product.Price),
                new NpgsqlParameter("@stock", product.Stock),
                new NpgsqlParameter("@category", (object)product.Category ?? DBNull.Value),
                new NpgsqlParameter("@description", (object)product.Description ?? DBNull.Value)
            };

            _db.ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Ürün bilgilerini günceller
        /// </summary>
        public void Update(Product product)
        {
            var query = @"
                UPDATE products 
                SET name = @name, price = @price, stock = @stock, 
                    category = @category, description = @description
                WHERE barcode = @barcode";

            var parameters = new[]
            {
                new NpgsqlParameter("@barcode", product.Barcode),
                new NpgsqlParameter("@name", product.Name),
                new NpgsqlParameter("@price", product.Price),
                new NpgsqlParameter("@stock", product.Stock),
                new NpgsqlParameter("@category", (object)product.Category ?? DBNull.Value),
                new NpgsqlParameter("@description", (object)product.Description ?? DBNull.Value)
            };

            _db.ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Ürün stoğunu günceller
        /// </summary>
        public void UpdateStock(string barcode, int quantity)
        {
            var query = @"
                UPDATE products 
                SET stock = stock + @quantity
                WHERE barcode = @barcode";

            var parameters = new[]
            {
                new NpgsqlParameter("@barcode", barcode),
                new NpgsqlParameter("@quantity", quantity)
            };

            int result = _db.ExecuteNonQuery(query, parameters);
            if (result == 0)
            {
                MessageBox.Show($"Stok güncellenmedi! Barkod: {barcode}", "Uyarı");
            }
        }

        /// <summary>
        /// Ürünü siler
        /// </summary>
        public void Delete(string barcode)
        {
            var query = "DELETE FROM products WHERE barcode = @barcode";
            var parameters = new[] { new NpgsqlParameter("@barcode", barcode) };
            
            _db.ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// DataRow'u Product nesnesine dönüştürür
        /// </summary>
        private Product MapToProduct(DataRow row)
        {
            return new Product
            {
                Barcode = row["barcode"].ToString(),
                Name = row["name"].ToString(),
                Price = Convert.ToDecimal(row["price"]),
                Stock = Convert.ToInt32(row["stock"]),
                Category = row["category"] == DBNull.Value ? null : row["category"].ToString(),
                Description = row["description"] == DBNull.Value ? null : row["description"].ToString(),
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                UpdatedAt = Convert.ToDateTime(row["updated_at"])
            };
        }
    }
} 