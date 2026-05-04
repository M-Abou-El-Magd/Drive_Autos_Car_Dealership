using System.ComponentModel.DataAnnotations;

namespace Car_Dealership_System.DTOs
{
    public class InterestCreateDto
    {
        [Required]
        public int ClientId { get; set; }

        [Required]
        public int CarId { get; set; }
    }

    public class InterestReadDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int CarId { get; set; }
        public DateTime InterestedAt { get; set; }
    }
}