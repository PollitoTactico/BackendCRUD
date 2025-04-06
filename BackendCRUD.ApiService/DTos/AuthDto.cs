using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using BackendCRUD.ApiService.Models;

public class RegisterDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string NumeroTelefono { get; set; }
    public DateTime Cumpleaños { get; set; }
    public string Contraseña { get; set; }
}


