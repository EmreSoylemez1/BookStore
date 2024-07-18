using BookStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("placeOrder")]
        public IActionResult PlaceOrder([FromQuery] int userId)
        {
            try
            {
                _orderService.PlaceOrder(userId);
                return Ok("Sipariş başarıyla oluşturuldu.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{userId}")]
        public IActionResult GetOrders(int userId)
        {
            var orders = _orderService.GetOrdersByUserId(userId);
            return Ok(orders);
        }
    }
}
