using SchoolManagementSystem.DTOs.AcademicYear;

namespace SchoolManagementSystem.Interfaces
{
    public interface IAcademicYearService
    {
        // Get all academic years
        Task<IEnumerable<AcademicYearResponseDto>> GetAllAsync();

        // Get one academic year by ID, returns null if not found
        Task<AcademicYearResponseDto?> GetByIdAsync(int id);

        // Get the currently active academic year, returns null if none is active
        Task<AcademicYearResponseDto?> GetActiveAsync();

        // Create a new academic year
        // Throws an exception if the year label already exists e.g. "2025"
        Task<AcademicYearResponseDto> CreateAsync(CreateAcademicYearDto dto);

        // Update an existing academic year
        // If IsActive is set to true, all other years are automatically deactivated
        // Returns null if not found
        Task<AcademicYearResponseDto?> UpdateAsync(int id, UpdateAcademicYearDto dto);

        // Delete an academic year, returns true if deleted, false if not found
        Task<bool> DeleteAsync(int id);
    }
}