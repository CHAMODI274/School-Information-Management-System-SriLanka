using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Repositories
{
    public class AcademicYearRepository : IAcademicYearRepository
    {
        // SchoolDbContext gives us access to all database tables
        private readonly SchoolDbContext _context;

        // Constructor injection
        public AcademicYearRepository(SchoolDbContext context)
        {
            _context = context;
        }



        // Fetch all academic years from the AcademicYears table
        public async Task<IEnumerable<AcademicYear>> GetAllAsync()
        {
            return await _context.AcademicYears
                .AsNoTracking() // only for read
                .OrderByDescending(a => a.Year) // Ordered by year descending -> the most recent year appears first
                .ToListAsync();
        }



        // Fetch a single academic year by its ID
        public async Task<AcademicYear?> GetByIdAsync(int id)
        {
            return await _context.AcademicYears
                .FirstOrDefaultAsync(a => a.YearId == id);
        }



        // Fetch the currently active academic year
        public async Task<AcademicYear?> GetActiveAsync()
        {
            return await _context.AcademicYears
                .AsNoTracking() 
                .FirstOrDefaultAsync(a => a.IsActive);
        }



        // Fetch all years that are currently marked as active
        public async Task<IEnumerable<AcademicYear>> GetAllActiveAsync()
        {
            return await _context.AcademicYears
                .Where(a => a.IsActive)
                .ToListAsync();
        }



        // Check whether a year label already exists e.g. "2025"
        public async Task<bool> YearExistsAsync(string year)
        {
            return await _context.AcademicYears
                .AnyAsync(a => a.Year == year);
        }



        // Add a new academic year record to the table in memory
        public async Task AddAsync(AcademicYear academicYear)
        {
            await _context.AcademicYears.AddAsync(academicYear);
        }



        // Tell EF Core this academic year object has been modified
        public void Update(AcademicYear academicYear)
        {
            _context.AcademicYears.Update(academicYear);
        }



        // Tell EF Core this academic year should be deleted
        public void Delete(AcademicYear academicYear)
        {
            _context.AcademicYears.Remove(academicYear);
        }



        // Send all pending changes (inserts, updates, and deletes) to MySQL
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}