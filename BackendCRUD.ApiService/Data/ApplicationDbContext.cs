using Microsoft.EntityFrameworkCore;
using BackendCRUD.ApiService.Models;

namespace BackendCRUD.ApiService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ProfileUser> ProfileUsers { get; set; }
        public DbSet<Curriculum> Curriculum { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<ProfileUser>()
                .Property(p => p.NombrePerfil)
                .IsRequired();

            modelBuilder.Entity<ProfileUser>()
                .Property(p => p.MisionCargo)
                .IsRequired();

            modelBuilder.Entity<ProfileUser>()
                .Property(p => p.Empresa)
                .IsRequired();
        }
    }
}