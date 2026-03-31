using SchoolManagementSystem.DTOs.Class;

namespace SchoolManagementSystem.Interfaces
{
    // business operations available for Class
    public interface IClassService
    {
        // Get all classes
        Task<IEnumerable<ClassResponseDto>> GetAllAsync();

        // Get all classes for a specific academic year
        Task<IEnumerable<ClassResponseDto>> GetByYearAsync(int yearId);

        // Get one class by ID, returns null if not found
        Task<ClassResponseDto?> GetByIdAsync(int id);

        // Create a new class
        // Throws an exception if the same Grade + Section + Year already exists
        Task<ClassResponseDto> CreateAsync(CreateClassDto dto);

        // Update an existing class, returns null if not found
        Task<ClassResponseDto?> UpdateAsync(int id, UpdateClassDto dto);

        // Delete a class, returns true if deleted, false if not found
        Task<bool> DeleteAsync(int id);
    }
}