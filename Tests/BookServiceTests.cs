using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmallLibrary.Controllers;
using SmallLibrary.Models;
using SmallLibrary.Services;
using SmallLibrary.Data;
using Xunit;

namespace SmallLibrary.Tests
{
    public class BooksControllerTests
    {
        [Fact]
        public async Task GetBooks_ReturnsCorrectType()
        {
            // Arrange
            var mockBookService = new Mock<IBookService>();
            mockBookService.Setup(repo => repo.GetBooksAsync())
                .ReturnsAsync(GetTestBooks());
            var controller = new BooksController(mockBookService.Object);

            // Act
            var result = await controller.GetBooks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var books = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);
            Assert.Equal(3, books.Count());
        }

        private IEnumerable<Book> GetTestBooks()
        {
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Book 1", Author = "Author 1", ISBN = "1234567890", Genre = "Fiction", Stock = 8 },
                new Book { Id = 2, Title = "Book 2", Author = "Author 2", ISBN = "0987654321", Genre = "Non-fiction", Stock = 5 },
                new Book { Id = 3, Title = "Book 3", Author = "Author 3", ISBN = "1357902468", Genre = "Fantasy", Stock = 10 }
            };
            return books;
        }
    }

    public class BookServiceTests
    {
        [Fact]
        public async Task GetBooksAsync_ReturnsCorrectCount()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "Test_GetBooks")
                .Options;

            using (var context = new DatabaseContext(options))
            {
                context.Books.AddRange(
                    new Book { Id = 1, Title = "Book 1", Author = "Author 1", ISBN = "1234567890", Genre = "Fiction", Stock = 8 },
                    new Book { Id = 2, Title = "Book 2", Author = "Author 2", ISBN = "0987654321", Genre = "Non-fiction", Stock = 5 },
                    new Book { Id = 3, Title = "Book 3", Author = "Author 3", ISBN = "1357902468", Genre = "Fantasy", Stock = 10 }
                );
                context.SaveChanges();
            }

            using (var context = new DatabaseContext(options))
            {
                var service = new BookService(context);

                // Act
                var result = await service.GetBooksAsync();

                // Assert
                Assert.Equal(3, result.Count());
            }
        }
    }
}
