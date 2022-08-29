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
        public DbSet<Assortment> Assortments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeOrder> EmployeeOrders { get; set; }
        public DbSet<Set> Sets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeOrder>()
                .HasKey(bc => new { bc.EmployeeId, bc.OrderId });

            modelBuilder.Entity<EmployeeOrder>()
                .HasOne(bc => bc.Employee)
                .WithMany(b => b.EmployeeOrders)
                .HasForeignKey(bc => bc.EmployeeId);

            modelBuilder.Entity<EmployeeOrder>()
                .HasOne(bc => bc.Order)
                .WithMany(c => c.EmployeeOrders)
                .HasForeignKey(bc => bc.OrderId);

            modelBuilder.Entity<Assortment>()
               .HasOne(bc => bc.Set)
               .WithMany(c => c.Assortments);

            base.OnModelCreating(modelBuilder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Persist Security Info=False;Integrated Security=true;Initial Catalog=BlessingsDb;Server=DESKTOP-MBFQM73\\SQLEXPRESS");
        //}
    }
}
