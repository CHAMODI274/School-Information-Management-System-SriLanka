using System.ComponentModel.DataAnnotations;
using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.DTOs
{
    // Request: Login 
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters.")]        
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = string.Empty;
    }


    // Request: Register
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email    { get; set; } = string.Empty;

        [Required]
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