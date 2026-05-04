using Car_Dealership_System.DTOs;
using Car_Dealership_System.Models;

namespace Car_Dealership_System.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserReadDto>> GetAllAsync();
        Task<UserReadDto?> GetByIdAsync(int id);
        Task<UserReadDto> CreateAsync(UserCreateDto dto);
        Task<bool> UpdateAsync(int id, UserUpdateDto dto);
        Task<bool> DeleteAsync(int id);

        Task<User?> AuthenticateAsync(string email, string password);
        Task<RefreshToken?> GenerateRefreshTokenAsync(int userId);
        Task<RefreshToken?> RefreshCredentialsAsync(string token);
    }
}