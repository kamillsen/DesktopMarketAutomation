using System;
using System.Configuration;

namespace MarketAutomation.Utils
{
    /// <summary>
    /// Uygulama ayarlarını yöneten sınıf
    /// </summary>
    public static class Config
    {
        // Veritabanı ayarları
        public static string DbHost => ConfigurationManager.AppSettings["DbHost"] ?? "localhost";
        public static int DbPort => int.Parse(ConfigurationManager.AppSettings["DbPort"] ?? "5432");
        public static string DbName => ConfigurationManager.AppSettings["DbName"] ?? "market_db";
        public static string DbUser => ConfigurationManager.AppSettings["DbUser"] ?? "postgres";
        public static string DbPassword => ConfigurationManager.AppSettings["DbPassword"] ?? "postgres";

        // POS yazıcı ayarları
        //public static string PosPort => ConfigurationManager.AppSettings["PosPort"] ?? "COM1";
        //public static int PosBaudRate => int.Parse(ConfigurationManager.AppSettings["PosBaudRate"] ?? "9600");

        // Barkod okuyucu ayarları
        public static string BarcodePort => ConfigurationManager.AppSettings["BarcodePort"] ?? "COM2";
        public static int BarcodeBaudRate => int.Parse(ConfigurationManager.AppSettings["BarcodeBaudRate"] ?? "9600");

        /// <summary>
        /// Veritabanı bağlantı dizesini oluşturur
        /// </summary>
        public static string GetConnectionString()
        {
            return $"Host={DbHost};Port={DbPort};Database={DbName};Username={DbUser};Password={DbPassword}";
        }

        /// <summary>
        /// Ayar değerini günceller
        /// </summary>
        public static void UpdateSetting(string key, string value)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] == null)
            {
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                config.AppSettings.Settings[key].Value = value;
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
} 