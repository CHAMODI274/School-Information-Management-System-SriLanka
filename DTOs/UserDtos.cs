using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.DTOs.User
{
    // 1. Fields required when changing a user's password 
    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }


    // 2. Fields required when admin updates a user's status
    public class UpdateUserStatusDto
    {
        public UserStatus Status { get; set; }           
    }


    // 3. Fields required when admin updates a user's role
    public class UpdateUserRoleDto
    {
        public UserRole Role { get; set; }
    }


    // 4. Fields returned to the client when fetching user data
    public class UserResponseDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
    }


}