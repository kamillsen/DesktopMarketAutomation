using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MarketAutomation.Utils
{
    /// <summary>
    /// Yardımcı fonksiyonları içeren sınıf
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Para birimini formatlar
        /// </summary>
        public static string FormatCurrency(decimal amount)
        {
            return amount.ToString("C2", new CultureInfo("tr-TR"));
        }

        /// <summary>
        /// Tarihi formatlar
        /// </summary>
        public static string FormatDate(DateTime date)
        {
            return date.ToString("dd.MM.yyyy HH:mm:ss", new CultureInfo("tr-TR"));
        }

        /// <summary>
        /// Barkod numarasının geçerli olup olmadığını kontrol eder
        /// </summary>
        public static bool IsValidBarcode(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode)) return false;

            // EAN-13 formatı kontrolü
            if (barcode.Length != 13) return false;

            // Sadece rakam kontrolü
            if (!Regex.IsMatch(barcode, @"^\d+$")) return false;

            // Kontrol hanesi hesaplama
            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                int digit = int.Parse(barcode[i].ToString());
                sum += (i % 2 == 0) ? digit : digit * 3;
            }

            int checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit == int.Parse(barcode[12].ToString());
        }

        /// <summary>
        /// Şifrenin güvenli olup olmadığını kontrol eder
        /// </summary>
        public static bool IsSecurePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;
            if (password.Length < 8) return false;

            // En az bir büyük harf
            if (!Regex.IsMatch(password, @"[A-Z]")) return false;

            // En az bir küçük harf
            if (!Regex.IsMatch(password, @"[a-z]")) return false;

            // En az bir rakam
            if (!Regex.IsMatch(password, @"\d")) return false;

            // En az bir özel karakter
            if (!Regex.IsMatch(password, @"[!@#$%^&*(),.?""':{}|<>]")) return false;

            return true;
        }

        /// <summary>
        /// Dosya adını güvenli hale getirir
        /// </summary>
        public static string SanitizeFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return string.Empty;

            // Geçersiz karakterleri kaldır
            var invalidChars = Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            var invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return Regex.Replace(fileName, invalidRegStr, "_");
        }

        /// <summary>
        /// Metni kısaltır ve üç nokta ekler
        /// </summary>
        public static string TruncateText(string text, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;
            if (text.Length <= maxLength) return text;

            return text.Substring(0, maxLength - 3) + "...";
        }
    }
} 