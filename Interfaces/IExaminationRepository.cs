using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Interfaces
{
    public interface IExaminationRepository
    {
        // Get all examination records from the database
        Task<IEnumerable<Examination>> GetAllAsync();

        // Get all examinations belonging to a specific academic year        
        Task<IEnumerable<Examination>> GetByYearAsync(int yearId);

        // Get one examination by its ID, returns null if not found
        Task<Examination?> GetByIdAsync(int id);

        // Check if an examination with the same name already exists for a year
        Task<bool> ExamExistsAsync(string examName, int yearId);

        // Add a new examination record to the database
        Task AddAsync(Examination examination);

        // Mark an examination record as modified in memory
        void Update(Examination examination);

        // Mark an examination record for deletion in memory
        void Delete(Examination examination);

        // Commit all pending changes to the database
        Task SaveChangesAsync();
    }
}