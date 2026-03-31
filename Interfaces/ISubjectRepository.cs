using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Interfaces
{
    public interface ISubjectRepository
    {
        // Get all subject records from the database
        Task<IEnumerable<Subject>> GetAllAsync();

        // Get only subjects that are currently active
        Task<IEnumerable<Subject>> GetAllActiveAsync();

        // Get one subject by its ID, returns null if not found
        Task<Subject?> GetByIdAsync(int id);

        // Check if a subject code already exists in the database
        Task<bool> SubjectCodeExistsAsync(string subjectCode);

        // Add a new subject record to the database
        Task AddAsync(Subject subject);

        // Mark a subject record as modified in memory
        void Update(Subject subject);

        // Mark a subject record for deletion in memory
        void Delete(Subject subject);

        // Commit all pending changes to the database
        Task SaveChangesAsync();

    }

    
}