using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackendApi.Models
{
    public class Kullanici
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Adi { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Soyadi { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Sifre { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        
        // Navigation Property
        public ICollection<Not> Notlar { get; set; } = new List<Not>();
    }
}