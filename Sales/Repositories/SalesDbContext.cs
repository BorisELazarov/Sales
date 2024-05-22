using Microsoft.EntityFrameworkCore;
using Sales.Entities;

namespace Sales.Repositories
{
    public class SalesDbContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SaleProduct> SaleProducts { get; set; }

        public SalesDbContext()
        {
            this.Users = this.Set<User>();
            this.Sales = this.Set<Sale>();
            this.Products = this.Set<Product>();
            this.SaleProducts = this.Set<SaleProduct>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Server=localhost;Database=SalesDb;Trusted_Connection=true")
                .UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { 
                Id= 1,
                Username="admin",
                Password="admin1234!",
                FirstName="Admin",
                LastName="Adminov",
                IsEmployee=true
                });
        }
    }
}
