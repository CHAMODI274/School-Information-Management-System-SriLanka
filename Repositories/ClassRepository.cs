using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly SchoolDbContext _context;

        public ClassRepository(SchoolDbContext context)
        {
            _context = context;
        }



        // Fetch all classes from the Classes table
        // .Include() loads the related Teacher and AcademicYear records at the same time (eager loading)
        public async Task<IEnumerable<Class>> GetAllAsync()
        {
            return await _context.Classes
                .AsNoTracking()
                .Include(c => c.Teacher) // loads the Teacher navigation property
                .Include(c => c.AcademicYear) // loads the AcademicYear navigation property
                .OrderBy(c => c.Grade)
                .ThenBy(c => c.Section)
                .ToListAsync();
        }



        // Fetch all classes that belong to a specific academic year
        public async Task<IEnumerable<Class>> GetByYearAsync(int yearId)
        {
            return await _context.Classes
                .AsNoTracking()
                .Include(c => c.Teacher) // loads the Teacher
                .Include(c => c.AcademicYear) // loads the AcademicYear
                .Where(c => c.YearId == yearId) // .Where() filters to only classes matching the given yearId
                .OrderBy(c => c.Grade)
                .ThenBy(c => c.Section)
                .ToListAsync();
        }



        // Fetch a single class by its ID
        public async Task<Class?> GetByIdAsync(int id)
        {
            return await _context.Classes
                .Include(c => c.Teacher)
                .Include(c => c.AcademicYear)
                .FirstOrDefaultAsync(c => c.ClassId == id);
        }



        // Check whether a class with the same Grade, Section and Year already exists
        public async Task<bool> ClassExistsAsync(string grade, string section, int yearId)
        {
            return await _context.Classes
                .AnyAsync(c => c.Grade == grade &&
                               c.Section == section &&
                               c.YearId == yearId);
        }



        // Add a new class record to the table in memory
        public async Task AddAsync(Class cls)
        {
            await _context.Classes.AddAsync(cls);
        }



        // Tell EF Core this class object has been modified
        public void Update(Class cls)
        {
            _context.Classes.Update(cls);
        }



        // Tell EF Core this class record should be deleted
        public void Delete(Class cls)
        {
            _context.Classes.Remove(cls);
        }



        // Send all pending changes to MySQL
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }

}