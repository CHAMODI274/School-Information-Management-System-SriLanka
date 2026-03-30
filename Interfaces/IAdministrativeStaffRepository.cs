using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Interfaces
{
    public interface IAdministrativeStaffRepository
    {
        // Get all administrrative staff records from the database
        Task<IEnumerable<AdministrativeStaff>> GetAllAsync();

        // Get one administrrative staff member by their ID, returns null if not found
        Task<AdministrativeStaff?> GetByIdAsync(int id);

        // Check if a NIC number already exists in the database
        Task<bool> NICExistsAsync(string nic);

        // Check if an email already exists in the database
        Task<bool> EmailExistsAsync(string email);

        // Add a new administrrative staff record to the database
        Task AddAsync(AdministrativeStaff staff);

        // Mark a administrrative staff record as modified in memory
        void Update(AdministrativeStaff staff);

        // Mark a administrrative staff record for deletion in memory
        void Delete(AdministrativeStaff staff);

        // Commit all pending changes to the database
        Task SaveChangesAsync();
    }
}