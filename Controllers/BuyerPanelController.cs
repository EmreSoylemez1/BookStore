using BookStore.Models;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuyerPanelController : ControllerBase
    {
        private readonly BookService _bookService;
        private readonly UserService _userService;
        private readonly CartService _cartService;
        private readonly OrderService _orderService;

        public BuyerPanelController(BookService bookService, UserService userService, CartService cartService, OrderService orderService)
        {
            _bookService = bookService;
            _userService = userService;
            _cartService = cartService;
            _orderService = orderService;
        }

        [HttpGet("search")]
        public IActionResult SearchBooks([FromQuery] string? genre, [FromQuery] string? title, [FromQuery] string? author)
        {
            var books = _bookService.GetAll();

            if (!string.IsNullOrEmpty(genre))
            {
                books = books.Where(b => b.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(title))
            {
                books = books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(author))
            {
                books = books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return Ok(books);
        }

        [HttpGet("details/{id}")]
        public IActionResult GetBookDetails(int id)
        {
            var book = _bookService.Get(id);
            if (book == null)
            {
                return NotFound("Kitap bulunamadı.");
            }

            return Ok(book);
        }

        [HttpPost("cart/add")]
        public IActionResult AddToCart([FromQuery] int userId, [FromQuery] int bookId, [FromQuery] int quantity)
        {
            try
            {
                _cartService.AddToCart(userId, bookId, quantity);
                return Ok("Kitap sepete eklendi.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Kitap eklenirken bir hata oluştu: {ex.Message}");
            }
        }

        [HttpGet("cart/{userId}")]
        public IActionResult GetCart(int userId)
        {
            var cart = _cartService.GetCartByUserId(userId);
            if (cart == null)
            {
                return NotFound("Sepet bulunamadı.");
            }

            return Ok(cart);
        }

        [HttpDelete("cart/remove/{userId}/{bookId}")]
        public IActionResult RemoveFromCart(int userId, int bookId)
        {
            try
            {
                _cartService.RemoveFromCart(userId, bookId);
                return Ok("Kitap sepetten kaldırıldı.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Kitap kaldırılırken bir hata oluştu: {ex.Message}");
            }
        }

        [HttpPost("order/place/{userId}")]
        public IActionResult PlaceOrder(int userId)
        {
            try
            {
                _orderService.PlaceOrder(userId);
                return Ok("Sipariş başarıyla oluşturuldu.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Sipariş oluşturulurken bir hata oluştu: {ex.Message}");
            }
        }

        [HttpGet("orders/{userId}")]
        public IActionResult GetOrders(int userId)
        {
            var orders = _orderService.GetOrdersByUserId(userId);
            if (orders == null || !orders.Any())
            {
                return NotFound("Sipariş bulunamadı.");
            }

            return Ok(orders);
        }

        [HttpPut("update-account/{userId}")]
        public IActionResult UpdateAccount(int userId, [FromBody] UpdateUserRequest request)
        {
            var user = _userService.GetUserById(userId);
            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            user.Email = request.Email;
            user.Password = request.Password;
            user.Phone = request.Phone;
            user.Address = request.Address;

            _userService.UpdateUser(user);

            return Ok("Kullanıcı bilgileri güncellendi.");
        }
    }

    public class AddToCartRequest
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateUserRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
