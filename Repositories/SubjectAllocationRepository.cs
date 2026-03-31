using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Repositories
{
    public class SubjectAllocationRepository : ISubjectAllocationRepository
    {
        private readonly SchoolDbContext _context;

        public SubjectAllocationRepository(SchoolDbContext context)
        {
            _context = context;
        }



        // Fetch all allocations from the SubjectAllocations table
        public async Task<IEnumerable<SubjectAllocation>> GetAllAsync()
        {
            return await _context.SubjectAllocations
                .AsNoTracking()

                .Include(sa => sa.ClassCurriculum) // load ClassCurriculum first
                    .ThenInclude(cc => cc.Class)  // then load Class inside it

                .Include(sa => sa.ClassCurriculum)   // load ClassCurriculum again
                    .ThenInclude(cc => cc.Subject)  // then load Subject inside it

                .Include(sa => sa.ClassCurriculum)       // load ClassCurriculum again
                    .ThenInclude(cc => cc.AcademicYear) // then load AcademicYear inside it

                .Include(sa => sa.Teacher)   // load Teacher directly

                .OrderBy(sa => sa.ClassCurriculum.Class.Grade)
                .ThenBy(sa => sa.ClassCurriculum.Class.Section)
                .ThenBy(sa => sa.ClassCurriculum.Subject.SubjectName)
                .ToListAsync();
        }



        // Fetch all allocations for a specific teacher
        public async Task<IEnumerable<SubjectAllocation>> GetByTeacherAsync(int teacherId)
        {
            return await _context.SubjectAllocations
                .AsNoTracking()
                .Include(sa => sa.ClassCurriculum)
                    .ThenInclude(cc => cc.Class)
                .Include(sa => sa.ClassCurriculum)
                    .ThenInclude(cc => cc.Subject)
                .Include(sa => sa.ClassCurriculum)
                    .ThenInclude(cc => cc.AcademicYear)
                .Include(sa => sa.Teacher)
                .Where(sa => sa.TeacherId == teacherId)
                .OrderBy(sa => sa.ClassCurriculum.Class.Grade)
                .ThenBy(sa => sa.ClassCurriculum.Subject.SubjectName)
                .ToListAsync();
        }



        // Fetch all allocations for a specific curriculum entry
        public async Task<IEnumerable<SubjectAllocation>> GetByCurriculumAsync(int classCurriculumId)
        {
            return await _context.SubjectAllocations
                .AsNoTracking()
                .Include(sa => sa.ClassCurriculum)
                    .ThenInclude(cc => cc.Class)
                .Include(sa => sa.ClassCurriculum)
                    .ThenInclude(cc => cc.Subject)
                .Include(sa => sa.ClassCurriculum)
                    .ThenInclude(cc => cc.AcademicYear)
                .Include(sa => sa.Teacher)
                .Where(sa => sa.ClassCurriculumId == classCurriculumId)
                .ToListAsync();
        }



        // Fetch a single allocation by its ID
        public async Task<SubjectAllocation?> GetByIdAsync(int id)
        {
            return await _context.SubjectAllocations
                .Include(sa => sa.ClassCurriculum)
                    .ThenInclude(cc => cc.Class)
                .Include(sa => sa.ClassCurriculum)
                    .ThenInclude(cc => cc.Subject)
                .Include(sa => sa.ClassCurriculum)
                    .ThenInclude(cc => cc.AcademicYear)
                .Include(sa => sa.Teacher)
                .FirstOrDefaultAsync(sa => sa.AllocationId == id);
        }



        // Check whether a teacher is already allocated to a specific curriculum entry
        public async Task<bool> AllocationExistsAsync(int classCurriculumId, int teacherId)
        {
            return await _context.SubjectAllocations
                .AnyAsync(sa => sa.ClassCurriculumId == classCurriculumId &&
                                sa.TeacherId == teacherId);
        }



        // Add a new allocation record to the table in memory
        public async Task AddAsync(SubjectAllocation subjectAllocation)
        {
            await _context.SubjectAllocations.AddAsync(subjectAllocation);
        }



        // Tell EF Core this allocation object has been modified
        public void Update(SubjectAllocation subjectAllocation)
        {
            _context.SubjectAllocations.Update(subjectAllocation);
        }



        // Tell EF Core this allocation record should be deleted
        public void Delete(SubjectAllocation subjectAllocation)
        {
            _context.SubjectAllocations.Remove(subjectAllocation);
        }



        // Send all pending changes to MySQL
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}