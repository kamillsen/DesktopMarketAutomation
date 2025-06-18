//using System;
//using System.IO.Ports;
//using System.Text;
//using MarketAutomation.Models;

//namespace MarketAutomation.Utils
//{
//    /// <summary>
//    /// POS yazıcı entegrasyonunu yöneten sınıf
//    /// </summary>
//    public class POSDevice
//    {
//        private readonly SerialPort _serialPort;
//        private readonly string _portName;
//        private readonly int _baudRate;

//        public POSDevice(string portName, int baudRate = 9600)
//        {
//            _portName = portName;
//            _baudRate = baudRate;
//            _serialPort = new SerialPort(portName, baudRate);
//        }

//        /// <summary>
//        /// POS yazıcıya bağlanır
//        /// </summary>
//        public bool Connect()
//        {
//            try
//            {
//                if (!_serialPort.IsOpen)
//                {
//                    _serialPort.Open();
//                }
//                return true;
//            }
//            catch (Exception)
//            {
//                return false;
//            }
//        }

//        /// <summary>
//        /// POS yazıcıdan bağlantıyı keser
//        /// </summary>
//        public void Disconnect()
//        {
//            if (_serialPort.IsOpen)
//            {
//                _serialPort.Close();
//            }
//        }

//        /// <summary>
//        /// Satış fişi yazdırır
//        /// </summary>
//        public void PrintReceipt(Sale sale)
//        {
//            if (!_serialPort.IsOpen)
//            {
//                throw new InvalidOperationException("POS yazıcıya bağlı değil!");
//            }

//            var sb = new StringBuilder();

//            // Fiş başlığı
//            sb.AppendLine("MARKET OTOMASYON SİSTEMİ");
//            sb.AppendLine("------------------------");
//            sb.AppendLine($"Tarih: {sale.DateTime:dd.MM.yyyy HH:mm:ss}");
//            sb.AppendLine($"Kasiyer: {sale.CashierUsername}");
//            sb.AppendLine("------------------------");

//            // Ürünler
//            foreach (var item in sale.Items)
//            {
//                sb.AppendLine($"{item.ProductName}");
//                sb.AppendLine($"{item.Quantity} x {item.UnitPrice:C2} = {item.TotalPrice:C2}");
//            }

//            // Toplam
//            sb.AppendLine("------------------------");
//            sb.AppendLine($"Toplam: {sale.TotalPrice:C2}");
//            sb.AppendLine($"Ödeme: {sale.PaymentMethod}");
//            sb.AppendLine("------------------------");
//            sb.AppendLine("Bizi tercih ettiğiniz için");
//            sb.AppendLine("teşekkür ederiz!");
//            sb.AppendLine("------------------------");
//            sb.AppendLine("\n\n\n"); // Fiş kesme için boşluk

//            // ESC/POS komutları
//            var commands = new byte[]
//            {
//                0x1B, 0x40, // Initialize printer
//                0x1B, 0x21, 0x00, // Normal text
//                0x1B, 0x61, 0x01, // Center alignment
//            };

//            // Türkçe karakter desteği için CP857 kodlaması
//            var encoding = Encoding.GetEncoding(857);
//            var data = encoding.GetBytes(sb.ToString());

//            // Komutları ve veriyi gönder
//            _serialPort.Write(commands, 0, commands.Length);
//            _serialPort.Write(data, 0, data.Length);

//            // Fişi kes
//            var cutCommand = new byte[] { 0x1D, 0x56, 0x41 };
//            _serialPort.Write(cutCommand, 0, cutCommand.Length);
//        }

//        /// <summary>
//        /// Test yazdırma yapar
//        /// </summary>
//        public void PrintTest()
//        {
//            if (!_serialPort.IsOpen)
//            {
//                throw new InvalidOperationException("POS yazıcıya bağlı değil!");
//            }

//            var sb = new StringBuilder();
//            sb.AppendLine("POS YAZICI TEST");
//            sb.AppendLine("------------------------");
//            sb.AppendLine("Bu bir test yazdırmasıdır.");
//            sb.AppendLine("------------------------");
//            sb.AppendLine("\n\n\n");

//            var encoding = Encoding.GetEncoding(857);
//            var data = encoding.GetBytes(sb.ToString());

//            _serialPort.Write(data, 0, data.Length);

//            var cutCommand = new byte[] { 0x1D, 0x56, 0x41 };
//            _serialPort.Write(cutCommand, 0, cutCommand.Length);
//        }
//    }
//} 