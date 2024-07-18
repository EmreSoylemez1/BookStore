using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Author { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string Genre { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Desc { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(3)]
        [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "CurrencyCode must be a 3-letter code")]
        public string CurrencyCode { get; set; } = string.Empty;

        [MaxLength(50)]
        public string SellerName { get; set; } = string.Empty;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative number")]
        public int Stock { get; set; }
    }
}
