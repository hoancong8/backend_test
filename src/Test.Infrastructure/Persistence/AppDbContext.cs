
using Microsoft.EntityFrameworkCore;
using test.src.Test.Domain.Entities;


namespace test.src.Test.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // public DbSet<User> Users { get; set; }
        // public DbSet<Product> Products { get; set; }




        // public DbSet<User> Users { get; set; }
        // public DbSet<Product> Products { get; set; }





        public DbSet<User> Users { get; set; }
        // Only include domain entities used by repositories
        // Scaffolded models (TestContext) live in test.Models and are registered separately.

    }
}