using Blessings.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blessings.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly DbContextOptions _options;
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Persist Security Info=False;Integrated Security=true;Initial Catalog=BlessingsDb;Server=DESKTOP-MBFQM73\\SQLEXPRESS");
        }
    }
}
