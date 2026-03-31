using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Interfaces
{
    // database operations available for Class
    public interface IClassRepository
    {
        // Get all class records from the database
        Task<IEnumerable<Class>> GetAllAsync();

        // Get all classes belonging to a specific academic year
        Task<IEnumerable<Class>> GetByYearAsync(int yearId);

        // Get one class by its ID, returns null if not found
        Task<Class?> GetByIdAsync(int id);

        // Check if a class with the same Grade, Section and Year already exists
        // e.g. "Grade 10 A" can only exist ONCE per academic year
        Task<bool> ClassExistsAsync(string grade, string section, int yearId);

        // Add a new class record to the database
        Task AddAsync(Class cls);

        // Mark a class record as modified in memory
        void Update(Class cls);

        // Mark a class record for deletion in memory
        void Delete(Class cls);

        // Commit all pending changes to the database
        Task SaveChangesAsync();
    }
}