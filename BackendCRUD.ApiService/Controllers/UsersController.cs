using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendCRUD.ApiService.Models;
using BackendCRUD.ApiService.Data;
using Microsoft.AspNetCore.Cors;
using Org.BouncyCastle.Crypto.Generators;

[EnableCors("AllowAll")]
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly
    ApplicationDbContext _context;

    public UsersController
    (ApplicationDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async
        Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    [HttpGet("{id}")]
    public async
        Task<ActionResult<User>> CreateUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(CreateUserDto createUserDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new User
        {
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            Email = createUserDto.Email,
            NumeroTelefono = createUserDto.NumeroTelefono,
            Cumpleaños = createUserDto.Cumpleaños,
            IsActive = createUserDto.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        var (hash, salt) = HashPassword(createUserDto.Password);
        user.Password = hash;
        user.Salt = salt;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
    }

    private (string hash, string salt) HashPassword(string password)
    {
        string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
        string hash = BCrypt.Net.BCrypt.HashPassword(password, salt);
        return (hash, salt);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UpdateUserDto updateUserDto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // Actualiza solo informacion basica
        user.FirstName = updateUserDto.FirstName;
        user.LastName = updateUserDto.LastName;
        user.Email = updateUserDto.Email;
        user.NumeroTelefono = updateUserDto.NumeroTelefono;
        user.Cumpleaños = updateUserDto.Cumpleaños;
        user.IsActive = updateUserDto.IsActive;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
            {
                return NotFound();
            }
            throw;
        }
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) 
        { 
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(u => u.Id == id);
    }
}