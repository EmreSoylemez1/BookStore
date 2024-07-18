using BookStore.Models;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminPanelController : ControllerBase
    {
        private readonly BookService _bookService;
        private readonly UserService _userService;

        public AdminPanelController(BookService bookService, UserService userService)
        {
            _bookService = bookService;
            _userService = userService;
        }

        // Kitap İşlemleri
        [HttpPost("addBook")]
        public IActionResult AddBook([FromBody] Book book)
        {
            _bookService.Add(book);
            return Ok("Kitap başarıyla eklendi.");
        }

        [HttpPut("updateBook/{id}")]
        public IActionResult UpdateBook(int id, [FromBody] Book book)
        {
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
            existingBook.SellerName = book.SellerName;
            existingBook.Stock = book.Stock;

            _bookService.Update(existingBook);
            return Ok("Kitap başarıyla güncellendi.");
        }

        [HttpDelete("deleteBook/{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _bookService.Get(id);
            if (book == null)
            {
                return NotFound("Kitap bulunamadı.");
            }

            _bookService.Delete(id);
            return Ok("Kitap başarıyla silindi.");
        }

        [HttpGet("books")]
        public IActionResult GetBooks()
        {
            var books = _bookService.GetAll();
            return Ok(books);
        }

        // Kullanıcı İşlemleri
        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpPut("updateUser/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            var existingUser = _userService.Get(id);
            if (existingUser == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;
            existingUser.Phone = user.Phone;
            existingUser.Address = user.Address;
            existingUser.Role = user.Role;

            _userService.Update(existingUser);
            return Ok("Kullanıcı başarıyla güncellendi.");
        }

        [HttpDelete("deleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _userService.Get(id);
            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            _userService.Delete(id);
            return Ok("Kullanıcı başarıyla silindi.");
        }
    }
}
