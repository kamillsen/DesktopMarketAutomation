# 🧾 Market Otomasyon Sistemi – Detaylı Teknik Rehber

## 1. Proje Amacı ve Genel Bakış

Bu proje, marketlerde stok ve satış işlemlerini dijitalleştirmek, kullanıcı yetkilendirmesiyle güvenli ve hızlı bir yönetim sağlamak amacıyla geliştirilmiş bir **masaüstü otomasyon sistemidir**. Uygulama, **C# WPF (.NET)** ile yazılmıştır ve **PostgreSQL** veritabanı kullanır. POS cihazı entegrasyonu ve barkod desteğiyle gerçek dünyadaki market süreçlerini birebir simüle eder.

---

## 2. Klasör ve Dosya Yapısı

Aşağıda, projenin ana klasör ve dosyalarının işlevleri açıklanmıştır:

### 📁 Ana Klasörler

- **Model/**  
  Veritabanı tablolarını temsil eden veri sınıfları (entity/model).
- **Services/**  
  İş mantığı ve veritabanı işlemlerini yöneten servis katmanı.
- **Utils/**  
  Yardımcı fonksiyonlar, konfigürasyon ve donanım entegrasyonları.
- **Views/**  
  Kullanıcı arayüzü (UI) pencereleri ve bunların kod-behind dosyaları.
- **Documentation/**  
  Proje dokümantasyonu ve kullanım kılavuzları.
- **Properties/**  
  Uygulama kaynakları, ayarları ve derleme bilgileri.

### 📄 Temel Dosyalar

- **App.xaml / App.xaml.cs**  
  Uygulamanın giriş noktası ve genel ayarları.
- **App.config**  
  Veritabanı ve donanım bağlantı ayarları.
- **MarketAutomation.csproj / .sln**  
  Proje ve çözüm dosyaları.
- **packages.config**  
  Harici NuGet bağımlılıkları.

---

## 3. Klasörlerin Detaylı Açıklamaları

### Model/

- **Product.cs**: Ürün bilgileri (barkod, ad, fiyat, stok, kategori, açıklama, oluşturulma/güncellenme tarihi).
- **Sale.cs**: Satış başlığı (tarih, toplam tutar, kasiyer, ödeme yöntemi, satış kalemleri).
- **SaleItem.cs**: Satışa ait ürün detayları (barkod, miktar, birim fiyat, toplam fiyat, ürün adı).
- **User.cs**: Kullanıcı bilgileri (kullanıcı adı, şifre hash, rol, oluşturulma tarihi).
- **Log.cs**: Kullanıcı aktiviteleri ve sistem logları (kullanıcı, işlem, zaman, detay).

### Services/

- **DatabaseService.cs**:  
  Tüm veritabanı bağlantı ve sorgu işlemlerini yönetir.  
  - `ExecuteQuery`, `ExecuteNonQuery`, transaction yönetimi gibi metotlar içerir.
- **ProductService.cs**:  
  Ürün ekleme, güncelleme, silme, stok güncelleme ve listeleme işlemleri.
- **SalesService.cs**:  
  Satış oluşturma, satış detaylarını getirme, tarih aralığına göre raporlama.
- **AuthService.cs**:  
  Kullanıcı girişi, ekleme, silme, şifre ve rol güncelleme, kullanıcı listeleme.

### Utils/

- **Config.cs**:  
  Uygulama ayarlarını (veritabanı, POS, barkod okuyucu) okur ve günceller.
- **Helpers.cs**:  
  Para ve tarih formatlama, barkod ve şifre doğrulama, dosya adı temizleme gibi yardımcı fonksiyonlar.
- **POSDevice.cs**:  
  (Yorum satırında) POS yazıcı ile bağlantı ve fiş yazdırma işlemleri (ESC/POS komutları, CP857 Türkçe karakter desteği).

### Views/

Her pencere için iki dosya bulunur:  
- `.xaml`: Arayüz tasarımı  
- `.xaml.cs`: O pencerenin iş mantığı (event handler'lar, servis çağrıları)

**Önemli Pencereler:**
- **MainWindow**: Ana menü ve hızlı erişim butonları.
- **AddProductWindow**: Ürün ekleme formu.
- **NewSaleWindow**: Satış işlemleri ve sepet yönetimi.
- **UpdateStockWindow**: Stok güncelleme.
- **StockWindow**: Stok listesini görüntüleme.

### Properties/

- **Resources.resx**: Uygulama kaynakları (metinler, ikonlar, vs.).
- **Settings.settings**: Kullanıcı veya uygulama ayarları (şu an boş).
- **AssemblyInfo.cs**: Derleme bilgileri.

---

## 4. Konfigürasyon Dosyaları

### App.config

- **Veritabanı bağlantı ayarları**:  
  `DbHost`, `DbPort`, `DbName`, `DbUser`, `DbPassword`
- **Donanım bağlantı ayarları**:  
  `PosPort`, `PosBaudRate`, `BarcodePort`, `BarcodeBaudRate`
- **ConnectionStrings**:  
  PostgreSQL bağlantı dizesi.

### Config.cs

- App.config'teki ayarları okur ve günceller.
- `GetConnectionString()` ile dinamik bağlantı dizesi oluşturur.

---

## 5. Kullanılan Yazılım Mimarisi ve Tasarım Desenleri

- **Katmanlı Mimari**:  
  Model, Service, Utils ve View katmanları net şekilde ayrılmıştır.
- **Dependency Injection**:  
  Servisler, pencerelere dışarıdan parametre olarak verilir.
- **Transaction Yönetimi**:  
  Satış işlemleri ve kritik veritabanı işlemleri transaction ile güvence altına alınır.
- **Event-Driven UI**:  
  WPF'in event tabanlı yapısı kullanılır (buton tıklamaları, form submit, vs.).

---

## 6. UI Bileşenleri ve Rolleri

Her pencere, belirli bir işlevi yerine getirir:

- **MainWindow**:  
  Ana menü, hızlı işlemler, sol menüden modül seçimi.
- **AddProductWindow**:  
  Ürün ekleme formu. Gerekli alanlar: barkod, ad, fiyat, stok, kategori, açıklama.
- **NewSaleWindow**:  
  Barkod ile ürün ekleme, sepet yönetimi, ödeme türü seçimi, satış tamamlama.
- **UpdateStockWindow**:  
  Barkod ve miktar girerek stok artırma/azaltma.
- **StockWindow**:  
  Tüm ürünlerin stok durumunu tablo halinde gösterir.

**Props/Alanlar:**  
Her pencere, ilgili servisleri (ör. `ProductService`) parametre olarak alır ve form alanları üzerinden veri toplar.

---

## 7. İş Mantığı ve Servisler

- **Ürün Yönetimi**:  
  Ürün ekleme, güncelleme, silme, stok güncelleme işlemleri `ProductService` ile yapılır.
- **Satış Yönetimi**:  
  Sepet yönetimi, satış kaydı oluşturma, stok düşürme, satış detaylarını getirme işlemleri `SalesService` ile yapılır.
- **Kullanıcı Yönetimi**:  
  Giriş, kullanıcı ekleme/silme, şifre ve rol güncelleme işlemleri `AuthService` ile yapılır.
- **Loglama**:  
  Kullanıcı işlemleri ve sistem aktiviteleri `Log` modeli ile kaydedilebilir.

---

## 8. Kimlik Doğrulama ve Yetkilendirme

- **Giriş İşlemi**:  
  Kullanıcı adı ve şifre ile giriş yapılır. Şifreler hash'lenerek saklanmalıdır (örnek kodda basit kontrol var, gerçek projede `HashPassword` fonksiyonu kullanılmalı).
- **Rol Bazlı Yetkilendirme**:  
  Kullanıcılar; Admin, Müdür, Kasiyer gibi rollere sahiptir.  
  `User.cs` içinde rol kontrolü için yardımcı metotlar (`IsAdmin`, `IsManager`) bulunur.
- **Kullanıcı Yönetimi**:  
  Sadece yetkili kullanıcılar yeni kullanıcı ekleyebilir veya silebilir.

---

## 9. API ve Veritabanı Bağlantısı

- **Npgsql** kütüphanesi ile PostgreSQL'e bağlanılır.
- **DatabaseService** üzerinden tüm SQL sorguları ve transaction işlemleri yapılır.
- **Servisler** (ProductService, SalesService, AuthService) iş mantığını soyutlar ve doğrudan UI ile etkileşime girmez.

---

## 10. Yardımcı Fonksiyonlar ve Entegrasyonlar

- **Helpers.cs**:  
  - Para ve tarih formatlama
  - Barkod doğrulama (EAN-13)
  - Şifre güvenlik kontrolü
  - Dosya adı temizleme
  - Metin kısaltma
- **POSDevice.cs**:  
  - (Yorum satırında) ESC/POS komutları ile fiş yazdırma
  - CP857 Türkçe karakter desteği
  - SerialPort ile fiziksel bağlantı

---

## 11. En İyi Uygulamalar ve Genişletme İpuçları

- **Kodunuzu Katmanlara Ayırın**:  
  UI, iş mantığı ve veri erişimi ayrı dosyalarda olmalı.
- **Servisleri Parametre Olarak Verin**:  
  Test edilebilirlik ve bağımlılıkların yönetimi için servisleri pencereye dışarıdan verin.
- **Transaction Kullanın**:  
  Satış ve kritik işlemlerde veri tutarlılığı için transaction kullanın.
- **Yardımcı Fonksiyonları Tek Yerde Toplayın**:  
  Tekrarlı kodu `Helpers.cs` gibi dosyalarda toplayın.
- **Rol ve Yetki Kontrolü**:  
  Her işlemde kullanıcı rolünü kontrol edin.
- **Hataları Kullanıcıya Anlaşılır Şekilde Bildirin**:  
  Try-catch blokları ve `MessageBox` ile kullanıcıya bilgi verin.
- **Veritabanı Ayarlarını Dışarıdan Yönetin**:  
  `App.config` ve `Config.cs` ile ayarları kolayca değiştirilebilir yapın.
- **Şifreleri Hash'leyin**:  
  Gerçek projede şifreler asla düz metin olarak saklanmamalı.

---

## 12. Başlangıç Seviyesi İçin Tavsiyeler

- Kodun akışını anlamak için önce `MainWindow.xaml` ve ilgili `.cs` dosyasını inceleyin.
- Her pencerenin bir işlevi olduğunu ve ilgili servislerle çalıştığını unutmayın.
- Model, Service ve View katmanlarını ayrı ayrı anlamaya çalışın.
- Yardımcı fonksiyonları ve konfigürasyon dosyalarını değiştirerek sistemi özelleştirebilirsiniz.
- Geliştirme sırasında veritabanı bağlantı ayarlarını doğru girdiğinizden emin olun.

---

## 13. Sıkça Sorulan Sorular

**S: React veya web teknolojileri var mı?**  
Hayır, bu proje tamamen C# WPF (masaüstü) ile yazılmıştır.

**S: API bağlantısı nasıl yapılıyor?**  
Tüm işlemler doğrudan PostgreSQL veritabanına Npgsql ile yapılır, harici bir REST API yoktur.

**S: Şifreler güvenli mi?**  
Örnek kodda düz metin kontrolü var, gerçek projede `HashPassword` fonksiyonu kullanılmalı.

---

## 14. Ek Belgeler ve Kaynaklar

- `Documentation/MarketOtomasyon_Dokumantasyon.md`: Kısa dokümantasyon
- `App.config`: Uygulama yapılandırması
- `packages.config`: Harici bağımlılıklar

---

# Sonuç

Bu rehber, Market Otomasyon Sistemi'ni anlamanız, geliştirmeniz ve genişletmeniz için kapsamlı bir kaynak olarak hazırlanmıştır. Herhangi bir sorunuzda veya geliştirme ihtiyacınızda bu belgeye başvurabilirsiniz.

---

**Not:**  
Bu belgeyi `MarketAutomation/EXPLANATION_GUIDE.md` olarak kaydedebilirsiniz. Geliştirme sırasında güncel tutmanız önerilir. 