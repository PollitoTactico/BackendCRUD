using System.ComponentModel.DataAnnotations;

namespace BackendCRUD.ApiService.Models
{
    public class LoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Contraseña { get; set; }
    }
}
