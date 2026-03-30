using SchoolManagementSystem.DTOs.ManagementStaff;

namespace SchoolManagementSystem.Interfaces
{
    // defines the business operations available for ManagementStaff
    public interface IManagementStaffService
    {
        // Get all management staff members
        Task<IEnumerable<ManagementStaffResponseDto>> GetAllAsync();

        // Get one management staff member by ID, returns null if not found
        Task<ManagementStaffResponseDto?> GetByIdAsync(int id);

        // Create a new management staff record
        // throws an exception if the NIC or email is already taken
        Task<ManagementStaffResponseDto> CreateAsync(CreateManagementStaffDto dto);

        // Update an existing management staff record, returns null if not found
        Task<ManagementStaffResponseDto?> UpdateAsync(int id, UpdateManagementStaffDto dto);

        // Delete a management staff record
        // returns true if deleted, false if not found
        Task<bool> DeleteAsync(int id);
    }
}