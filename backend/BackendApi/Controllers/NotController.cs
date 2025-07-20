using BackendApi.Data;
using BackendApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace BackendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotController(AppDbContext context)
        {
            _context = context;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim?.Value ?? "0");
        }

        [HttpGet]
        public async Task<IActionResult> GetNotlar()
        {
            var kullaniciId = GetCurrentUserId();
            
            var notlar = await _context.Notlar
                .Where(n => n.KullaniciId == kullaniciId && !n.Silindi)
                .OrderByDescending(n => n.GuncellemeTarihi)
                .Select(n => new
                {
                    id = n.Id,
                    baslik = n.Baslik,
                    icerik = n.Icerik,
                    dosyaAdi = n.DosyaAdi,
                    dosyaYolu = n.DosyaYolu,
                    dosyaBoyutu = n.DosyaBoyutu,
                    dosyaTuru = n.DosyaTuru,
                    olusturmaTarihi = n.OlusturmaTarihi,
                    guncellemeTarihi = n.GuncellemeTarihi
                })
                .ToListAsync();

            return Ok(notlar);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNot(int id)
        {
            var kullaniciId = GetCurrentUserId();
            
            var not = await _context.Notlar
                .FirstOrDefaultAsync(n => n.Id == id && n.KullaniciId == kullaniciId && !n.Silindi);

            if (not == null)
            {
                return NotFound(new { message = "Not bulunamadı" });
            }

            return Ok(new
            {
                id = not.Id,
                baslik = not.Baslik,
                icerik = not.Icerik,
                dosyaAdi = not.DosyaAdi,
                dosyaYolu = not.DosyaYolu,
                dosyaBoyutu = not.DosyaBoyutu,
                dosyaTuru = not.DosyaTuru,
                olusturmaTarihi = not.OlusturmaTarihi,
                guncellemeTarihi = not.GuncellemeTarihi
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateNot([FromBody] CreateNotRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var kullaniciId = GetCurrentUserId();

            var yeniNot = new Not
            {
                Baslik = request.Baslik,
                Icerik = request.Icerik,
                KullaniciId = kullaniciId,
                OlusturmaTarihi = DateTime.Now,
                GuncellemeTarihi = DateTime.Now,
                Silindi = false
            };

            _context.Notlar.Add(yeniNot);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNot), new { id = yeniNot.Id }, new
            {
                id = yeniNot.Id,
                baslik = yeniNot.Baslik,
                icerik = yeniNot.Icerik,
                olusturmaTarihi = yeniNot.OlusturmaTarihi,
                guncellemeTarihi = yeniNot.GuncellemeTarihi
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNot(int id, [FromBody] UpdateNotRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var kullaniciId = GetCurrentUserId();

            var not = await _context.Notlar
                .FirstOrDefaultAsync(n => n.Id == id && n.KullaniciId == kullaniciId && !n.Silindi);

            if (not == null)
            {
                return NotFound(new { message = "Not bulunamadı" });
            }

            not.Baslik = request.Baslik;
            not.Icerik = request.Icerik;
            not.GuncellemeTarihi = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                id = not.Id,
                baslik = not.Baslik,
                icerik = not.Icerik,
                guncellemeTarihi = not.GuncellemeTarihi
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteNot(int id)
        {
            var kullaniciId = GetCurrentUserId();

            var not = await _context.Notlar
                .FirstOrDefaultAsync(n => n.Id == id && n.KullaniciId == kullaniciId && !n.Silindi);

            if (not == null)
            {
                return NotFound(new { message = "Not bulunamadı" });
            }

            not.Silindi = true;
            not.SilmeTarihi = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Not arşive taşındı" });
        }

        [HttpGet("arsiv")]
        public async Task<IActionResult> GetArsiviNotlar()
        {
            var kullaniciId = GetCurrentUserId();
            
            var arsiviNotlar = await _context.Notlar
                .Where(n => n.KullaniciId == kullaniciId && n.Silindi)
                .OrderByDescending(n => n.SilmeTarihi)
                .Select(n => new
                {
                    id = n.Id,
                    baslik = n.Baslik,
                    icerik = n.Icerik,
                    olusturmaTarihi = n.OlusturmaTarihi,
                    silmeTarihi = n.SilmeTarihi
                })
                .ToListAsync();

            return Ok(arsiviNotlar);
        }

        [HttpPut("arsiv/{id}/geri-yukle")]
        public async Task<IActionResult> RestoreNot(int id)
        {
            var kullaniciId = GetCurrentUserId();

            var not = await _context.Notlar
                .FirstOrDefaultAsync(n => n.Id == id && n.KullaniciId == kullaniciId && n.Silindi);

            if (not == null)
            {
                return NotFound(new { message = "Arşivde not bulunamadı" });
            }

            not.Silindi = false;
            not.SilmeTarihi = null;
            not.GuncellemeTarihi = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Not geri yüklendi" });
        }

        [HttpDelete("arsiv/{id}/kalici-sil")]
        public async Task<IActionResult> HardDeleteNot(int id)
        {
            var kullaniciId = GetCurrentUserId();

            var not = await _context.Notlar
                .FirstOrDefaultAsync(n => n.Id == id && n.KullaniciId == kullaniciId && n.Silindi);

            if (not == null)
            {
                return NotFound(new { message = "Arşivde not bulunamadı" });
            }

            _context.Notlar.Remove(not);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Not kalıcı olarak silindi" });
        }
    }

    public class CreateNotRequest
    {
        [Required]
        [StringLength(200)]
        public string Baslik { get; set; } = string.Empty;

        [Required]
        public string Icerik { get; set; } = string.Empty;
    }

    public class UpdateNotRequest
    {
        [Required]
        [StringLength(200)]
        public string Baslik { get; set; } = string.Empty;

        [Required]
        public string Icerik { get; set; } = string.Empty;
    }
} 