using System.Collections.Generic;
using SmallLibrary.Models;

namespace SmallLibrary.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetBooksAsync();
        Task<Book> AddBookAsync(Book book);
        Task<Book> GetBookByIdAsync(int id);
        Task DeleteBookAsync(int id);
        Task<Book> UpdateBookAsync(Book book);
        // Book GetBook(int id);
        // Book AddBook(Book book);
        // void UpdateBook(Book book);
        // void DeleteBook(int id);
    }
}
