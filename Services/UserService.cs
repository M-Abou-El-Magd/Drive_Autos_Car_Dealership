using Car_Dealership_System.Data;
using Car_Dealership_System.DTOs;
using Car_Dealership_System.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Car_Dealership_System.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;

        public UserService(AppDbContext db)
        {
            _db = db;
        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(sha.ComputeHash(bytes));
        }

        public async Task<UserReadDto> CreateAsync(UserCreateDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
            {
                throw new InvalidOperationException("Email already exists.");
            }

            if (dto.Role == Role.Admin)
            {
                throw new InvalidOperationException("Admin user cannot be created through public signup.");
            }

            if (dto.Role != Role.Client && dto.Role != Role.Seller)
            {
                throw new InvalidOperationException("Only Client and Seller roles are allowed for signup.");
            }

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password),
                Role = dto.Role
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return new UserReadDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return false;
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserReadDto>> GetAllAsync()
        {
            return await _db.Users
                .AsNoTracking()
                .Include(u => u.Profile)
                .Select(u => new UserReadDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role,
                    Profile = u.Profile != null ? new UserProfileReadDto
                    {
                        Id = u.Profile.Id,
                        Address = u.Profile.Address,
                        PhoneNumber = u.Profile.PhoneNumber,
                        UserId = u.Id
                    } : null
                })
                .ToListAsync();
        }

        public async Task<UserReadDto?> GetByIdAsync(int id)
        {
            return await _db.Users
                .AsNoTracking()
                .Include(u => u.Profile)
                .Where(u => u.Id == id)
                .Select(u => new UserReadDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role,
                    Profile = u.Profile != null ? new UserProfileReadDto
                    {
                        Id = u.Profile.Id,
                        Address = u.Profile.Address,
                        PhoneNumber = u.Profile.PhoneNumber,
                        UserId = u.Id
                    } : null
                })
                .FirstOrDefaultAsync();
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var hash = HashPassword(password);
            return await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == hash);
        }

        public async Task<RefreshToken?> GenerateRefreshTokenAsync(int userId)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var refreshToken = new RefreshToken
            {
                Token = token,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                UserId = userId
            };

            _db.RefreshTokens.Add(refreshToken);
            await _db.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<RefreshToken?> RefreshCredentialsAsync(string token)
        {
            var existing = await _db.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == token);

            if (existing == null || existing.Revoked || existing.ExpiryDate < DateTime.UtcNow)
                return null;

            existing.Revoked = true;
            await _db.SaveChangesAsync();

            var newToken = await GenerateRefreshTokenAsync(existing.UserId);
            if (newToken != null)
            {
                newToken.User = existing.User;
            }

            return newToken;
        }

        public async Task<bool> UpdateAsync(int id, UserUpdateDto dto)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return false;

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.Role = dto.Role;

            await _db.SaveChangesAsync();
            return true;
        }
    }
}