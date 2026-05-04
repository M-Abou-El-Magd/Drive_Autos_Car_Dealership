using System.ComponentModel.DataAnnotations;

namespace Car_Dealership_System.DTOs
{
    public class CarCreateDto
    {
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

        [Required]
        public int SellerId { get; set; }

        public string? ImageUrl { get; set; }

        public bool Sold { get; set; } = false;
    }

    public class CarUpdateDto
    {
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

        public string? ImageUrl { get; set; }

        public bool Sold { get; set; } = false;
    }

    public class CarReadDto
    {
        public int Id { get; set; }
        public string Make { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string Color { get; set; } = null!;
        public int SellerId { get; set; }
        public string? ImageUrl { get; set; }
        public bool Sold { get; set; }
    }
}