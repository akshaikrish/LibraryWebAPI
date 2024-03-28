using Microsoft.EntityFrameworkCore;
using SmallLibrary.Models;

namespace SmallLibrary.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure seed data
            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "Book 1", Author = "Author 1", ISBN = "1234567890", Genre = "Fiction", Stock = 8 },
                new Book { Id = 2, Title = "Book 2", Author = "Author 2", ISBN = "0987654321", Genre = "Non-fiction", Stock = 1 }
            );
        }
    }
}
