using SchoolManagementSystem.DTOs.User;
using SchoolManagementSystem.Interfaces;

namespace SchoolManagementSystem.Services
{
    // UserService contains all the BUSINESS LOGIC for user operations.
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        // Constructor injection
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        // Get all users from the repository and convert each one to a DTO
        public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(u => MapToResponseDto(u));
        }


        // Get a single user by ID and convert to DTO
        public async Task<UserResponseDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null) return null;

            return MapToResponseDto(user);
        }



        // Change the logged-in user's password
        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            // If somehow the user doesn't exist, return false
            if (user == null) return false;

            // Business rule: verify the current password before allowing change
            bool currentPasswordCorrect = BCrypt.Net.BCrypt.Verify(
                dto.CurrentPassword, user.Password);
                // if the current password is wrong, reject the change
                if (!currentPasswordCorrect) return false;

            // Hash the new password before saving
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            // Tell the repository to update this user, then save to the database
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }



        // Update a user's Active/Inactive status
        public async Task<UserResponseDto?> UpdateStatusAsync(int id, UpdateUserStatusDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null) return null;

            user.Status = dto.Status; // Apply the status change from the DTO to the user model

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return MapToResponseDto(user); // Return the updated user as a DTO
        }



        // Update a user's role
        public async Task<UserResponseDto?> UpdateRoleAsync(int id, UpdateUserRoleDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null) return null;

            user.Role = dto.Role; // Apply the role change from the DTO to the user model

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return MapToResponseDto(user);
        }



        // Delete a user permanently
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null) return false; // If the user doesn't exist, there's nothing to delete

            _userRepository.Delete(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }


        // Converts a User model (database object) into a UserResponseDto (what we send to the frontend)
        // This keeps the mapping logic in one place
        private static UserResponseDto MapToResponseDto(Models.User u)
        {
            return new UserResponseDto
            {
                UserId = u.UserId,
                UserName = u.UserName,
                Email = u.Email,
                Role = u.Role.ToString(), // converts enum to string
                Status = u.Status.ToString(), // converts enum to string
                CreateDate = u.CreateDate
            };
        }
    }
}