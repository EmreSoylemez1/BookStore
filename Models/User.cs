using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Phone]
        public string? Phone { get; set; } // Phone alanı null olabilir

        public string? Address { get; set; } // Adress alanı null olabilir

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = "Buyer"; // Varsayılan rol "Buyer"
    }
}
