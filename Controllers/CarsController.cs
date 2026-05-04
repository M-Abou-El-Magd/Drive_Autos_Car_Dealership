using Car_Dealership_System.Data;
using Car_Dealership_System.DTOs;
using Car_Dealership_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;

namespace Car_Dealership_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;
        private readonly AppDbContext _db;

        public CarsController(ICarService carService, AppDbContext db)
        {
            _carService = carService;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cars = await _carService.GetAllAsync();
            if (User.IsInRole("Client"))
            {
                cars = cars.Where(c => !c.Sold);
            }
            return Ok(cars);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var car = await _carService.GetByIdAsync(id);
            if (car == null) return NotFound();

            var currentUserId = GetCurrentUserId();
            if (car.Sold && !User.IsInRole("Admin") && !(User.IsInRole("Seller") && car.SellerId == currentUserId))
            {
                return NotFound();
            }

            return Ok(car);
        }

        [HttpPost]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Create(CarCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _carService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPost("upload")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            if (!file.ContentType.StartsWith("image/"))
            {
                return BadRequest("Only image files are allowed.");
            }

            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "cars");
            Directory.CreateDirectory(uploadsDir);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsDir, fileName);

            await using var stream = System.IO.File.Create(filePath);
            await file.CopyToAsync(stream);

            var imageUrl = $"/uploads/cars/{fileName}";
            return Ok(new { imageUrl });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, CarUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var car = await _db.Cars.FindAsync(id);
            if (car == null) return NotFound();

            var currentUserId = GetCurrentUserId();
            if (!User.IsInRole("Admin") && !(User.IsInRole("Seller") && car.SellerId == currentUserId))
            {
                return Forbid();
            }

            car.Make = dto.Make;
            car.Model = dto.Model;
            car.Year = dto.Year;
            car.Price = dto.Price;
            car.Color = dto.Color;
            car.ImageUrl = dto.ImageUrl;
            car.Sold = dto.Sold;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var car = await _db.Cars.FindAsync(id);
            if (car == null) return NotFound();

            var currentUserId = GetCurrentUserId();
            if (User.IsInRole("Client"))
            {
                return Forbid();
            }

            if (User.IsInRole("Seller") && car.SellerId != currentUserId)
            {
                return Forbid();
            }

            _db.Cars.Remove(car);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("{id:int}/mark-sold")]
        public async Task<IActionResult> MarkSold(int id)
        {
            var car = await _db.Cars.FindAsync(id);
            if (car == null) return NotFound();

            var currentUserId = GetCurrentUserId();
            if (User.IsInRole("Seller") && car.SellerId != currentUserId)
            {
                return Forbid();
            }

            var marked = await _carService.MarkAsSoldAsync(id);
            if (!marked) return NotFound();
            return NoContent();
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var id) ? id : 0;
        }
    }
}
