using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Interfaces
{
    public interface IAcademicYearRepository
    {
        // Get all academic year records from the database
        Task<IEnumerable<AcademicYear>> GetAllAsync();

        // Get one academic year by its ID, returns null if not found
        Task<AcademicYear?> GetByIdAsync(int id);

        // Get the currently active academic year, returns null if none is active
        Task<AcademicYear?> GetActiveAsync();

        // Get all years that are currently marked as active
        Task<IEnumerable<AcademicYear>> GetAllActiveAsync();

        // Check if a year label already exists e.g. "2025"
        // Used to prevent duplicate year labels before creating
        Task<bool> YearExistsAsync(string year);



        // Add a new academic year record to the database
        Task AddAsync(AcademicYear academicYear);

        // Mark an academic year record as modified in memory
        void Update(AcademicYear academicYear);

        // Mark an academic year record for deletion in memory
        void Delete(AcademicYear academicYear);

        // Commit all pending changes to the database
        Task SaveChangesAsync();
    }
}