using SchoolManagementSystem.DTOs.Examination;

namespace SchoolManagementSystem.Interfaces
{
    public interface IExaminationService
    {
        // Get all examinations across all years
        Task<IEnumerable<ExaminationResponseDto>> GetAllAsync();

        // Get all examinations for a specific academic year
        Task<IEnumerable<ExaminationResponseDto>> GetByYearAsync(int yearId);

        // Get one examination by ID, or returns null if not found
        Task<ExaminationResponseDto?> GetByIdAsync(int id);

        // Create a new examination
        // Throws an exception if an exam with the same name already exists for that year
        Task<ExaminationResponseDto> CreateAsync(CreateExaminationDto dto);

        // Update an existing examination, or returns null if not found
        Task<ExaminationResponseDto?> UpdateAsync(int id, UpdateExaminationDto dto);

        // Delete an examination, returns true if deleted, false if not found
        Task<bool> DeleteAsync(int id);
    }
}