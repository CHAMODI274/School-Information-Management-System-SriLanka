using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Repositories
{
    public class ClassCurriculumRepository : IClassCurriculumRepository
    {
        private readonly SchoolDbContext _context;

        public ClassCurriculumRepository(SchoolDbContext context)
        {
            _context = context;
        }



        // Fetch all curriculum entries from the ClassCurriculums table
        public async Task<IEnumerable<ClassCurriculum>> GetAllAsync()
        {
            return await _context.ClassCurriculums
                .AsNoTracking()
                .Include(cc => cc.Class) // loads Class navigation property
                .Include(cc => cc.Subject)  // loads Subject navigation property
                .Include(cc => cc.AcademicYear)  // loads AcademicYear navigation property
                .OrderBy(cc => cc.Class.Grade)
                .ThenBy(cc => cc.Class.Section)
                .ThenBy(cc => cc.Subject.SubjectName)
                .ToListAsync();
        }



        // Fetch all curriculum entries for a specific class
        public async Task<IEnumerable<ClassCurriculum>> GetByClassAsync(int classId)
        {
            return await _context.ClassCurriculums
                .AsNoTracking()
                .Include(cc => cc.Class)
                .Include(cc => cc.Subject)
                .Include(cc => cc.AcademicYear)
                .Where(cc => cc.ClassId == classId)
                .OrderBy(cc => cc.Subject.SubjectName)
                .ToListAsync();
        }



        // Fetch all curriculum entries for a specific academic year
        public async Task<IEnumerable<ClassCurriculum>> GetByYearAsync(int yearId)
        {
            return await _context.ClassCurriculums
                .AsNoTracking()
                .Include(cc => cc.Class)
                .Include(cc => cc.Subject)
                .Include(cc => cc.AcademicYear)
                .Where(cc => cc.YearId == yearId)
                .OrderBy(cc => cc.Class.Grade)
                .ThenBy(cc => cc.Class.Section)
                .ThenBy(cc => cc.Subject.SubjectName)
                .ToListAsync();
        }



        // Fetch a single curriculum entry by its ID
        public async Task<ClassCurriculum?> GetByIdAsync(int id)
        {
            return await _context.ClassCurriculums
                .Include(cc => cc.Class)
                .Include(cc => cc.Subject)
                .Include(cc => cc.AcademicYear)
                .FirstOrDefaultAsync(cc => cc.ClassCurriculumId == id);
        }



        // Check whether a subject is already assigned to a class for a specific year
        public async Task<bool> EntryExistsAsync(int classId, int subjectId, int yearId)
        {
            return await _context.ClassCurriculums
                .AnyAsync(cc => cc.ClassId == classId &&
                                cc.SubjectId == subjectId &&
                                cc.YearId == yearId);
        }



        // Add a new curriculum entry to the table in memory
        public async Task AddAsync(ClassCurriculum classCurriculum)
        {
            await _context.ClassCurriculums.AddAsync(classCurriculum);
        }



        // Tell EF Core this curriculum entry has been modified
        public void Update(ClassCurriculum classCurriculum)
        {
            _context.ClassCurriculums.Update(classCurriculum);
        }




        // Tell EF Core this curriculum entry should be deleted
        public void Delete(ClassCurriculum classCurriculum)
        {
            _context.ClassCurriculums.Remove(classCurriculum);
        }



        // Send all pending changes to MySQL
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }    
}