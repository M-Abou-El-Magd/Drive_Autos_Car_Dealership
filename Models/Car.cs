using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Car_Dealership_System.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Make { get; set; } = null!;

        [Required]
        public string Model { get; set; } = null!;

        [Range(1886, 2100)]
        public int Year { get; set; }

        [Range(0.01, 10000000)]
        public decimal Price { get; set; }

        [Required]
        public string Color { get; set; } = null!;

        [ForeignKey("Seller")]
        public int SellerId { get; set; }

        public string? ImageUrl { get; set; }

        public bool Sold { get; set; } = false;

        public User Seller { get; set; } = null!;

        public ICollection<ClientInterest>? Interests { get; set; }

        public ICollection<Reservation>? Reservations { get; set; }
    }
}