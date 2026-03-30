using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Repositories
{
    public class ManagementStaffRepository : IManagementStaffRepository
    {
        private readonly SchoolDbContext _context;

        // Constructor injection
        public ManagementStaffRepository(SchoolDbContext context)
        {
            _context = context;
        }



        // Fetch all management staff from the ManagementStaffs table
        public async Task<IEnumerable<ManagementStaff>> GetAllAsync()
        {
            return await _context.ManagementStaffs
                .AsNoTracking() //for read only
                .ToListAsync();
        }



        // Fetch a single management staff member by their ID
        public async Task<ManagementStaff?> GetByIdAsync(int id)
        {
            return await _context.ManagementStaffs
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
        }



        // Check whether a NIC already exists in the ManagementStaffs table
        public async Task<bool> NICExistsAsync(string nic)
        {
            return await _context.ManagementStaffs
                .AnyAsync(m => m.NIC == nic);
        }



        // Check whether an email already exists in the ManagementStaffs table
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.ManagementStaffs
                .AnyAsync(m => m.Email == email);
        }



        // Add a new management staff record to the table in memory
        public async Task AddAsync(ManagementStaff staff)
        {
            await _context.ManagementStaffs.AddAsync(staff);
        }



        // Tell EF Core this management staff object has been modified
        public void Update(ManagementStaff staff)
        {
            _context.ManagementStaffs.Update(staff);
        }



        // Tell EF Core this management staff record should be deleted
        public void Delete(ManagementStaff staff)
        {
            _context.ManagementStaffs.Remove(staff);
        }



        // Send all pending changes (inserts, updates, and deletes) to MySQL
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}