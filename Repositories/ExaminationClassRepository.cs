using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Repositories
{
    public class ExaminationClassRepository : IExaminationClassRepository
    {
        private readonly SchoolDbContext _context;

        public ExaminationClassRepository(SchoolDbContext context)
        {
            _context = context;
        }



        // Fetch all examination class records from the ExaminationClasses table
        public async Task<IEnumerable<ExaminationClass>> GetAllAsync()
        {
            return await _context.ExaminationClasses
                .AsNoTracking()
                .Include(ec => ec.Examination)  // loads Examination navigation property
                .Include(ec => ec.Class)  // loads Class navigation property
                .OrderBy(ec => ec.ScheduledDate)
                .ThenBy(ec => ec.ScheduledTime)
                .ThenBy(ec => ec.Class.Grade)
                .ThenBy(ec => ec.Class.Section)
                .ToListAsync();
        }



        // Fetch all classes scheduled for a specific examination
        public async Task<IEnumerable<ExaminationClass>> GetByExamAsync(int examId)
        {
            return await _context.ExaminationClasses
                .AsNoTracking()
                .Include(ec => ec.Examination)
                .Include(ec => ec.Class)
                .Where(ec => ec.ExamId == examId)
                .OrderBy(ec => ec.ScheduledDate)
                .ThenBy(ec => ec.Class.Grade)
                .ThenBy(ec => ec.Class.Section)
                .ToListAsync();
        }



        // Fetch all exams scheduled for a specific class
        public async Task<IEnumerable<ExaminationClass>> GetByClassAsync(int classId)
        {
            return await _context.ExaminationClasses
                .AsNoTracking()
                .Include(ec => ec.Examination)
                .Include(ec => ec.Class)
                .Where(ec => ec.ClassId == classId)
                .OrderBy(ec => ec.ScheduledDate)
                .ThenBy(ec => ec.ScheduledTime)
                .ToListAsync();
        }



        // Fetch a single examination class record by its ID
        public async Task<ExaminationClass?> GetByIdAsync(int id)
        {
            return await _context.ExaminationClasses
                .Include(ec => ec.Examination)
                .Include(ec => ec.Class)
                .FirstOrDefaultAsync(ec => ec.ExamClassId == id);
        }



        // Check whether an exam is already scheduled for a specific clas
        public async Task<bool> ExamClassExistsAsync(int examId, int classId)
        {
            return await _context.ExaminationClasses
                .AnyAsync(ec => ec.ExamId == examId && ec.ClassId == classId);
        }



        // Add a new examination class record to the table in memory
        public async Task AddAsync(ExaminationClass examinationClass)
        {
            await _context.ExaminationClasses.AddAsync(examinationClass);
        }



        // Tell EF Core this examination class object has been modified
        public void Update(ExaminationClass examinationClass)
        {
            _context.ExaminationClasses.Update(examinationClass);
        }



        // Tell EF Core this examination class record should be deleted
        public void Delete(ExaminationClass examinationClass)
        {
            _context.ExaminationClasses.Remove(examinationClass);
        }



        // Send all pending changes to MySQL
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}