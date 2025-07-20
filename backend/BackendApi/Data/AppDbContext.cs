using Microsoft.EntityFrameworkCore;
using BackendApi.Models;

namespace BackendApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Not> Notlar { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Kullanici tablosu konfigürasyonu
            modelBuilder.Entity<Kullanici>(entity =>
            {
                entity.ToTable("Kullanicilar");
                entity.HasKey(k => k.Id);
                entity.Property(k => k.Email).IsRequired().HasMaxLength(200);
                entity.HasIndex(k => k.Email).IsUnique();
                entity.Property(k => k.Adi).IsRequired().HasMaxLength(100);
                entity.Property(k => k.Soyadi).IsRequired().HasMaxLength(100);
                entity.Property(k => k.Sifre).IsRequired();
                entity.Property(k => k.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(k => k.UpdatedAt).HasDefaultValueSql("GETDATE()");
            });
            
            // Not tablosu konfigürasyonu
            modelBuilder.Entity<Not>(entity =>
            {
                entity.ToTable("Notlar");
                entity.HasKey(n => n.Id);
                entity.Property(n => n.Baslik).IsRequired().HasMaxLength(200);
                entity.Property(n => n.Icerik).IsRequired();
                entity.Property(n => n.DosyaAdi).HasMaxLength(500);
                entity.Property(n => n.DosyaYolu).HasMaxLength(1000);
                entity.Property(n => n.DosyaTuru).HasMaxLength(100);
                entity.Property(n => n.OlusturmaTarihi).HasDefaultValueSql("GETDATE()");
                entity.Property(n => n.GuncellemeTarihi).HasDefaultValueSql("GETDATE()");
                entity.Property(n => n.Silindi).HasDefaultValue(false);
                
                // Foreign Key Relationship
                entity.HasOne(n => n.Kullanici)
                      .WithMany(k => k.Notlar)
                      .HasForeignKey(n => n.KullaniciId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}