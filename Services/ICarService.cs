using Car_Dealership_System.DTOs;

namespace Car_Dealership_System.Services
{
    public interface ICarService
    {
        Task<IEnumerable<CarReadDto>> GetAllAsync();
        Task<CarReadDto?> GetByIdAsync(int id);
        Task<CarReadDto> CreateAsync(CarCreateDto dto);
        Task<bool> UpdateAsync(int id, CarUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> MarkAsSoldAsync(int id);
    }
}