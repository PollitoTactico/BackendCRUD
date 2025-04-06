using Microsoft.EntityFrameworkCore;
using BackendCRUD.ApiService.Models;
using Microsoft.AspNetCore.Mvc;
using BackendCRUD.ApiService.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BackendCRUD.ApiService.Services;

namespace BackendCRUD.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        public AuthController(ApplicationDbContext context, IConfiguration configuration, EmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                    return BadRequest("El usuario ya existe.");

                var salt = GenerateSalt();
                var passwordHash = HashPassword(request.Contraseña, salt);

                var verificationToken = new Random().Next(100000, 999999).ToString();

                var user = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    NumeroTelefono = request.NumeroTelefono,
                    Cumpleaños = request.Cumpleaños,
                    IsActive = true,
                    Password = passwordHash,
                    Salt = Convert.ToBase64String(salt),
                    VerificationToken = verificationToken,
                    CreatedAt = DateTime.Now
                };

                try
                {
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    await _emailService.SendVerificationEmail(user.Email, verificationToken);

                   

                    return Ok(new
                    {
                        message = "Usuario registrado exitosamente, Revise su correo para verificar la cuenta",
                        token = verificationToken,
                        user = new
                        {
                            email = user.Email,
                            firstName = user.FirstName
                        }
                    });
                }
                catch (Exception dbEx)
                {
                    return StatusCode(500, $"Error de base de datos: {dbEx.Message}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (user == null)
                    return BadRequest("Usuario no encontrado.");

                if (user.VerificationToken != null)
                    return BadRequest("Por favor verifica tu email primero.");

                var salt = Convert.FromBase64String(user.Salt);
                var passwordHash = HashPassword(request.Contraseña, salt);

                if (user.Password != passwordHash)
                    return BadRequest("Contraseña incorrecta.");

                return Ok(new
                {
                    message = "Login exitoso",
                    user = new
                    {
                        id = user.Id,
                        email = user.Email,
                        firstName = user.FirstName,
                        lastName = user.LastName
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error en login: {ex.Message}");
            }
        }
        private string HashPassword(string password, byte[] salt)
        {
            using (var hmac = new HMACSHA512(salt))
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hash = hmac.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hash);
            }
        }

        private byte[] GenerateSalt()
        {
            return RandomNumberGenerator.GetBytes(128 / 8);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromBody] VerifyTokenDto request)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == request.Token);
                if (user == null)
                    return BadRequest("Token inválido");

                user.VerifiDate = DateTime.Now;
                user.VerificationToken = null;

                await _context.SaveChangesAsync();
                return Ok("Usuario verificado exitosamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

    }

}