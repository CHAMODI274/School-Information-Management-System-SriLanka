using SchoolManagementSystem.DTOs.User;

namespace SchoolManagementSystem.Interfaces
{
    public interface IUserService
    {
        // Get all users
        Task<IEnumerable<UserResponseDto>> GetAllAsync();

        // Get a single user by ID
        Task<UserResponseDto?> GetByIdAsync(int id);

        // Change the logged-in user's own password
        // Returns false if the current password is wrong
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto dto);

        // Admin: activate or deactivate a user account
        Task<UserResponseDto?> UpdateStatusAsync(int id, UpdateUserStatusDto dto);

        // Admin: change a user's role
        Task<UserResponseDto?> UpdateRoleAsync(int id, UpdateUserRoleDto dto);

        // Admin: delete a user
        Task<bool> DeleteAsync(int id);
    }
}