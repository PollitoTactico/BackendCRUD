using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using BackendCRUD.ApiService.Models;

namespace BackendCRUD.ApiService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

    }
}


