using BackendApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Data
{
    public static class DataSeeder
    {
        public static void SeedData(AppDbContext context)
        {
            // Veritabanının hazır olduğundan emin olun
            context.Database.EnsureCreated();

            // Kullanıcılar zaten varsa seeding yapmayın
            if (context.Kullanicilar.Any())
            {
                return;
            }

            // Örnek kullanıcılar
            var kullanicilar = new List<Kullanici>
            {
                new Kullanici
                {
                    Adi = "Ahmet",
                    Soyadi = "Yılmaz",
                    Email = "ahmet@example.com",
                    Sifre = "123456", // Gerçek projede hashlenmeli
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Kullanici
                {
                    Adi = "Ayşe",
                    Soyadi = "Demir",
                    Email = "ayse@example.com",
                    Sifre = "123456", // Gerçek projede hashlenmeli
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Kullanici
                {
                    Adi = "Mehmet",
                    Soyadi = "Kaya",
                    Email = "mehmet@example.com",
                    Sifre = "123456", // Gerçek projede hashlenmeli
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            };

            context.Kullanicilar.AddRange(kullanicilar);
            context.SaveChanges();

            // Örnek notlar
            var notlar = new List<Not>
            {
                new Not
                {
                    Baslik = "Matematik - Diferansiyel Hesap",
                    Icerik = "Diferansiyel hesap konusu ve örnekleri. Türev alma kuralları ve uygulamaları.",
                    KullaniciId = 1,
                    OlusturmaTarihi = DateTime.Now.AddDays(-5),
                    GuncellemeTarihi = DateTime.Now.AddDays(-5),
                    Silindi = false
                },
                new Not
                {
                    Baslik = "Fizik - Elektromanyetizma",
                    Icerik = "Elektromanyetik alanlar ve Maxwell denklemleri. Elektrik ve manyetik alan etkileşimleri.",
                    KullaniciId = 1,
                    OlusturmaTarihi = DateTime.Now.AddDays(-3),
                    GuncellemeTarihi = DateTime.Now.AddDays(-3),
                    Silindi = false
                },
                new Not
                {
                    Baslik = "Kimya - Organik Kimya",
                    Icerik = "Organik bileşikler ve reaksiyonları. Hidrokarbonlar ve fonksiyonel gruplar.",
                    KullaniciId = 1,
                    OlusturmaTarihi = DateTime.Now.AddDays(-1),
                    GuncellemeTarihi = DateTime.Now.AddDays(-1),
                    Silindi = false
                },
                new Not
                {
                    Baslik = "Bilgisayar Programlama - C#",
                    Icerik = "C# programlama dili temelleri. Veri tipleri, döngüler ve koşullu ifadeler.",
                    KullaniciId = 2,
                    OlusturmaTarihi = DateTime.Now.AddDays(-4),
                    GuncellemeTarihi = DateTime.Now.AddDays(-4),
                    Silindi = false
                },
                new Not
                {
                    Baslik = "Veritabanı Yönetimi - SQL",
                    Icerik = "SQL sorguları ve veritabanı tasarımı. JOIN işlemleri ve indeksleme.",
                    KullaniciId = 2,
                    OlusturmaTarihi = DateTime.Now.AddDays(-2),
                    GuncellemeTarihi = DateTime.Now.AddDays(-2),
                    Silindi = false
                },
                new Not
                {
                    Baslik = "Silinmiş Not Örneği",
                    Icerik = "Bu not soft delete örneği için silinmiş durumda.",
                    KullaniciId = 2,
                    OlusturmaTarihi = DateTime.Now.AddDays(-7),
                    GuncellemeTarihi = DateTime.Now.AddDays(-7),
                    Silindi = true,
                    SilmeTarihi = DateTime.Now.AddDays(-1)
                },
                new Not
                {
                    Baslik = "Tarih - Osmanlı İmparatorluğu",
                    Icerik = "Osmanlı İmparatorluğu'nun kuruluşu, gelişimi ve yıkılışı.",
                    KullaniciId = 3,
                    OlusturmaTarihi = DateTime.Now.AddDays(-6),
                    GuncellemeTarihi = DateTime.Now.AddDays(-6),
                    Silindi = false
                }
            };

            context.Notlar.AddRange(notlar);
            context.SaveChanges();
        }
    }
} 