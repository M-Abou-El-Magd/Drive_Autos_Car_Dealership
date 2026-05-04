using Car_Dealership_System.Data;
using Car_Dealership_System.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Car_Dealership_System.Services
{
    public class CarService : ICarService
    {
        private readonly AppDbContext _db;

        public CarService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<CarReadDto> CreateAsync(CarCreateDto dto)
        {
            var car = new Models.Car
            {
                Make = dto.Make,
                Model = dto.Model,
                Year = dto.Year,
                Price = dto.Price,
                Color = dto.Color,
                SellerId = dto.SellerId,
                ImageUrl = dto.ImageUrl,
                Sold = dto.Sold
            };

            _db.Cars.Add(car);
            await _db.SaveChangesAsync();

            return new CarReadDto
            {
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                Price = car.Price,
                Color = car.Color,
                SellerId = car.SellerId,
                ImageUrl = car.ImageUrl,
                Sold = car.Sold
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var car = await _db.Cars.FindAsync(id);
            if (car == null) return false;

            _db.Cars.Remove(car);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CarReadDto>> GetAllAsync()
        {
            return await _db.Cars
                .AsNoTracking()
                .Select(c => new CarReadDto
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    Year = c.Year,
                    Price = c.Price,
                    Color = c.Color,
                    SellerId = c.SellerId,
                    ImageUrl = c.ImageUrl,
                    Sold = c.Sold
                })
                .ToListAsync();
        }

        public async Task<CarReadDto?> GetByIdAsync(int id)
        {
            return await _db.Cars
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new CarReadDto
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    Year = c.Year,
                    Price = c.Price,
                    Color = c.Color,
                    SellerId = c.SellerId,
                    ImageUrl = c.ImageUrl,
                    Sold = c.Sold
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(int id, CarUpdateDto dto)
        {
            var car = await _db.Cars.FindAsync(id);
            if (car == null) return false;

            car.Make = dto.Make;
            car.Model = dto.Model;
            car.Year = dto.Year;
            car.Price = dto.Price;
            car.Color = dto.Color;
            car.ImageUrl = dto.ImageUrl;
            car.Sold = dto.Sold;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAsSoldAsync(int id)
        {
            var car = await _db.Cars.FindAsync(id);
            if (car == null) return false;

            car.Sold = true;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}