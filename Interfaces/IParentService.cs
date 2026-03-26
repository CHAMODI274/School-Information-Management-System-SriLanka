using SchoolManagementSystem.DTOs.Parent;

namespace SchoolManagementSystem.Interfaces
{
    public interface IParentService
    {
        // Get all parents
        Task<IEnumerable<ParentResponseDto>> GetAllAsync();

        // Get one parent by ID, returns null if not found
        Task<ParentResponseDto?> GetByIdAsync(int id);

        // Create a new parent record and return the saved data
        Task<ParentResponseDto> CreateAsync(CreateParentDto dto);

        // Update an existing parent, returns null if not found
        Task<ParentResponseDto?> UpdateAsync(int id, UpdateParentDto dto);

        // Delete a parent, returns true if deleted, false if not found
        Task<bool> DeleteAsync(int id);
    }
}