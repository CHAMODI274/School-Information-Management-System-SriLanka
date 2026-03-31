using SchoolManagementSystem.DTOs.SubjectAllocation;

namespace SchoolManagementSystem.Interfaces
{
    public interface ISubjectAllocationService
    {
        // Get all subject allocations
        Task<IEnumerable<SubjectAllocationResponseDto>> GetAllAsync();

        // Get all allocations for a specific teacher
        Task<IEnumerable<SubjectAllocationResponseDto>> GetByTeacherAsync(int teacherId);

        // Get all allocations for a specific curriculum entry
        Task<IEnumerable<SubjectAllocationResponseDto>> GetByCurriculumAsync(int classCurriculumId);

        // Get one allocation by ID, or returns null if not found
        Task<SubjectAllocationResponseDto?> GetByIdAsync(int id);

        // Create a new allocation linking a Teacher to a ClassCurriculum entry
        // Throws an exception if the same teacher is already allocated to the same curriculum entry
        Task<SubjectAllocationResponseDto> CreateAsync(CreateSubjectAllocationDto dto);

        // Update an allocation
        // only the Teacher can be changed
        // Returns null if not found
        Task<SubjectAllocationResponseDto?> UpdateAsync(int id, UpdateSubjectAllocationDto dto);

        // Delete an allocation, returns true if deleted, false if not found
        Task<bool> DeleteAsync(int id);
    }
}