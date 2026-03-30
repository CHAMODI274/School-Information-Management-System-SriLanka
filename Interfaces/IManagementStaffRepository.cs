using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Interfaces
{
    public interface IManagementStaffRepository
    {
        Task<IEnumerable<ManagementStaff>> GetAllAsync();
        Task<ManagementStaff?> GetByIdAsync(int id);
        Task<bool> NICExistsAsync(string nic);
        Task<bool> EmailExistsAsync(string email);
        Task AddAsync(ManagementStaff staff);
        void Update(ManagementStaff staff);
        void Delete(ManagementStaff staff);
        Task SaveChangesAsync();
    }
}