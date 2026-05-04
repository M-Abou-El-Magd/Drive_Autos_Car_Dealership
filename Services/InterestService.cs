using Car_Dealership_System.Data;
using Car_Dealership_System.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Car_Dealership_System.Services
{
    public class InterestService : IInterestService
    {
        private readonly AppDbContext _db;

        public InterestService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<InterestReadDto> CreateAsync(InterestCreateDto dto)
        {
            if (!await _db.Users.AnyAsync(u => u.Id == dto.ClientId && u.Role == Models.Role.Client))
                throw new InvalidOperationException("Client user not found or role mismatch.");

            if (!await _db.Cars.AnyAsync(c => c.Id == dto.CarId))
                throw new InvalidOperationException("Car not found.");

            if (await _db.ClientInterests.AnyAsync(ci => ci.ClientId == dto.ClientId && ci.CarId == dto.CarId))
                throw new InvalidOperationException("Already expressed interest.");

            var interest = new Models.ClientInterest
            {
                ClientId = dto.ClientId,
                CarId = dto.CarId,
                InterestedAt = DateTime.UtcNow
            };

            _db.ClientInterests.Add(interest);
            await _db.SaveChangesAsync();

            return new InterestReadDto
            {
                Id = interest.Id,
                ClientId = interest.ClientId,
                CarId = interest.CarId,
                InterestedAt = interest.InterestedAt
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var interest = await _db.ClientInterests.FindAsync(id);
            if (interest == null) return false;

            _db.ClientInterests.Remove(interest);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<InterestReadDto>> GetAllAsync()
        {
            return await _db.ClientInterests
                .AsNoTracking()
                .Select(i => new InterestReadDto
                {
                    Id = i.Id,
                    ClientId = i.ClientId,
                    CarId = i.CarId,
                    InterestedAt = i.InterestedAt
                })
                .ToListAsync();
        }

        public async Task<InterestReadDto?> GetByIdAsync(int id)
        {
            return await _db.ClientInterests
                .AsNoTracking()
                .Where(i => i.Id == id)
                .Select(i => new InterestReadDto
                {
                    Id = i.Id,
                    ClientId = i.ClientId,
                    CarId = i.CarId,
                    InterestedAt = i.InterestedAt
                })
                .FirstOrDefaultAsync();
        }
    }
}