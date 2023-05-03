using Models;
using Microsoft.EntityFrameworkCore;

namespace Data {
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        
        public DbSet<Book> Books => Set<Book>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // create tables
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Book>().ToTable("books");

            // set auto generated values for createon
            modelBuilder.Entity<User>()
                .Property(b => b.CreateOn)
                .HasDefaultValueSql("NOW()");
            
            modelBuilder.Entity<Book>()
                .Property(b => b.DateAdded)
                .HasDefaultValueSql("NOW()");

            modelBuilder.Entity<Book>()
                .Property(b => b.Reserved)
                .HasDefaultValueSql("0");

            modelBuilder.Entity<Book>()
                .Property(b => b.Borrowed)
                .HasDefaultValueSql("0");

            // set computed value for available book
            modelBuilder.Entity<Book>()
                .Property(p => p.Available)
                .HasComputedColumnSql("CASE WHEN reserved = 0 and borrowed = 0 THEN 1 ELSE 0 END", stored: true);

            modelBuilder.Entity<User>()
                .HasMany(b => b.Books)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.ReservedByUserId)
                .HasPrincipalKey(b => b.Id);
        }
    }
}