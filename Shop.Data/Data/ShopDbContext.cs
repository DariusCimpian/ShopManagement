using Microsoft.EntityFrameworkCore;
using Shop.Data.Models;

namespace Shop.Data.Data
{
    
    public class ShopDbContext : DbContext
    {

        public ShopDbContext()
        {
          
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
            if (!optionsBuilder.IsConfigured)
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "shop_database.db");

                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }
    }
}