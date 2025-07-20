using BackendApi.Data;
using BackendApi.Models;
using BackendApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BackendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(k => k.Email == request.Email);

            if (kullanici == null || kullanici.Sifre != request.Sifre)
            {
                return Unauthorized(new { message = "Geçersiz email veya şifre" });
            }

            var token = _jwtService.GenerateToken(kullanici);

            return Ok(new
            {
                token = token,
                kullanici = new
                {
                    id = kullanici.Id,
                    adi = kullanici.Adi,
                    soyadi = kullanici.Soyadi,
                    email = kullanici.Email
                }
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Email kontrolü
            var mevcutKullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(k => k.Email == request.Email);

            if (mevcutKullanici != null)
            {
                return BadRequest(new { message = "Bu email adresi zaten kullanılıyor" });
            }

            var yeniKullanici = new Kullanici
            {
                Adi = request.Adi,
                Soyadi = request.Soyadi,
                Email = request.Email,
                Sifre = request.Sifre, // Gerçek projede hash'lenmeli
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Kullanicilar.Add(yeniKullanici);
            await _context.SaveChangesAsync();

            var token = _jwtService.GenerateToken(yeniKullanici);

            return Ok(new
            {
                token = token,
                kullanici = new
                {
                    id = yeniKullanici.Id,
                    adi = yeniKullanici.Adi,
                    soyadi = yeniKullanici.Soyadi,
                    email = yeniKullanici.Email
                }
            });
        }
    }

    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Sifre { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        [Required]
        public string Adi { get; set; } = string.Empty;

        [Required]
        public string Soyadi { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Sifre { get; set; } = string.Empty;
    }
} 