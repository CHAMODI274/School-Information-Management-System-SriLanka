using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Interfaces
{
    public interface IManagementStaffRepository
    {
        //Get all management staff records from the database
        Task<IEnumerable<ManagementStaff>> GetAllAsync();

        // Get one management staff member by their ID, returns null if not found
        Task<ManagementStaff?> GetByIdAsync(int id);

        // Check if a NIC number already exists in the database
        Task<bool> NICExistsAsync(string nic);

        // Check if an email already exists in the database
        Task<bool> EmailExistsAsync(string email);


        // Add a new management staff record to the database
        Task AddAsync(ManagementStaff staff);

        // Mark a management staff record as modified in memory
        void Update(ManagementStaff staff);        
        
        // Mark a management staff record for deletion in memory
        void Delete(ManagementStaff staff);
        
        
        // Commit all pending changes to the database
        Task SaveChangesAsync();
    }
}