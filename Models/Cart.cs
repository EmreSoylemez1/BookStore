using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BookStore.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        [NotMapped]
        public decimal TotalPrice
        {
            get
            {
                return Items.Sum(item => item.Book.Price * item.Quantity);
            }
        }
    }

    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; } = null!;
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
