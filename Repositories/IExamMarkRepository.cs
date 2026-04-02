using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Interfaces
{
    public interface IExamMarkRepository
    {
        // Get all marks for a specific exam and class
        Task<IEnumerable<ExamMark>> GetByExamAndClassAsync(int examId, int classId);

        // Get all marks for a specific enrollment
        Task<IEnumerable<ExamMark>> GetByEnrollmentAsync(int enrollmentId);

        // Get all marks for a specific examination across all classes
        Task<IEnumerable<ExamMark>> GetByExamAsync(int examId);

        // Get one exam mark by its ID, or returns null if not found
        Task<ExamMark?> GetByIdAsync(int id);

        // Check if a mark already exists for a student for a subject in an exam
        Task<bool> MarkExistsAsync(int enrollmentId, int classCurriculumId, int examId);

        // Get the Examination record by ID
        Task<Examination?> GetExaminationByIdAsync(int examId);

        // Get a Teacher record by their linked UserId, or return null if no teacher is linked
        Task<Teacher?> GetTeacherByUserIdAsync(int userId);

        // Check if a teacher is allocated to teach a specific subject in a class -> returns true if allocated, or false if not
        Task<bool> IsTeacherAllocatedToSubjectAsync(int classCurriculumId, int teacherId);

        // Add a new exam mark record to the database
        Task AddAsync(ExamMark examMark);

        // Mark an exam mark record as modified in memory
        void Update(ExamMark examMark);

        // Mark an exam mark record for deletion in memory
        void Delete(ExamMark examMark);

        // Commit all pending changes to the database
        Task SaveChangesAsync();
    }
}