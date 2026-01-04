using Microsoft.EntityFrameworkCore;

namespace WebApplication3.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Ta table Produits
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Admin> Admins { get; set; }

        public DbSet<performanceLog> PerformanceLogs { get; set; }

        public DbSet<User> Users { get; set; }

    }
}
