# 📚 Ders Not Yönetim Sistemi

Bu proje, kullanıcıların kendi hesaplarıyla giriş yaparak ders notlarını dijital ortamda yönetebilecekleri modern bir web uygulamasıdır.

## 🎯 Proje Amacı

Ders Not Yönetim Sistemi, öğrencilerin ders notlarını organize etmelerine, dosyalarını güvenli şekilde saklamalarına ve ihtiyaç duyduklarında kolayca erişmelerine olanak tanır. Sistem, kullanıcı dostu arayüzü ve kapsamlı özellik seti ile etkili bir not yönetimi deneyimi sunar.

## ✨ Özellikler

### 🔐 Kullanıcı Yetkilendirme

- Güvenli giriş ve kayıt sistemi
- JWT tabanlı authentication
- Kullanıcıya özel veri erişimi

### 📝 Not Yönetimi

- **Not Ekleme**: Başlık, içerik ve dosya desteği
- **Not Listeleme**: Kullanıcıya özel not görüntüleme
- **Not Güncelleme**: Mevcut notları düzenleme
- **Not Silme**: Soft delete mekanizması

### 📎 Dosya Yönetimi

- PDF, Word, TXT ve resim dosyası desteği
- 5MB'a kadar dosya yükleme
- Güvenli dosya saklama ve indirme
- Dosya türü doğrulaması

### 🗂️ Arşiv Sistemi

- Soft delete ile güvenli silme
- Silinmiş notları arşivde saklama
- Arşivden geri yükleme özelliği
- Kalıcı silme (hard delete) seçeneği

### 📱 Modern Arayüz

- Responsive tasarım
- Tailwind CSS ile modern görünüm
- Kullanıcı dostu navigasyon
- Mobil uyumlu deneyim

## 🛠️ Kullanılan Teknolojiler

### Backend

- **ASP.NET Core 9.0** - Web API framework
- **Entity Framework Core** - ORM ve veritabanı yönetimi
- **SQL Server** - İlişkisel veritabanı
- **JWT Authentication** - Güvenli kimlik doğrulama
- **Microsoft.AspNetCore.Authentication.JwtBearer** - JWT middleware

### Frontend

- **Next.js 14** - React framework
- **React** - UI kütüphanesi
- **TypeScript** - Tip güvenliği
- **Tailwind CSS** - Utility-first CSS framework

### Geliştirme Araçları

- **Entity Framework Migrations** - Veritabanı şema yönetimi
- **Data Seeder** - Örnek veri oluşturma

## 📋 Sistem Gereksinimleri

- .NET 9.0 SDK
- Node.js 18+ ve npm
- SQL Server (LocalDB veya Express)
- Visual Studio 2022 veya VS Code (önerilen)

## 🚀 Kurulum ve Çalıştırma

### 1. Projeyi Klonlama

```bash
git clone [repository-url]
cd ders_not_yonetim
```

### 2. Backend Kurulumu

#### Veritabanı Bağlantısını Yapılandırma

`backend/BackendApi/appsettings.json` dosyasındaki connection string'i düzenleyin:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=DersNotYonetim;Integrated Security=true;TrustServerCertificate=true"
  }
}
```

#### Veritabanı Migration'ları

```bash
cd backend/BackendApi
dotnet ef database update
```

#### Backend'i Çalıştırma

```bash
cd backend/BackendApi
dotnet run
```

Backend varsayılan olarak `https://localhost:7000` ve `http://localhost:5000` portlarında çalışacaktır.

### 3. Frontend Kurulumu

#### Bağımlılıkları Yükleme

```bash
cd frontend
npm install
```

#### Frontend'i Çalıştırma

```bash
npm run dev
```

Frontend varsayılan olarak `http://localhost:3000` portunda çalışacaktır.

## 📡 API Uç Noktaları

### Authentication Endpoints

| Method | Endpoint             | Açıklama         |
| ------ | -------------------- | ---------------- |
| POST   | `/api/auth/login`    | Kullanıcı girişi |
| POST   | `/api/auth/register` | Kullanıcı kaydı  |

#### Login Request

```json
{
  "email": "kullanici@example.com",
  "sifre": "password123"
}
```

#### Register Request

```json
{
  "adi": "Ahmet",
  "soyadi": "Yılmaz",
  "email": "ahmet@example.com",
  "sifre": "password123"
}
```

### Not Yönetimi Endpoints

| Method | Endpoint        | Açıklama                       | Auth |
| ------ | --------------- | ------------------------------ | ---- |
| GET    | `/api/not`      | Kullanıcının notlarını listele | ✅   |
| GET    | `/api/not/{id}` | Belirli bir notu getir         | ✅   |
| POST   | `/api/not`      | Yeni not oluştur               | ✅   |
| PUT    | `/api/not/{id}` | Notu güncelle                  | ✅   |
| DELETE | `/api/not/{id}` | Notu sil (soft delete)         | ✅   |

#### Not Oluşturma Request

```json
{
  "baslik": "Matematik - Diferansiyel Hesap",
  "icerik": "Diferansiyel hesap konusu ve örnekleri..."
}
```

