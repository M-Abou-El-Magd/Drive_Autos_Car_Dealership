using Car_Dealership_System.DTOs;

namespace Car_Dealership_System.Services
{
    public interface IInterestService
    {
        Task<IEnumerable<InterestReadDto>> GetAllAsync();
        Task<InterestReadDto?> GetByIdAsync(int id);
        Task<InterestReadDto> CreateAsync(InterestCreateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}