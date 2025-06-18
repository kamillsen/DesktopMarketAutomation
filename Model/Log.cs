using System;

namespace MarketAutomation.Models
{
    /// <summary>
    /// Sistem loglarını temsil eden model sınıfı
    /// </summary>
    public class Log
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
        public string Details { get; set; }

        public Log()
        {
            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// Yeni bir log kaydı oluşturur
        /// </summary>
        public static Log Create(string username, string action, string details = null)
        {
            return new Log
            {
                Username = username,
                Action = action,
                Details = details,
                Timestamp = DateTime.Now
            };
        }
    }
} 