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
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}

