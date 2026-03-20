using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.DTOs
{
    // Request: Login 
    public class LoginRequestDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }


    // Request: Register
    public class RegisterRequestDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email    { get; set; } = string.Empty;
        public UserRole Role   { get; set; }   // Admin | Teacher | Student | Parent
    }
    

    // Response: returned after successful login / register
    public class AuthResponseDto
    {
        public int    UserId   { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email    { get; set; } = string.Empty;
        public string Role     { get; set; } = string.Empty;
        public string Token    { get; set; } = string.Empty;   // JWT
        public DateTime ExpiresAt { get; set; }
    }
}