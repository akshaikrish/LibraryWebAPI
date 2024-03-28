using SmallLibrary.Models;
using SmallLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmallLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            // Retrieve all books from the service asynchronously
            var books = await _bookService.GetBooksAsync();
            // Return HTTP 200 OK response with the list of books
            return Ok(books);
        }

        // POST: api/books
        [HttpPost]
        public async Task<ActionResult<Book>> AddBook(Book book)
        {
            // Check if the book object is null
            if (book == null)
            {
                return BadRequest("Book object is null");
            }

            // Add the book to the database asynchronously
            var addedBook = await _bookService.AddBookAsync(book);
            // Return HTTP 201 Created response with the added book and its location in the header
            return CreatedAtAction(nameof(GetBook), new { id = addedBook.Id }, addedBook);
        }

        // GET: api/books/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            // Retrieve the book with the specified ID from the service asynchronously
            var book = await _bookService.GetBookByIdAsync(id);
            // If the book is not found, return HTTP 404 Not Found response
            if (book == null)
            {
                return NotFound();
            }
            // Return HTTP 200 OK response with the book
            return book;
        }

        // DELETE: api/books/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            // Retrieve the book with the specified ID from the service asynchronously
            var book = await _bookService.GetBookByIdAsync(id);
            // If the book is not found, return HTTP 404 Not Found response
            if (book == null)
            {
                return NotFound();
            }

            // Delete the book from the database asynchronously
            await _bookService.DeleteBookAsync(id);
            // Return HTTP 204 No Content response
            return NoContent();
        }

        // PUT: api/books/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, Book book)
        {
            // Check if the ID in the URL matches the ID in the request body
            if (id != book.Id)
            {
                return BadRequest("ID in the URL does not match the ID in the request body");
            }

            // Retrieve the existing book with the specified ID from the service asynchronously
            var existingBook = await _bookService.GetBookByIdAsync(id);
            // If the book is not found, return HTTP 404 Not Found response
            if (existingBook == null)
            {
                return NotFound();
            }

            // Update the book in the database asynchronously
            await _bookService.UpdateBookAsync(book);
            // Return HTTP 204 No Content response
            return NoContent();
        }
    }
}
