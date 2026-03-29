using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly SchoolDbContext _context;

        public TeacherRepository(SchoolDbContext context)
        {
            _context = context;
        }


        // Fetch all teachers from the Teachers table
        public async Task<IEnumerable<Teacher>> GetAllAsync()
        {
            return await _context.Teachers
                .AsNoTracking() // only for reading
                .ToListAsync();
        }


        // Fetch a single teacher by their ID
        public async Task<Teacher?> GetByIdAsync(int id)
        {
            return await _context.Teachers
                .FirstOrDefaultAsync(t => t.TeacherId == id);
        }


        // Check whether a NIC already exists in the Teachers table
        public async Task<bool> NICExistsAsync(string nic)
        {
            return await _context.Teachers
                .AnyAsync(t => t.NIC == nic);
                // AnyAsync() stops as soon as it finds one match. Does not load the full teacher record
        }


        // Check whether an email already exists in the Teachers table
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Teachers
                .AnyAsync(t => t.Email == email);
        }


        // Add a new teacher to the Teachers table in memory
        public async Task AddAsync(Teacher teacher)
        {
            await _context.Teachers.AddAsync(teacher);
        }


        // Tell EF Core this teacher object has been modified
        public void Update(Teacher teacher)
        {
            _context.Teachers.Update(teacher);
        }


        // Tell EF Core this teacher should be deleted
        public void Delete(Teacher teacher)
        {
            _context.Teachers.Remove(teacher);
        }


        // Send all pending changes (inserts, updates, and deletes) to MySQL
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}