using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendApi.Data;
using System.Security.Claims;

namespace BackendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public FileController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim?.Value ?? "0");
        }

        [HttpPost("upload/{notId}")]
        public async Task<IActionResult> UploadFile(int notId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "Dosya seçilmedi" });
            }

            var kullaniciId = GetCurrentUserId();

            // Not kontrolü
            var not = await _context.Notlar
                .FirstOrDefaultAsync(n => n.Id == notId && n.KullaniciId == kullaniciId && !n.Silindi);

            if (not == null)
            {
                return NotFound(new { message = "Not bulunamadı" });
            }

            // Dosya türü kontrolü
            var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".txt", ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            
            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest(new { message = "Desteklenmeyen dosya türü" });
            }

            // Dosya boyutu kontrolü (5MB limit)
            if (file.Length > 5 * 1024 * 1024)
            {
                return BadRequest(new { message = "Dosya boyutu 5MB'dan büyük olamaz" });
            }

            // Uploads klasörü oluştur
            var uploadsPath = Path.Combine(_environment.ContentRootPath, "uploads");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            // Benzersiz dosya adı oluştur
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsPath, fileName);

            // Dosyayı kaydet
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Not'u güncelle
            not.DosyaAdi = file.FileName;
            not.DosyaYolu = $"uploads/{fileName}";
            not.DosyaBoyutu = file.Length;
            not.DosyaTuru = file.ContentType;
            not.GuncellemeTarihi = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Dosya başarıyla yüklendi",
                dosyaAdi = not.DosyaAdi,
                dosyaBoyutu = not.DosyaBoyutu,
                dosyaTuru = not.DosyaTuru
            });
        }

        [HttpGet("download/{notId}")]
        public async Task<IActionResult> DownloadFile(int notId)
        {
            var kullaniciId = GetCurrentUserId();

            var not = await _context.Notlar
                .FirstOrDefaultAsync(n => n.Id == notId && n.KullaniciId == kullaniciId && !n.Silindi);

            if (not == null || string.IsNullOrEmpty(not.DosyaYolu))
            {
                return NotFound(new { message = "Dosya bulunamadı" });
            }

            var filePath = Path.Combine(_environment.ContentRootPath, not.DosyaYolu);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new { message = "Dosya sistemde bulunamadı" });
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            var contentType = not.DosyaTuru ?? "application/octet-stream";

            return File(fileBytes, contentType, not.DosyaAdi);
        }

        [HttpDelete("delete/{notId}")]
        public async Task<IActionResult> DeleteFile(int notId)
        {
            var kullaniciId = GetCurrentUserId();

            var not = await _context.Notlar
                .FirstOrDefaultAsync(n => n.Id == notId && n.KullaniciId == kullaniciId && !n.Silindi);

            if (not == null)
            {
                return NotFound(new { message = "Not bulunamadı" });
            }

            if (!string.IsNullOrEmpty(not.DosyaYolu))
            {
                var filePath = Path.Combine(_environment.ContentRootPath, not.DosyaYolu);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            // Not'tan dosya bilgilerini temizle
            not.DosyaAdi = null;
            not.DosyaYolu = null;
            not.DosyaBoyutu = null;
            not.DosyaTuru = null;
            not.GuncellemeTarihi = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Dosya silindi" });
        }
    }
} 