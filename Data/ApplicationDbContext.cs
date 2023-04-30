using Models;
using Microsoft.EntityFrameworkCore;

namespace Data {
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // create users table
            modelBuilder.Entity<User>().ToTable("users");

            // set auto generated values for createon
            modelBuilder.Entity<User>()
                .Property(b => b.CreateOn)
                .HasDefaultValueSql("NOW()");
        }
    }
}