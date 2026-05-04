using Car_Dealership_System.DTOs;
using Car_Dealership_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Car_Dealership_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Client")]
    public class InterestsController : ControllerBase
    {
        private readonly IInterestService _interestService;

        public InterestsController(IInterestService interestService)
        {
            _interestService = interestService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var interests = await _interestService.GetAllAsync();
            return Ok(interests);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var interest = await _interestService.GetByIdAsync(id);
            if (interest == null) return NotFound();
            return Ok(interest);
        }

        [HttpPost]
        public async Task<IActionResult> Create(InterestCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var created = await _interestService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _interestService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}