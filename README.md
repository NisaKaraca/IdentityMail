# IdentityMail

IdentityMail; kullanıcıların güvenli şekilde hesap oluşturabildiği, birbirlerine mesaj gönderebildiği ve gelen kutularını yönetebildiği, ASP.NET Core MVC tabanlı dinamik bir mesajlaşma uygulamasıdır.

Uygulamada kullanıcı panelinin yanı sıra kullanıcıların, mesajların, kategorilerin ve şikâyetlerin yönetilebildiği kapsamlı bir Admin Paneli bulunmaktadır.

## Proje Hakkında

Bu proje, ASP.NET Core Identity altyapısını ve gerçek bir mesajlaşma sisteminde ihtiyaç duyulabilecek temel işlevleri uygulamak amacıyla geliştirilmiştir.

Kullanıcılar sistem içerisinde:

- Hesap oluşturabilir ve giriş yapabilir.
- Diğer kullanıcılara mesaj gönderebilir.
- Gelen ve gönderilen mesajlarını görüntüleyebilir.
- Mesajları önemli olarak işaretleyebilir.
- Mesajları çöp kutusuna taşıyabilir veya geri yükleyebilir.
- Mesajlarını kategorilere ayırabilir.
- Uygunsuz mesajları yönetime şikâyet edebilir.
- Taslak oluşturabilir ve daha sonra düzenleyebilir.

Admin kullanıcıları ise sistem genelindeki kullanıcıları, mesajları, kategorileri ve şikâyetleri yönetebilir.

## Uygulama Görselleri

### Giriş Ekranı

<img width="1919" height="984" alt="image" src="https://github.com/user-attachments/assets/27859aeb-aa7a-435b-8b78-3376e7d1de66" />

### Kayıt Ekranı

<img width="1900" height="988" alt="image (1)" src="https://github.com/user-attachments/assets/8069aa95-46b1-401e-a91f-8812ba59e5da" />

### Gelen Kutusu

<img width="1912" height="978" alt="image (2)" src="https://github.com/user-attachments/assets/dff08e9c-0685-4108-8572-e39d7fc7a104" />

### Yeni Mesaj Oluşturma

<img width="1894" height="988" alt="image" src="https://github.com/user-attachments/assets/e0627e49-fe5b-484b-9ee2-810812d6cf70" />

### Mesaj Detayı

<img width="1918" height="982" alt="image" src="https://github.com/user-attachments/assets/24ea5514-0af2-4029-92d7-16c009b6cd1d" />

### Admin Dashboard

![Admin Dashboard](screenshots/admin-dashboard.png)

### Admin Kullanıcı Yönetimi

<img width="1895" height="983" alt="image (10)" src="https://github.com/user-attachments/assets/50b85873-a8cf-4b43-a58f-91ff4600f52b" />

### Admin Mesaj Yönetimi

<img width="1895" height="978" alt="image (12)" src="https://github.com/user-attachments/assets/9ca641e0-0f05-47a8-a0fb-d9444a047296" />

### Admin Kategori Yönetimi

<img width="1897" height="985" alt="image (13)" src="https://github.com/user-attachments/assets/47ed1036-b98c-4649-ae79-a22a17ba1afd" />

### Admin Şikâyet Yönetimi

<img width="1919" height="981" alt="image (16)" src="https://github.com/user-attachments/assets/843603c1-d444-471b-b4d2-4c28ffb4f861" />

---

## Özellikler

### Kimlik Doğrulama ve Kullanıcı Yönetimi

- Kullanıcı kayıt ve giriş işlemleri
- ASP.NET Core Identity altyapısı
- Benzersiz e-posta ve kullanıcı adı kontrolü
- Özel Identity hata mesajları
- Rol tabanlı yetkilendirme
- Admin ve normal kullanıcı ayrımı
- Kullanıcı hesabını aktif veya pasif hâle getirme
- Pasif kullanıcıların sisteme girişini engelleme
- Kullanıcı profil bilgilerini güncelleme
- Güvenli şifre değiştirme

### Mesaj Yönetimi

- Yeni mesaj gönderme
- Gelen mesajları listeleme
- Gönderilen mesajları listeleme
- Mesaj detaylarını görüntüleme
- Okundu ve okunmadı durumu
- Mesajı önemli olarak işaretleme
- Önemli mesajları ayrı listeleme
- Mesajları çöp kutusuna taşıma
- Çöp kutusundan geri yükleme
- Mesajı kalıcı olarak silme
- Gönderen ve alıcı için bağımsız silme durumu

### Taslak Yönetimi

- Mesajı taslak olarak kaydetme
- Taslakları listeleme
- Taslağı düzenleyerek mesaja dönüştürme
- Kullanıcıya özel taslak yönetimi
- Taslak silme

### Kategori Yönetimi

- Admin tarafından kategori oluşturma
- Kategori adı, açıklaması, rengi ve ikonu belirleme
- Kategori bilgilerini güncelleme
- Kategoriyi aktif veya pasif hâle getirme
- Kullanıcıların mesajlarına kategori ataması
- Kullanıcıya özel mesaj-kategori ilişkisi
- Kategori kullanım sayılarının hesaplanması
- En çok kullanılan kategorilerin Dashboard üzerinde gösterilmesi

### Şikâyet Sistemi

