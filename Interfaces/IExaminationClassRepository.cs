using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Interfaces
{
    public interface IExaminationClassRepository
    {
        // Get all examination class records from the database
        Task<IEnumerable<ExaminationClass>> GetAllAsync();

        // Get all classes scheduled for a specific examination
        Task<IEnumerable<ExaminationClass>> GetByExamAsync(int examId);

        // Get all exams scheduled for a specific class
        Task<IEnumerable<ExaminationClass>> GetByClassAsync(int classId);

        // Get one examination class record by its ID, or returns null if not found
        Task<ExaminationClass?> GetByIdAsync(int id);

        // Check if an exam is already scheduled for a specific class (same exam cannot be scheduled for the same class twice)
        Task<bool> ExamClassExistsAsync(int examId, int classId);

        // Add a new examination class record to the database
        Task AddAsync(ExaminationClass examinationClass);

        // Mark an examination class record as modified in memory
        void Update(ExaminationClass examinationClass);

        // Mark an examination class record for deletion in memory
        void Delete(ExaminationClass examinationClass);

        // Commit all pending changes to the database
        Task SaveChangesAsync();
    }
}