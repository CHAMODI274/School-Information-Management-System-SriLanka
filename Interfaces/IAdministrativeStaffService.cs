using SchoolManagementSystem.DTOs.AdministrativeStaff;

namespace SchoolManagementSystem.Interfaces
{
    // Defines the business operations
    public interface IAdministrativeStaffService
    {
        // Get all administrative staff members
        Task<IEnumerable<AdministrativeStaffResponseDto>> GetAllAsync();

        // Get one adminnistrative staff member by ID, returns null if not found
        Task<AdministrativeStaffResponseDto?> GetByIdAsync(int id);

        // Create a new administrative staff record
        // Throws an exception if the NIC or email is already taken
        Task<AdministrativeStaffResponseDto> CreateAsync(CreateAdministrativeStaffDto dto);

        // Update an existing administrative staff record, returns null if not found
        Task<AdministrativeStaffResponseDto?> UpdateAsync(int id, UpdateAdministrativeStaffDto dto);

        // Delete a administrative staff record
        // Returns true if deleted, false if not found
        Task<bool> DeleteAsync(int id);
    }
}