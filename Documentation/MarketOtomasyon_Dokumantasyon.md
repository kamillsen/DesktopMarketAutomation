
# 🧾 Gelişmiş Market Otomasyon Sistemi – Dokümantasyon

## 📌 Proje Tanımı

Bu sistem, küçükten büyüğe her ölçekteki marketlerin dijitalleştirilmiş stok ve satış operasyonlarını tek bir görsel arayüzden yönetmelerini sağlayan gelişmiş bir otomasyon sistemidir. Uygulama, C# WPF (.NET) ile geliştirilmiş olup, PostgreSQL veritabanı kullanılarak veri yönetimi sağlanmaktadır.

---

## 🎯 Temel Amaçlar

- Barkod destekli satış yönetimi
- Stok takibi ve kritik seviye uyarıları
- Kategorilere göre ürün filtreleme
- Günlük/aylık satış raporları
- Kullanıcı rolleri ve yetkilendirme
- POS cihazı (Hugin T300) ile entegre fiş yazdırma

---

## 🧰 Kullanılan Teknolojiler

- **Programlama Dili**: C# (.NET)
- **UI**: WPF (XAML)
- **Veritabanı**: PostgreSQL (pgAdmin)
- **Kütüphaneler**: Npgsql, System.Data, System.Windows

---

## 📂 Dosya Yapısı

```
MarketAutomation/
├── Documentation/
│   └── MarketOtomasyon_Dokumantasyon.md
├── Model/
│   ├── Log.cs
│   ├── Product.cs
│   ├── Sale.cs
│   ├── SaleItem.cs
│   └── User.cs
├── Services/
│   └── [Hizmet sınıflarını buraya ekleyin]
├── Utils/
│   ├── Config.cs
│   ├── Helpers.cs
│   └── POSDevice.cs
├── Views/
│   ├── StockWindow.xaml
│   ├── MainWindow.xaml
│   └── MainWindow.xaml.cs
├── App.xaml
├── App.config
└── packages.config
```

---

## 🧱 Katman Açıklamaları

### 📦 Model/
Uygulamadaki veritabanı tablolarını temsil eden veri sınıflarıdır.

- `Product.cs`: Ürün bilgilerini (barcode, name, stock, price) tutar.
- `Sale.cs`: Satış başlık verilerini içerir (tarih, toplam fiyat, kullanıcı).
- `SaleItem.cs`: Her satışa ait ürün detayları.
- `User.cs`: Kullanıcı bilgileri ve rol verileri.
- `Log.cs`: Kullanıcı aktiviteleri ve işlem logları.


### 🛠️ Services/
Uygulamanın iş mantığı ve veritabanı işlemlerini yöneten katmandır. Tüm işlevler `DatabaseService` sınıfı üzerinden veritabanı ile iletişim kurarak gerçekleştirilir.

- `DatabaseService.cs`: PostgreSQL bağlantısını yöneten temel sınıf. SQL sorguları çalıştırma, transaction yönetimi ve veri döndürme gibi işlemleri içerir.
- `ProductService.cs`: Ürün verilerini yönetir. Ürün ekleme, güncelleme, silme, stok güncelleme ve ürün listeleme işlemleri burada gerçekleştirilir.
- `SalesService.cs`: Satış işlemlerini yönetir. Yeni satış oluşturma, stok düşürme, satış detaylarını getirme ve raporlamaya yönelik tarih aralıklı satış listeleri burada yer alır.
- `AuthService.cs`: Kullanıcı giriş işlemleri, kullanıcı ekleme/silme, şifre ve rol güncelleme işlemlerini yönetir. Ayrıca kullanıcı listesi çekme işlevi ve (şu an kullanılmasa da) şifre hash fonksiyonu içerir.


---

## 🖨️ POS Cihazı Entegrasyonu (Hugin T300)

- `POSDevice.cs` sınıfı ile POS cihazı simülasyonu yapılır.
- ESC/POS komutları desteklenir.
- CP857 karakter setiyle Türkçe karakterler yazdırılabilir.
- SerialPort ile fiziksel bağlantı yapılabilir.

---

## 📝 Veritabanı Tabloları

### `product`
- barcode (PK), name, price, stock, category, description

### `sales`
- id, datetime, total_price, cashier_username

### `sale_items`
- id, sale_id, barcode, quantity, unit_price

### `users`
- username (PK), password_hash, role

### `logs`
- id, username, action, timestamp, details

---

## 📈 Geliştirme Aşamaları

1. Veritabanı kurulumu (`pgAdmin` üzerinden)
2. `Model` sınıflarının oluşturulması
3. `POSDevice` sınıfı ile POS desteği
4. `Views` klasöründe stok penceresi (`StockWindow.xaml`)
5. `MainWindow.xaml` ile genel pencere yönetimi
6. Giriş ve yetki mekanizması için `User.cs` + Auth servisleri (planlanıyor)

---

## 📋 Kullanım Senaryoları

- Barkod okut → Sepete ekle → Ödeme → Fiş yazdır → Stok güncelle
- Ürün ekle → Kritik stok seviyesi belirle
- Tarih aralığı seç → Rapor al → CSV dışa aktar
- Kullanıcı yetkisi → Giriş ekranı → Yetki bazlı erişim

---

## 🔐 Giriş Bilgileri (Varsayılan)

| Rol       | Kullanıcı Adı | Şifre  |
|-----------|----------------|--------|
| Admin     | admin          | 1234   |
| Müdür     | manager        | 1234   |
| Kasiyer   | cashier        | 1234   |

---

## 📤 Fiş Formatı Örneği

```
================================
        MARKET OTOMASYONU
   Satış ve Stok Yönetim Sistemi
================================

Fiş No: 1640995200000
Tarih: 31.12.2023 14:30
Kasiyer: admin
Ödeme: Nakit
--------------------------------
1. Coca Cola 330ml
   2 x 5,50 TL           11,00 TL
   Barkod: 8690637001031

================================
TOPLAM:                 14,25 TL

        Teşekkür ederiz!
        Tekrar bekleriz...
```

---

## 📎 Ek Belgeler

- `MarketOtomasyon_Dokumantasyon.me`: Bu döküman
- `App.config`: Uygulama yapılandırması
- `packages.config`: Harici bağımlılıklar
