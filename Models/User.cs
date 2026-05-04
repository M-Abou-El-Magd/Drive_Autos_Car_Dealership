using System.ComponentModel.DataAnnotations;

namespace Car_Dealership_System.Models
{
    public enum Role
    {
        Admin,
        SalesManager,
        Seller,
        Client
    }

    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        [Required]
        public Role Role { get; set; }

        public UserProfile? Profile { get; set; }

        public ICollection<Car>? CarsSold { get; set; }

        public ICollection<ClientInterest>? Interests { get; set; }

        public ICollection<Reservation>? Reservations { get; set; }

        public ICollection<RefreshToken>? RefreshTokens { get; set; }
    }
}