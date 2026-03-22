using Microsoft.AspNetCore.Mvc;
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

                // Step 3 : Check if account is active
                if (user.Status == UserStatus.Inactive)
                    return Unauthorized(new {message = "Account is disabled. Contact admin."});

             // Step 4 : Verify the password using BCrypt
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



        // Post api/auth/register
        // Body: { "userName": "admin01", "password": "secret123", "email": "admin@school.lk", "role": "Admin" }

        // NOTE: In production, protect this endpoint so only Admins can call it.
        //       For now it is open so you can create your first admin account.

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            // Step 1: Check if username already exists
            bool userNameTaken = await _context.Users
                 .AnyAsync(u => u.UserName == dto.UserName);
                 if (userNameTaken)
                    return Conflict(new {message = "Username is already taken."});

            // Step 2: Check if email already exists
            bool emailTaken = await _context.Users
                 .AnyAsync(u => u.Email == dto.Email);
                 if (emailTaken)
                    return Conflict(new { message = "Email is already registered." });

            // Step 3: Hash Password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // Step 4: Create new user object
            var user = new User
            {
                UserName = dto.UserName,
                Password = hashedPassword,
                Email = dto.Email,
                Role = dto.Role,
                Status = UserStatus.Active,
                CreateDate = DateTime.UtcNow
            };

            // Step 5: Save to Database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Step 6: Generate JWT token
            var (token, expiresAt) = _jwtService.GenerateToken(user);

            // Step 7: Return the response
             return CreatedAtAction(nameof(Login), new AuthResponseDto
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