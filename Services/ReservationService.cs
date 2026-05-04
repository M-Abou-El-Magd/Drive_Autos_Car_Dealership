using Car_Dealership_System.Data;
using Car_Dealership_System.DTOs;
using Car_Dealership_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Car_Dealership_System.Services
{
    public class ReservationService : IReservationService
    {
        private readonly AppDbContext _db;
        private readonly IEmailService _emailService;

        public ReservationService(AppDbContext db, IEmailService emailService)
        {
            _db = db;
            _emailService = emailService;
        }

        public async Task<ReservationReadDto> CreateAsync(int clientId, ReservationCreateDto dto)
        {
            var user = await _db.Users.Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Id == clientId && u.Role == Role.Client);
            if (user == null)
                throw new InvalidOperationException("Client not found or role mismatch.");

            var car = await _db.Cars.FirstOrDefaultAsync(c => c.Id == dto.CarId);
            if (car == null)
                throw new InvalidOperationException("Car not found.");

            var existingReservation = await _db.Reservations
                .FirstOrDefaultAsync(r => r.ClientId == clientId && r.CarId == dto.CarId && r.ExpiresAt > DateTime.UtcNow);
            if (existingReservation != null)
                throw new InvalidOperationException("You already have an active reservation for this car.");

            var reservation = new Reservation
            {
                ClientId = clientId,
                CarId = dto.CarId,
                Message = dto.Message,
                ReservedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1)
            };

            _db.Reservations.Add(reservation);
            await _db.SaveChangesAsync();

            var emailBody = $"Hello {user.Name},\n\n" +
                            "Your car reservation has been confirmed. Here are the details:\n\n" +
                            $"Reservation ID: {reservation.Id}\n" +
                            $"Car: {car.Make} {car.Model}\n" +
                            $"Year: {car.Year}\n" +
                            $"Price: ${car.Price:F2}\n" +
                            $"Color: {car.Color}\n" +
                            $"Reserved At (UTC): {reservation.ReservedAt:yyyy-MM-dd HH:mm}\n" +
                            $"Expires At (UTC): {reservation.ExpiresAt:yyyy-MM-dd HH:mm}\n" +
                            $"Message: {reservation.Message}\n\n" +
                            "Please visit the store within 24 hours to complete your purchase.\n\n" +
                            "Thank you for choosing our dealership.\n";

            await _emailService.SendEmailAsync(user.Email, $"Reservation confirmed - #{reservation.Id}", emailBody);

            return new ReservationReadDto
            {
                Id = reservation.Id,
                ClientId = reservation.ClientId,
                CarId = reservation.CarId,
                CarMake = car.Make,
                CarModel = car.Model,
                CarPrice = car.Price,
                CarColor = car.Color,
                CarImageUrl = car.ImageUrl,
                Message = reservation.Message,
                ReservedAt = reservation.ReservedAt,
                ExpiresAt = reservation.ExpiresAt
            };
        }

        public async Task<IEnumerable<ReservationReadDto>> GetByClientAsync(int clientId)
        {
            return await _db.Reservations
                .AsNoTracking()
                .Where(r => r.ClientId == clientId)
                .Select(r => new ReservationReadDto
                {
                    Id = r.Id,
                    ClientId = r.ClientId,
                    CarId = r.CarId,
                    CarMake = r.Car.Make,
                    CarModel = r.Car.Model,
                    CarPrice = r.Car.Price,
                    CarColor = r.Car.Color,
                    CarImageUrl = r.Car.ImageUrl,
                    Message = r.Message,
                    ReservedAt = r.ReservedAt,
                    ExpiresAt = r.ExpiresAt
                })
                .ToListAsync();
        }

        public async Task<ReservationReadDto?> GetByIdAsync(int id, int clientId)
        {
            return await _db.Reservations
                .AsNoTracking()
                .Where(r => r.Id == id && r.ClientId == clientId)
                .Select(r => new ReservationReadDto
                {
                    Id = r.Id,
                    ClientId = r.ClientId,
                    CarId = r.CarId,
                    CarMake = r.Car.Make,
                    CarModel = r.Car.Model,
                    CarPrice = r.Car.Price,
                    CarColor = r.Car.Color,
                    CarImageUrl = r.Car.ImageUrl,
                    Message = r.Message,
                    ReservedAt = r.ReservedAt,
                    ExpiresAt = r.ExpiresAt
                })
                .FirstOrDefaultAsync();
        }
    }
}
