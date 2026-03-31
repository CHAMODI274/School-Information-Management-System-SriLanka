using SchoolManagementSystem.DTOs.Subject;

namespace SchoolManagementSystem.Interfaces
{
    public interface ISubjectService
    {
        // Get all subjects
        Task<IEnumerable<SubjectResponseDto>> GetAllAsync();

        // Get only active subjects
        Task<IEnumerable<SubjectResponseDto>> GetAllActiveAsync();

        // Get one subject by ID, returns null if not found
        Task<SubjectResponseDto?> GetByIdAsync(int id);

        // Create a new subject
        // Throws an exception if the subject code is already taken
        Task<SubjectResponseDto> CreateAsync(CreateSubjectDto dto);

        // Update an existing subject, returns null if not found
        Task<SubjectResponseDto?> UpdateAsync(int id, UpdateSubjectDto dto);

        // Delete a subject, returns true if deleted, false if not found
        Task<bool> DeleteAsync(int id);
    }
}