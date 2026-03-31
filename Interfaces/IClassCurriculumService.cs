using SchoolManagementSystem.DTOs.ClassCurriculum;

namespace SchoolManagementSystem.Interfaces
{
    public interface IClassCurriculumService
    {
        // Get all curriculum entries
        Task<IEnumerable<ClassCurriculumResponseDto>> GetAllAsync();

        // Get all curriculum entries for a specific class
        Task<IEnumerable<ClassCurriculumResponseDto>> GetByClassAsync(int classId);

        // Get all curriculum entries for a specific academic year
        Task<IEnumerable<ClassCurriculumResponseDto>> GetByYearAsync(int yearId);

        // Get one curriculum entry by ID, or returns null if not found
        Task<ClassCurriculumResponseDto?> GetByIdAsync(int id);

        // Create a new curriculum entry linking a Class + Subject + Year
        // Throws an exception if the same combination already exists
        Task<ClassCurriculumResponseDto> CreateAsync(CreateClassCurriculumDto dto);

        // Update the subject of an existing curriculum entry, or Returns null if not found
        Task<ClassCurriculumResponseDto?> UpdateAsync(int id, UpdateClassCurriculumDto dto);

        // Delete a curriculum entry, returns true if deleted, false if not found
        Task<bool> DeleteAsync(int id);
    }
}