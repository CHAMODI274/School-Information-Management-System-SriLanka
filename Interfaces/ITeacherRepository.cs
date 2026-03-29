using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Interfaces
{
    public interface ITeacherRepository
    {
        // Get all teacher records from the database
        Task<IEnumerable<Teacher>> GetAllAsync();

        // Get one teacher by their ID, returns null if not found
        Task<Teacher?> GetByIdAsync(int id);

        // Check if a NIC number already exists in the database 
        Task<bool> NICExistsAsync(string nic);

        // Check if an email already exists in the database
        Task<bool> EmailExistsAsync(string email);

        // Add a new teacher record to the database
        Task AddAsync(Teacher teacher);

        // Mark a teacher record as modified in memory
        void Update(Teacher teacher);

        // Mark a teacher record for deletion in memory
        void Delete(Teacher teacher);

        // Commit all pending changes to the database
        Task SaveChangesAsync();
    }
}