using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmallLibrary.Models;
using SmallLibrary.Data;

namespace SmallLibrary.Services
{
    public class BookService : IBookService
    {
        private readonly DatabaseContext _context;

        public BookService(DatabaseContext context)
        {
            _context = context;
        }

        // Retrieve all books asynchronously
        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        // Add a new book asynchronously
        public async Task<Book> AddBookAsync(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book), "Book object is null");
            }

            // Add the book to the database and save changes
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        // Retrieve a book by ID asynchronously
        public async Task<Book> GetBookByIdAsync(int id)
        {
            // Find the book with the specified ID or return null if not found
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            
            // Return the book if found, otherwise return null
            return book!;
        }

        // Delete a book by ID asynchronously
        public async Task DeleteBookAsync(int id)
        {
            // Find the book with the specified ID
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                // Remove the book from the database and save changes
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        // Update an existing book asynchronously
        public async Task<Book> UpdateBookAsync(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book), "Book object is null");
            }

            // Detach any existing entity with the same key from the context
            var existingBook = await _context.Books.FindAsync(book.Id);
            if (existingBook != null)
            {
                _context.Entry(existingBook).State = EntityState.Detached;
            }

            // Update the book's state and save changes
            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency conflicts
                if (!BookExists(book.Id))
                {
                    // Throw an exception or handle the case appropriately
                    throw new Exception("Book not found"); 
                }
                else
                {
                    throw;
                }
            }

            return book;
        }




        // Check if a book exists by ID
        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
