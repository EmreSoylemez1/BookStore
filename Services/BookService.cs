using BookStore.Data;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services
{
    public class BookService
    {
        private readonly AppDbContext _dbContext;

        public BookService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Book> GetAll()
        {
            return _dbContext.Books.ToList();
        }

        public Book? Get(int id)
        {
            return _dbContext.Books.FirstOrDefault(b => b.Id == id);
        }

        public void Add(Book book)
        {
            _dbContext.Books.Add(book);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var book = _dbContext.Books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                _dbContext.Books.Remove(book);
                _dbContext.SaveChanges();
            }
        }

        public void Update(Book book)
        {
            var existingBook = _dbContext.Books.FirstOrDefault(b => b.Id == book.Id);
            if (existingBook != null)
            {
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.Genre = book.Genre;
                existingBook.Desc = book.Desc;
                existingBook.Price = book.Price;
                existingBook.CurrencyCode = book.CurrencyCode;
                existingBook.SellerName = book.SellerName;
                existingBook.Stock = book.Stock;

                _dbContext.SaveChanges();
            }
        }
    }
}
