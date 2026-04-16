
using Microsoft.EntityFrameworkCore;
using test.src.Test.Domain.Entities;

namespace test.src.Test.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}