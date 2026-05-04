using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Car_Dealership_System.Models
{
    public class ClientInterest
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }

        public User Client { get; set; } = null!;

        [ForeignKey("Car")]
        public int CarId { get; set; }

        public Car Car { get; set; } = null!;

        public DateTime InterestedAt { get; set; } = DateTime.UtcNow;
    }
}