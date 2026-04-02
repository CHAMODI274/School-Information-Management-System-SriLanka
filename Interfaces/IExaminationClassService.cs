using SchoolManagementSystem.DTOs.ExaminationClass;

namespace SchoolManagementSystem.Interfaces
{
    public interface IExaminationClassService
    {
        // Get all examination class records
        Task<IEnumerable<ExaminationClassResponseDto>> GetAllAsync();

        // Get all classes scheduled for a specific examination
        Task<IEnumerable<ExaminationClassResponseDto>> GetByExamAsync(int examId);

        // Get all exams scheduled for a specific class
        Task<IEnumerable<ExaminationClassResponseDto>> GetByClassAsync(int classId);

        // Get one examination class record by ID, or returns null if not found
        Task<ExaminationClassResponseDto?> GetByIdAsync(int id);

        // Create a new examination class record
        // Throws an exception if the same exam is already scheduled for that class
        Task<ExaminationClassResponseDto> CreateAsync(CreateExaminationClassDto dto);

        // Update the scheduled date and time for an examination class record, returns null if not found
        Task<ExaminationClassResponseDto?> UpdateAsync(int id, UpdateExaminationClassDto dto);

        // Delete an examination class record, returns true if deleted, false if not found
        Task<bool> DeleteAsync(int id);
    }
}