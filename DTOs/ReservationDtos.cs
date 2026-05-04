using System.ComponentModel.DataAnnotations;

namespace Car_Dealership_System.DTOs
{
    public class ReservationCreateDto
    {
        [Required]
        public int CarId { get; set; }

        [Required]
        [MinLength(5)]
        public string Message { get; set; } = null!;
    }

    public class ReservationReadDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int CarId { get; set; }
        public string CarMake { get; set; } = null!;
        public string CarModel { get; set; } = null!;
        public decimal CarPrice { get; set; }
        public string CarColor { get; set; } = null!;
        public string? CarImageUrl { get; set; }
        public string Message { get; set; } = null!;
        public DateTime ReservedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