### Arşiv Endpoints

| Method | Endpoint                         | Açıklama                  | Auth |
| ------ | -------------------------------- | ------------------------- | ---- |
| GET    | `/api/not/arsiv`                 | Arşivdeki notları listele | ✅   |
| PUT    | `/api/not/arsiv/{id}/geri-yukle` | Notu arşivden geri yükle  | ✅   |
| DELETE | `/api/not/arsiv/{id}/kalici-sil` | Notu kalıcı olarak sil    | ✅   |

### Dosya Yönetimi Endpoints

| Method | Endpoint                     | Açıklama              | Auth |
| ------ | ---------------------------- | --------------------- | ---- |
| POST   | `/api/file/upload/{notId}`   | Nota dosya yükle      | ✅   |
| GET    | `/api/file/download/{notId}` | Notun dosyasını indir | ✅   |
| DELETE | `/api/file/delete/{notId}`   | Notun dosyasını sil   | ✅   |

## 🗃️ Veritabanı Şeması

### Kullanicilar Tablosu

```sql
CREATE TABLE Kullanicilar (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Adi nvarchar(100) NOT NULL,
    Soyadi nvarchar(100) NOT NULL,
    Email nvarchar(200) NOT NULL UNIQUE,
    Sifre nvarchar(max) NOT NULL,
    CreatedAt datetime2 DEFAULT GETDATE(),
    UpdatedAt datetime2 DEFAULT GETDATE()
);
```

### Notlar Tablosu

```sql
CREATE TABLE Notlar (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Baslik nvarchar(200) NOT NULL,
    Icerik nvarchar(max) NOT NULL,
    DosyaAdi nvarchar(500),
    DosyaYolu nvarchar(1000),
    DosyaBoyutu bigint,
    DosyaTuru nvarchar(100),
    OlusturmaTarihi datetime2 DEFAULT GETDATE(),
    GuncellemeTarihi datetime2 DEFAULT GETDATE(),
    Silindi bit DEFAULT 0,
    SilmeTarihi datetime2,
    KullaniciId int NOT NULL,
    FOREIGN KEY (KullaniciId) REFERENCES Kullanicilar(Id)
);
```

## 🔧 Konfigürasyon

### JWT Ayarları

`appsettings.json` dosyasında JWT ayarları:

```json
{
  "Jwt": {
    "Key": "SuperSecretKeyForJwtTokenGeneration123456789",
    "Issuer": "DersNotYonetimApp",
    "Audience": "DersNotYonetimApp",
    "ExpireMinutes": 60
  }
}
```

### CORS Ayarları

API, frontend'den gelen isteklere izin vermek için CORS yapılandırması içerir.

## 📊 Örnek Veriler

Sistem ilk çalıştırıldığında otomatik olarak örnek kullanıcılar ve notlar oluşturulur:

### Örnek Kullanıcılar

- **Ahmet Yılmaz** - ahmet@example.com (Şifre: 123456)
- **Ayşe Demir** - ayse@example.com (Şifre: 123456)
- **Mehmet Kaya** - mehmet@example.com (Şifre: 123456)

### Örnek Notlar

- Matematik, Fizik, Kimya dersleri için örnek notlar
- Soft delete örneği
- Farklı tarihlerle oluşturulmuş içerikler

## 🔒 Güvenlik

- **JWT Authentication**: Güvenli token tabanlı kimlik doğrulama
- **Authorization**: Kullanıcılar sadece kendi verilerine erişebilir
- **Dosya Doğrulama**: Yüklenen dosyaların türü ve boyutu kontrol edilir
- **SQL Injection Protection**: Entity Framework ile parametreli sorgular
- **HTTPS**: Production ortamında HTTPS zorunlu

## 🎨 UI/UX Özellikleri

- **Responsive Design**: Tüm cihazlarda uyumlu
- **Modern İkonlar**: Emoji tabanlı görsel öğeler
- **Loading States**: Kullanıcı geri bildirimi
- **Error Handling**: Anlaşılır hata mesajları
- **Confirmation Dialogs**: Kritik işlemler için onay
- **Date Formatting**: Türkçe tarih formatları

## 🚧 Gelecek Geliştirmeler

- [ ] Not kategorileri/etiketleri
- [ ] Gelişmiş arama ve filtreleme
- [ ] Notları paylaşma özelliği
- [ ] Dark mode desteği
- [ ] Mobil uygulama
- [ ] Çoklu dil desteği
- [ ] Export/Import özelliği

## 🤝 Katkıda Bulunma

1. Fork yapın
2. Feature branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Değişikliklerinizi commit edin (`git commit -m 'Add amazing feature'`)
4. Branch'e push yapın (`git push origin feature/amazing-feature`)
5. Pull Request açın

## 📝 Lisans

Bu proje eğitim amaçlı geliştirilmiştir.

---

**Not**: Bu proje .NET 9.0 ve Next.js 14 kullanarak geliştirilmiş modern bir web uygulamasıdır. Tüm özellikler test edilmiş ve production ortamına hazır haldedir.
