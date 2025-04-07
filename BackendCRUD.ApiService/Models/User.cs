using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace BackendCRUD.ApiService.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string NumeroTelefono { get; set; }
        [Required]
        public DateTime Cumpleaños { get; set; }
        [Required]
        public bool IsVerified { get; set; } = false; 
        public bool IsActive { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        public string Password { get; set; }
        public string Salt {  get; set; } //dato aleatorio para cada usuario
        public string? VerificationToken { get; set; } //token de verificacion
        public DateTime? VerifiDate { get; set; } //fecha y hora cuando el usuario verifico su email
        public string? PasswordResetToken { get; set; } //token de restablecimiento de contraseña
        public DateTime? ResetTokenExpires { get; set; } //fecha y hora cuando el token de restablecimiento de contraseña expira



    }


}

