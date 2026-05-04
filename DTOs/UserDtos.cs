using System.ComponentModel.DataAnnotations;
using Car_Dealership_System.Models;

namespace Car_Dealership_System.DTOs
{
    public class UserCreateDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        public Role Role { get; set; }
    }

    public class UserUpdateDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public Role Role { get; set; }
    }

    public class UserReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public Role Role { get; set; }
        public UserProfileReadDto? Profile { get; set; }
    }

    public class UserProfileCreateDto
    {
        [Required]
        public string Address { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public int UserId { get; set; }
    }

    public class UserProfileUpdateDto
    {
        [Required]
        public string Address { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;
    }

    public class UserProfileReadDto
    {
        public int Id { get; set; }
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public int UserId { get; set; }
    }

    public class LoginRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }

    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }

    public class RefreshRequestDto
    {
        [Required]
        public string RefreshToken { get; set; } = null!;
    }
}