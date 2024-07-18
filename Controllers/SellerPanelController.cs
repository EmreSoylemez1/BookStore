using BookStore.Models;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SellerPanelController : ControllerBase
    {
        private readonly BookService _bookService;
        private readonly UserService _userService;

        public SellerPanelController(BookService bookService, UserService userService)
        {
            _bookService = bookService;
            _userService = userService;
        }

        [HttpPost("add")]
        public IActionResult AddBook([FromBody] Book book, [FromQuery] int sellerId)
        {
            var seller = _userService.Get(sellerId);
            if (seller == null || seller.Role != "Seller")
            {
                return Unauthorized("Sadece satıcılar kitap ekleyebilir.");
            }

            book.SellerName = seller.Username; // Satıcının kullanıcı adını ekleyin
            _bookService.Add(book);
            return Ok("Kitap başarıyla eklendi.");
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateBook(int id, [FromBody] Book book, [FromQuery] int sellerId)
        {
            var seller = _userService.Get(sellerId);
            if (seller == null || seller.Role != "Seller")
            {
                return Unauthorized("Sadece satıcılar kitap güncelleyebilir.");
            }

            var existingBook = _bookService.Get(id);
            if (existingBook == null)
            {
                return NotFound("Kitap bulunamadı.");
            }

            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.Genre = book.Genre;
            existingBook.Desc = book.Desc;
            existingBook.Price = book.Price;
            existingBook.CurrencyCode = book.CurrencyCode;
            existingBook.SellerName = seller.Username; // Satıcının kullanıcı adını ekleyin
            existingBook.Stock = book.Stock;

            _bookService.Update(existingBook);
            return Ok("Kitap başarıyla güncellendi.");
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteBook(int id, [FromQuery] int sellerId)
        {
            var seller = _userService.Get(sellerId);
            if (seller == null || seller.Role != "Seller")
            {
                return Unauthorized("Sadece satıcılar kitap silebilir.");
            }

            var book = _bookService.Get(id);
            if (book == null)
            {
                return NotFound("Kitap bulunamadı.");
            }

            _bookService.Delete(id);
            return Ok("Kitap başarıyla silindi.");
        }
    }
}