- Gelen mesajları şikâyet etme
- Spam, taciz, uygunsuz içerik ve dolandırıcılık nedenleri
- Şikâyete açıklama ekleyebilme
- Aynı kullanıcının aynı mesajı tekrar şikâyet etmesinin engellenmesi
- Admin tarafından şikâyetleri listeleme
- Bekleyen ve çözülen şikâyetleri filtreleme
- Şikâyet detaylarını inceleme
- Şikâyeti çözüldü olarak işaretleme
- Şikâyeti yeniden incelemeye alma

### Admin Dashboard

- Toplam kullanıcı sayısı
- Aktif kullanıcı sayısı
- Toplam mesaj sayısı
- Bugün gönderilen mesaj sayısı
- Okunmamış mesaj sayısı
- Çöp kutusundaki mesaj sayısı
- En fazla mesaj gönderen kullanıcılar
- En çok kullanılan kategoriler
- Bekleyen şikâyet sayısı
- Sistem geneli mesaj istatistikleri

### Admin Paneli

- Rol tabanlı güvenli Admin erişimi
- Dinamik Dashboard
- Kullanıcı listeleme ve arama
- Kullanıcı aktif/pasif yönetimi
- Sistem mesajlarını listeleme
- Mesajlarda arama ve durum filtreleme
- Mesaj detaylarını inceleme
- Kategori CRUD işlemleri
- Şikâyet yönetimi
- Admin profil ve şifre ayarları

---

## Kullanılan Teknolojiler

- ASP.NET Core 8.0 MVC
- ASP.NET Core Identity
- Entity Framework Core
- Microsoft SQL Server
- Code First Yaklaşımı
- LINQ
- Razor View Engine
- ViewComponent
- DTO Katmanı
- Dependency Injection
- Role-Based Authorization
- Cookie Authentication
- Tailwind CSS
- JavaScript
- Material Symbols
- HTML5 & CSS3
- Git & GitHub

## Kurulum

### 1. Repoyu klonlayın

```bash
git clone https://github.com/NisaKaraca/IdentityMail.git
```

### 2. Proje dizinine geçin

```bash
cd IdentityMail
```

### 3. Bağlantı dizesini düzenleyin

`IdentityMail.Web/appsettings.json` dosyasında SQL Server bağlantınızı tanımlayın:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=IdentityMailDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 4. NuGet paketlerini yükleyin

```bash
dotnet restore
```

### 5. Veritabanını oluşturun

```bash
dotnet ef database update --project IdentityMail.Web
```

### 6. Uygulamayı çalıştırın

```bash
dotnet run --project IdentityMail.Web
```

---

## Yetkilendirme Yapısı

Uygulama iki temel role sahiptir:

| Rol | Yetkiler |
|---|---|
| Kullanıcı | Mesaj gönderme, gelen kutusu, taslak, kategori ve şikâyet işlemleri |
| Admin | Dashboard, kullanıcı, mesaj, kategori ve şikâyet yönetimi |

Admin Dashboard controller’ları şu yetkilendirme ile korunmaktadır:

```csharp
[Authorize(Roles = "Admin")]
```

Admin rolü ve geliştirme ortamındaki demo yönetici hesabı uygulama başlangıcında seed işlemiyle oluşturulmaktadır.

> Güvenlik nedeniyle gerçek ortamlarda yönetici parolası kaynak kodunda tutulmamalıdır. User Secrets veya environment variable kullanılmalıdır.

---

## Dashboard Hesaplamaları

Dashboard verileri Entity Framework Core ve LINQ sorguları kullanılarak dinamik olarak hesaplanmaktadır:

- Aktif kullanıcılar `IsActive` alanına göre
- Bugünün mesajları `SendTime` tarihine göre
- Okunmamış mesajlar `IsRead` alanına göre
- Çöp kutusu mesajları kullanıcı tarafındaki silme alanlarına göre
- En aktif gönderenler `SenderId` üzerinden gruplandırılarak
- Kategori istatistikleri `UserMessageCategory` kayıtlarına göre
- Bekleyen şikâyetler `IsResolved` durumuna göre

---

## Güvenlik

Projede uygulanan bazı güvenlik önlemleri:

- Rol tabanlı yetkilendirme
- Anti-forgery token doğrulaması
- Identity parola hashleme
- Kullanıcıya özel mesaj erişim kontrolü
- Kullanıcıya özel taslak erişim kontrolü
- Başkasına ait mesajlara doğrudan erişimin engellenmesi
- Aynı mesajın tekrar şikâyet edilmesinin engellenmesi
- Pasif kullanıcı giriş kontrolü
- Kalıcı silme öncesi sahiplik doğrulaması

---

## Projenin Amacı

Bu proje ile aşağıdaki konularda pratik yapılmıştır:

- ASP.NET Core MVC mimarisi
- ASP.NET Core Identity kullanımı
- Rol ve yetkilendirme yönetimi
- Entity Framework Core ilişkileri
- Code First ve Migration işlemleri
- DTO kullanımı
- LINQ ile dinamik sorgular
- ViewComponent tabanlı ortak arayüzler
- Gerçek hayata yakın mesajlaşma iş akışları
- Admin paneli geliştirme
- Responsive kullanıcı arayüzü

---

## Geliştirici

**Nisa Karaca**

- Yönetim Bilişim Sistemleri
- .NET Developer
- GitHub: [NisaKaraca](https://github.com/NisaKaraca)

---

## ⭐ Destek

Projeyi faydalı bulduysanız GitHub üzerinden yıldız vererek destek olabilirsiniz.
