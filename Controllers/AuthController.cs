using Car_Dealership_System.DTOs;
using Car_Dealership_System.Models;
using Car_Dealership_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Car_Dealership_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = await _userService.AuthenticateAsync(dto.Email, dto.Password);
            if (entity == null)
                return Unauthorized("Invalid credentials");

            var user = new UserReadDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                Role = entity.Role
            };

            var token = GenerateJwt(user);
            var refreshToken = await _userService.GenerateRefreshTokenAsync(user.Id);
            if (refreshToken == null) return StatusCode(500, "Could not create refresh token");

            Response.Cookies.Append("refreshToken", refreshToken.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = refreshToken.ExpiryDate
            });

            return Ok(new AuthResponseDto
            {
                Token = token,
                RefreshToken = refreshToken.Token
            });
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var refreshTokenValue = dto.RefreshToken;
            var refreshTokenCookie = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshTokenValue) || refreshTokenValue != refreshTokenCookie)
                return BadRequest("Refresh token mismatch");

            var newToken = await _userService.RefreshCredentialsAsync(refreshTokenValue);
            if (newToken == null)
                return Unauthorized("Invalid or expired refresh token");

            var user = await _userService.GetByIdAsync(newToken.UserId);
            if (user == null)
                return Unauthorized("Invalid user for refresh token");

            var jwt = GenerateJwt(user);
            Response.Cookies.Append("refreshToken", newToken.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = newToken.ExpiryDate
            });

            return Ok(new AuthResponseDto
            {
                Token = jwt,
                RefreshToken = newToken.Token
            });
        }

        private string GenerateJwt(UserReadDto user)
        {
            var key = _configuration["JwtSettings:Secret"] ?? throw new InvalidOperationException("JWT secret is not configured");
            var issuer = _configuration["JwtSettings:Issuer"] ?? "CarDealershipApi";
            var audience = _configuration["JwtSettings:Audience"] ?? "CarDealershipApiUsers";

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}