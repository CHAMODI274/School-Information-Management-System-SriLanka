using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.DTOs;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.Enums;
using SchoolManagementSystem.Services;

namespace SchoolManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SchoolDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(SchoolDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // POST api/auth/login
        // Body: { "userName": "admin01", "password": "secret123"}

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            // Step 1: Find the user by username
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == dto.UserName);

                // Step 2 : If user not found, return error
                if (user == null)
                return Unauthorized(new { message = "Invalid username or password."});

                // Step 3 : Check if account is acctive
                if (user.Status == UserStatus.Inactive)
                    return Unauthorized(new {message = "Account is disabled. Contact admin."});

                // Step 4 : Verify the password using BCryppt
                bool passwordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
                if (!passwordValid)
                    return Unauthorized(new {message = "Invalid username or password."});

             //Step 5: Generate JWT token
             var (token, expiresAt) = _jwtService.GenerateToken(user);

             // Step 6: Return the response
             return Ok(new AuthResponseDto
             {
                 UserId = user.UserId,
                 UserName = user.UserName,
                 Email = user.Email,
                 Role = user.Role.ToString(),
                 Token = token,
                 ExpiresAt = expiresAt
             });


            
        }


    }
}