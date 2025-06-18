using System;

namespace MarketAutomation.Models
{
    /// <summary>
    /// Kullanıcı bilgilerini temsil eden model sınıfı
    /// </summary>
    public class User
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }

        public User()
        {
            CreatedAt = DateTime.Now;
        }

        /// <summary>
        /// Kullanıcının belirli bir role sahip olup olmadığını kontrol eder
        /// </summary>
        public bool HasRole(string role)
        {
            return Role.Equals(role, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Kullanıcının admin yetkisine sahip olup olmadığını kontrol eder
        /// </summary>
        public bool IsAdmin()
        {
            return HasRole("Admin");
        }

        /// <summary>
        /// Kullanıcının müdür yetkisine sahip olup olmadığını kontrol eder
        /// </summary>
        public bool IsManager()
        {
            return HasRole("Manager") || IsAdmin();
        }
    }
} 