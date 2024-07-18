using BookStore.Models;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly BookService _bookService;

        public BookController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public ActionResult<List<Book>> GetAll() =>
            _bookService.GetAll();

        [HttpGet("{id}")]
        public ActionResult<Book> Get(int id)
        {
            var book = _bookService.Get(id);

            if (book == null)
                return NotFound();

            return book;
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            _bookService.Add(book);
            return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Book book)
        {
            if (id != book.Id)
                return BadRequest();

            var existingBook = _bookService.Get(id);
            if (existingBook == null)
                return NotFound();

            _bookService.Update(book);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var book = _bookService.Get(id);
            if (book == null)
                return NotFound();

            _bookService.Delete(id);
            return NoContent();
        }
    }
}
