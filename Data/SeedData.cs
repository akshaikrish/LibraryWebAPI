using System;
using System.Linq;
using SmallLibrary.Models;

namespace SmallLibrary.Data
{
    public static class SeedData
    {
        public static void Initialize(DatabaseContext context)
        {
            if (!context.Books.Any())
            {
                context.Books.AddRange(
                    new Book { Title = "Book 1", Author = "Author 1", ISBN = "1234567890", Genre = "Fiction", Stock = 8 },
                    new Book { Title = "Book 2", Author = "Author 2", ISBN = "0987654321", Genre = "Non-fiction", Stock = 5 }
                );
                context.SaveChanges();
            }
        }
    }
}
