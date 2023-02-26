using CompService.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace CompService.Database
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
            //Database.EnsureCreated();
        }

        public DbSet<UserDb> Users { get; set; } = null!;
        public DbSet<UserVerificationDb> UserVerifications { get; set; } = null!;
    }
}
