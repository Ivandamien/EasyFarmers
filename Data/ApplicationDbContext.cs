using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EasyFarmers.Models;

namespace EasyFarmers.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        // Replace with your EasyFarmer entities
        // Example:
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Add any additional model configurations here specific to EasyFarmer
        }
    }
}