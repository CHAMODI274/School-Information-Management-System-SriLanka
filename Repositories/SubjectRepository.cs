using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly SchoolDbContext _context;

        public SubjectRepository(SchoolDbContext context)
        {
            _context = context;
        }



        // Fetch ALL subjects (all active & inactive) from the Subjects table 
        public async Task<IEnumerable<Subject>> GetAllAsync()
        {
            return await _context.Subjects
                .AsNoTracking()
                .OrderBy(s => s.SubjectName) // Ordered alphabetically by name
                .ToListAsync();
        }



        // Fetch only subjects where IsActive is true
        public async Task<IEnumerable<Subject>> GetAllActiveAsync()
        {
            return await _context.Subjects
                .AsNoTracking()
                .Where(s => s.IsActive)
                .OrderBy(s => s.SubjectName)
                .ToListAsync();
        }



        // Fetch a single subject by its ID
        public async Task<Subject?> GetByIdAsync(int id)
        {
            return await _context.Subjects
                .FirstOrDefaultAsync(s => s.SubjectId == id);
        }



        // Check whether a subject code already exists in the Subjects table
        public async Task<bool> SubjectCodeExistsAsync(string subjectCode)
        {
            return await _context.Subjects
                .AnyAsync(s => s.SubjectCode == subjectCode);
        }



        // Add a new subject record to the table in memory
        public async Task AddAsync(Subject subject)
        {
            await _context.Subjects.AddAsync(subject);
        }



        // Tell EF Core this subject object has been modified
        public void Update(Subject subject)
        {
            _context.Subjects.Update(subject);
        }



        // Tell EF Core this subject record should be deleted
        public void Delete(Subject subject)
        {
            _context.Subjects.Remove(subject);
        }



        // Send all pending changes to MySQL
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}