using BookStore.Models;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public IActionResult GetCart(int userId)
        {
            var cart = _cartService.GetCartByUserId(userId);
            return Ok(cart);
        }

        [HttpPost("add")]
        public IActionResult AddToCart([FromQuery] int userId, [FromQuery] int bookId, [FromQuery] int quantity)
        {
            _cartService.AddToCart(userId, bookId, quantity);
            return Ok("Kitap sepete eklendi.");
        }

        [HttpDelete("remove")]
        public IActionResult RemoveFromCart([FromQuery] int userId, [FromQuery] int bookId)
        {
            _cartService.RemoveFromCart(userId, bookId);
            return Ok("Kitap sepetten çıkarıldı.");
        }

        [HttpPost("clear")]
        public IActionResult ClearCart([FromQuery] int userId)
        {
            _cartService.ClearCart(userId);
            return Ok("Sepet temizlendi.");
        }
    }
}
