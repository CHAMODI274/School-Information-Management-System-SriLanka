using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Interfaces
{
        public interface IClassCurriculumRepository
    {
        // Get all curriculum entries from the database
        Task<IEnumerable<ClassCurriculum>> GetAllAsync();

        // Get all curriculum entries belonging to a specific class
        Task<IEnumerable<ClassCurriculum>> GetByClassAsync(int classId);

        // Get all curriculum entries belonging to a specific academic year
        Task<IEnumerable<ClassCurriculum>> GetByYearAsync(int yearId);

        // Get one curriculum entry by its ID, returns null if not found
        Task<ClassCurriculum?> GetByIdAsync(int id);

        // Check if a subject is already assigned to a class for a specific year
        // same subject cannot appear twice in the same class in the same year
        Task<bool> EntryExistsAsync(int classId, int subjectId, int yearId);

        // Add a new curriculum entry to the database
        Task AddAsync(ClassCurriculum classCurriculum);

        // Mark a curriculum entry as modified in memory
        void Update(ClassCurriculum classCurriculum);

        // Mark a curriculum entry for deletion in memory
        void Delete(ClassCurriculum classCurriculum);

        // Commit all pending changes to the database
        Task SaveChangesAsync();
    }
}