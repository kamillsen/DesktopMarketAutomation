using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using Npgsql;
using MarketAutomation.Models;

namespace MarketAutomation.Services
{
    /// <summary>
    /// Kimlik doğrulama ve yetkilendirme işlemlerini yöneten servis sınıfı
    /// </summary>
    public class AuthService
    {
        private readonly DatabaseService _db;

        public AuthService(DatabaseService db)
        {
            _db = db;
        }

        /// <summary>
        /// Kullanıcı girişi yapar
        /// </summary>
        public User Login(string username, string password)
        {
            var query = "SELECT * FROM users WHERE username = @username";
            var parameters = new[] { new NpgsqlParameter("@username", username) };
            
            var result = _db.ExecuteQuery(query, parameters);
            if (result.Rows.Count == 0) return null;

            var row = result.Rows[0];
            var storedHash = row["password_hash"].ToString();

            // Basit şifre kontrolü (gerçek uygulamada hash kullanılmalı)
            if (storedHash != password) return null;

            return new User
            {
                Username = row["username"].ToString(),
                PasswordHash = storedHash,
                Role = row["role"].ToString(),
                CreatedAt = Convert.ToDateTime(row["created_at"])
            };
        }

        /// <summary>
        /// Yeni kullanıcı ekler
        /// </summary>
        public void AddUser(User user)
        {
            var query = @"
                INSERT INTO users (username, password_hash, role)
                VALUES (@username, @password_hash, @role)";

            var parameters = new[]
            {
                new NpgsqlParameter("@username", user.Username),
                new NpgsqlParameter("@password_hash", user.PasswordHash),
                new NpgsqlParameter("@role", user.Role)
            };

            _db.ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Kullanıcı şifresini günceller
        /// </summary>
        public void UpdatePassword(string username, string newPassword)
        {
            var query = @"
                UPDATE users 
                SET password_hash = @password_hash
                WHERE username = @username";

            var parameters = new[]
            {
                new NpgsqlParameter("@username", username),
                new NpgsqlParameter("@password_hash", newPassword)
            };

            _db.ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Kullanıcı rolünü günceller
        /// </summary>
        public void UpdateRole(string username, string newRole)
        {
            var query = @"
                UPDATE users 
                SET role = @role
                WHERE username = @username";

            var parameters = new[]
            {
                new NpgsqlParameter("@username", username),
                new NpgsqlParameter("@role", newRole)
            };

            _db.ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Kullanıcıyı siler
        /// </summary>
        public void DeleteUser(string username)
        {
            var query = "DELETE FROM users WHERE username = @username";
            var parameters = new[] { new NpgsqlParameter("@username", username) };
            
            _db.ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Tüm kullanıcıları listeler
        /// </summary>
        public DataTable GetAllUsers()
        {
            var query = "SELECT username, role, created_at FROM users ORDER BY username";
            return _db.ExecuteQuery(query);
        }

        /// <summary>
        /// Şifreyi hash'ler (gerçek uygulamada kullanılmalı)
        /// </summary>
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
} 