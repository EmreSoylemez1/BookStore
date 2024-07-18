using BookStore.Data;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services
{
    public class CartService
    {
        private readonly AppDbContext _dbContext;

        public CartService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Cart? GetCartByUserId(int userId)
        {
            var cart = _dbContext.Carts
                .Include(c => c.Items)
                .ThenInclude(ci => ci.Book)
                .Include(c => c.User) // Kullanıcı bilgilerini de dahil etmek için Include ekleyin
                .FirstOrDefault(c => c.UserId == userId);

            return cart;
        }

        public void AddToCart(int userId, int bookId, int quantity)
        {
            var cart = GetCartByUserId(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _dbContext.Carts.Add(cart);
            }

            var book = _dbContext.Books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                throw new InvalidOperationException("Kitap bulunamadı.");
            }

            if (book.Stock < quantity)
            {
                throw new InvalidOperationException("Yeterli stok yok.");
            }

            var cartItem = cart.Items.FirstOrDefault(ci => ci.BookId == bookId);
            if (cartItem == null)
            {
                cartItem = new CartItem { BookId = bookId, Quantity = quantity };
                cart.Items.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            // Stoktan geçici olarak düş
            book.Stock -= quantity;
            _dbContext.SaveChanges();
        }

        public void RemoveFromCart(int userId, int bookId)
        {
            var cart = GetCartByUserId(userId);
            if (cart == null) return;

            var cartItem = cart.Items.FirstOrDefault(ci => ci.BookId == bookId);
            if (cartItem != null)
            {
                var book = _dbContext.Books.FirstOrDefault(b => b.Id == bookId);
                if (book != null)
                {
                    // Stoku geri yükle
                    book.Stock += cartItem.Quantity;
                }

                cart.Items.Remove(cartItem);
                _dbContext.SaveChanges();
            }
        }

        public void ClearCart(int userId)
        {
            var cart = GetCartByUserId(userId);
            if (cart == null) return;

            foreach (var cartItem in cart.Items)
            {
                var book = _dbContext.Books.FirstOrDefault(b => b.Id == cartItem.BookId);
                if (book != null)
                {
                    // Stoku geri yükle
                    book.Stock += cartItem.Quantity;
                }
            }

            cart.Items.Clear();
            _dbContext.SaveChanges();
        }
    }
}
