using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Repositories
{
    public class AdministrativeStaffRepositiry : IAdministrativeStaffRepository
    {
        private readonly SchoolDbContext _context;

        public AdministrativeStaffRepositiry(SchoolDbContext context)
        {
            _context = context;
        }



        // Fetch all administrative staff from the NonAcademicStaffs table
        public async Task<IEnumerable<AdministrativeStaff>> GetAllAsync()
        {
            return await _context.AdministrativeStaffs
                .AsNoTracking()
                .ToListAsync();
        }



        // Fetch a single administrative staff member by their ID
        public async Task<AdministrativeStaff?> GetByIdAsync(int id)
        {
            return await _context.AdministrativeStaffs
                .FirstOrDefaultAsync(n => n.EmployeeId == id);
        }



        // Check whether a NIC already exists in the AdministrativecStaffs table
        public async Task<bool> NICExistsAsync(string nic)
        {
            return await _context.AdministrativeStaffs
                .AnyAsync(n => n.NIC == nic);
        }




        // Check whether an email already exists in the AdministrativeStaffs table
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.AdministrativeStaffs
                .AnyAsync(n => n.Email == email);
        }



        // Add a new administrative staff record to the table in memory
        public async Task AddAsync(AdministrativeStaff staff)
        {
            await _context.AdministrativeStaffs.AddAsync(staff);
        }



        // Tell EF Core this non-academic staff object has been modified
        public void Update(AdministrativeStaff staff)
        {
            _context.AdministrativeStaffs.Update(staff);
        }



        // Tell EF Core this non-academic staff record should be deleted
        public void Delete(AdministrativeStaff staff)
        {
            _context.AdministrativeStaffs.Remove(staff);
        }



        // Send all pending changes to MySQL
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}