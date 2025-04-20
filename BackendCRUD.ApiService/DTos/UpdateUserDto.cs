using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace BackendCRUD.ApiService.Models;

public class UpdateUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string NumeroTelefono { get; set; }
    public DateTime Cumpleaños { get; set; }
    public bool IsActive { get; set; }
}