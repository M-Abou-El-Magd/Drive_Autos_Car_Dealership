using Car_Dealership_System.DTOs;

namespace Car_Dealership_System.Services
{
    public interface IReservationService
    {
        Task<ReservationReadDto> CreateAsync(int clientId, ReservationCreateDto dto);
        Task<IEnumerable<ReservationReadDto>> GetByClientAsync(int clientId);
        Task<ReservationReadDto?> GetByIdAsync(int id, int clientId);
    }
}
