using BookStore.Data;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services
{
    public class OrderService
    {
        private readonly AppDbContext _dbContext;
        private readonly CartService _cartService;

        public OrderService(AppDbContext dbContext, CartService cartService)
        {
            _dbContext = dbContext;
            _cartService = cartService;
        }

        public void PlaceOrder(int userId)
        {
            var cart = _cartService.GetCartByUserId(userId);
            if (cart == null || !cart.Items.Any())
            {
                throw new InvalidOperationException("Sepet boş.");
            }

            foreach (var cartItem in cart.Items)
            {
                var book = _dbContext.Books.FirstOrDefault(b => b.Id == cartItem.BookId);
                if (book == null)
                {
                    throw new InvalidOperationException($"Kitap (ID: {cartItem.BookId}) bulunamadı.");
                }

                if (book.Stock < cartItem.Quantity)
                {
                    throw new InvalidOperationException($"Kitap (ID: {book.Id}) için yeterli stok yok.");
                }

                // Stoktan düş
                book.Stock -= cartItem.Quantity;
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Items = cart.Items.Select(ci => new OrderItem
                {
                    BookId = ci.BookId,
                    Quantity = ci.Quantity
                }).ToList()
            };

            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();

            _cartService.ClearCart(userId);
        }

        public List<Order> GetOrdersByUserId(int userId)
        {
            return _dbContext.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Book)
                .Include(o => o.User) // Kullanıcı bilgilerini de dahil etmek için Include ekleyin
                .Where(o => o.UserId == userId)
                .ToList();
        }
    }
}
