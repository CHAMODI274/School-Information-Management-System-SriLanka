using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Repositories
{
    public class ExaminationRepository : IExaminationRepository
    {
        private readonly SchoolDbContext _context;

        public ExaminationRepository(SchoolDbContext context)
        {
            _context = context;
        }



        // Fetch all examinations from the Examinations table
        public async Task<IEnumerable<Examination>> GetAllAsync()
        {
            return await _context.Examinations
                .AsNoTracking()
                .Include(e => e.AcademicYear) // loads AcademicYear navigation property
                .OrderByDescending(e => e.AcademicYear.Year)
                .ThenBy(e => e.ExamName)
                .ToListAsync();
        }



        // Fetch all examinations for a specific academic year
        public async Task<IEnumerable<Examination>> GetByYearAsync(int yearId)
        {
            return await _context.Examinations
                .AsNoTracking()
                .Include(e => e.AcademicYear)
                .Where(e => e.YearId == yearId)
                .OrderBy(e => e.ExamName)
                .ToListAsync();
        }



        // Fetch a single examination by its ID
        public async Task<Examination?> GetByIdAsync(int id)
        {
            return await _context.Examinations
                .Include(e => e.AcademicYear)
                .FirstOrDefaultAsync(e => e.ExamId == id);
        }



        // Check whether an exam with the same name already exists for the same year
        public async Task<bool> ExamExistsAsync(string examName, int yearId)
        {
            return await _context.Examinations
                .AnyAsync(e => e.ExamName == examName &&
                               e.YearId == yearId);
        }



        // Add a new examination record to the table in memory
        public async Task AddAsync(Examination examination)
        {
            await _context.Examinations.AddAsync(examination);
        }



        // Tell EF Core this examination object has been modified
        public void Update(Examination examination)
        {
            _context.Examinations.Update(examination);
        }



        // Tell EF Core this examination record should be deleted
        public void Delete(Examination examination)
        {
            _context.Examinations.Remove(examination);
        }



        // Send all pending changes to MySQL
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }    
}