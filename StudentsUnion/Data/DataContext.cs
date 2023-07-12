using Microsoft.EntityFrameworkCore;
using StudentsUnion.Models;

namespace StudentsUnion.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Bid > Bids { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    Id = 1,
                    Email = "111",
                    Role = "Admin",
                    Password = "password",
                    CreationDate = DateTime.Now,
                    FIO = "Abobus",
                    Phone = "+78005553535",
                    Address = "улица",
                    Position = "батя"
                }
            );
        }
    }
}
