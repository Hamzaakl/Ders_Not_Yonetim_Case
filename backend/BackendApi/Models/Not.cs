using System;
using System.ComponentModel.DataAnnotations;

namespace BackendApi.Models
{
    public class Not
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Baslik { get; set; } = string.Empty;
        
        [Required]
        public string Icerik { get; set; } = string.Empty;
        
        // Dosya bilgileri
        public string? DosyaAdi { get; set; }
        public string? DosyaYolu { get; set; }
        public long? DosyaBoyutu { get; set; }
        public string? DosyaTuru { get; set; }
        
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        public DateTime GuncellemeTarihi { get; set; } = DateTime.Now;
        
        // Soft Delete
        public bool Silindi { get; set; } = false;
        public DateTime? SilmeTarihi { get; set; }
        
        // Foreign Key
        public int KullaniciId { get; set; }
        
        // Navigation Property
        public Kullanici Kullanici { get; set; } = null!;
    }
}