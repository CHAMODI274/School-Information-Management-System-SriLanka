using SchoolManagementSystem.DTOs.ExamMarks;

namespace SchoolManagementSystem.Interfaces
{
    public interface IExamMarkService
    {
        // Get all marks for a specific exam and class
        Task<IEnumerable<ExamMarkResponseDto>> GetByExamAndClassAsync(int examId, int classId);

        // Get all marks for a specific enrollment
        Task<IEnumerable<ExamMarkResponseDto>> GetByEnrollmentAsync(int enrollmentId);

        // Get all marks for a specific examination across all classes
        Task<IEnumerable<ExamMarkResponseDto>> GetByExamAsync(int examId);

        // Get one exam mark by ID,or returns null if not found
        Task<ExamMarkResponseDto?> GetByIdAsync(int id);

        // Create a new exam mark
        Task<ExamMarkResponseDto> CreateAsync(CreateExamMarkDto dto, int? loggedInUserId);

        // Update an existing mark
        Task<ExamMarkResponseDto?> UpdateAsync(int id, UpdateExamMarkDto dto, int? loggedInUserId);

        // Delete a mark
        Task<bool> DeleteAsync(int id);
    }
}