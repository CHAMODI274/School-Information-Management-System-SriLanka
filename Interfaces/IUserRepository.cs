using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Interfaces
{
    public interface IUserRepository
    {
        // Get all users from the database
        Task<IEnumerable<User>> GetAllAsync();

        // Get a single user by their ID
        Task<User?> GetByIdAsync(int id);

        // Update an existing user record
        void Update(User user);

        // Delete a user record
        void Delete(User user);

        // Save all pending changes to the database
        Task SaveChangesAsync();
    }
}