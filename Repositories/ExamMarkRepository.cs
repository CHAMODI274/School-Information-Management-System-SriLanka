using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Repositories
{
    public class ExamMarkRepository : IExamMarkRepository
    {
        private readonly SchoolDbContext _context;

        public ExamMarkRepository(SchoolDbContext context)
        {
            _context = context;
        }



        // Fetch all marks for a specific exam and class
        public async Task<IEnumerable<ExamMark>> GetByExamAndClassAsync(int examId, int classId)
        {
            return await _context.ExamMarks
                .AsNoTracking()
                .Include(em => em.Examination) // for ExamName
                .Include(em => em.Enrollment)
                    .ThenInclude(e => e.Student) // for StudentName
                .Include(em => em.ClassCurriculum)
                    .ThenInclude(cc => cc.Subject) // for SubjectName
                .Include(em => em.Teacher) // for TeacherName
                .Where(em => em.ExamId == examId && em.Enrollment.ClassId == classId)
                .OrderBy(em => em.Enrollment.Student.FullName)
                .ThenBy(em => em.ClassCurriculum.Subject.SubjectName)
                .ToListAsync();
        }



        // Fetch all marks for a specific enrollment
        public async Task<IEnumerable<ExamMark>> GetByEnrollmentAsync(int enrollmentId)
        {
            return await _context.ExamMarks
                .AsNoTracking()
                .Include(em => em.Examination)
                .Include(em => em.Enrollment)
                    .ThenInclude(e => e.Student)
                .Include(em => em.ClassCurriculum)
                    .ThenInclude(cc => cc.Subject)
                .Include(em => em.Teacher)
                .Where(em => em.EnrollmentId == enrollmentId)
                .OrderBy(em => em.Examination.ExamName)
                .ThenBy(em => em.ClassCurriculum.Subject.SubjectName)
                .ToListAsync();
        }



        // Fetch all marks for a specific examination across all classes
        public async Task<IEnumerable<ExamMark>> GetByExamAsync(int examId)
        {
            return await _context.ExamMarks
                .AsNoTracking()
                .Include(em => em.Examination)
                .Include(em => em.Enrollment)
                    .ThenInclude(e => e.Student)
                .Include(em => em.Enrollment)
                    .ThenInclude(e => e.Class) // for Grade and Section
                .Include(em => em.ClassCurriculum)
                    .ThenInclude(cc => cc.Subject)
                .Include(em => em.Teacher)
                .Where(em => em.ExamId == examId)
                .OrderBy(em => em.Enrollment.Class.Grade)
                .ThenBy(em => em.Enrollment.Class.Section)
                .ThenBy(em => em.Enrollment.Student.FullName)
                .ToListAsync();
        }



        // Fetch a single exam mark by its ID
        public async Task<ExamMark?> GetByIdAsync(int id)
        {
            return await _context.ExamMarks
                .Include(em => em.Examination)
                .Include(em => em.Enrollment)
                    .ThenInclude(e => e.Student)
                .Include(em => em.ClassCurriculum)
                    .ThenInclude(cc => cc.Subject)
                .Include(em => em.Teacher)
                .FirstOrDefaultAsync(em => em.MarkId == id);
        }



        // Check whether a mark already exists for a student for a subject in an exam
        public async Task<bool> MarkExistsAsync(
            int enrollmentId, int classCurriculumId, int examId)
        {
            return await _context.ExamMarks
                .AnyAsync(em => em.EnrollmentId == enrollmentId &&
                                em.ClassCurriculumId == classCurriculumId &&
                                em.ExamId == examId);
        }



        // Fetch the Examination record by ID
        public async Task<Examination?> GetExaminationByIdAsync(int examId)
        {
            return await _context.Examinations
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.ExamId == examId);
        }



        // Find a Teacher record by looking up their UserId
        public async Task<Teacher?> GetTeacherByUserIdAsync(int userId)
        {
            return await _context.Teachers
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.UserId == userId);
        }



        // Check whether a teacher is allocated to teach a specific subject in a class
        public async Task<bool> IsTeacherAllocatedToSubjectAsync(int classCurriculumId, int teacherId)
        {
            return await _context.SubjectAllocations
                .AnyAsync(sa => sa.ClassCurriculumId == classCurriculumId &&
                                sa.TeacherId  == teacherId);
        }



        // Add a new exam mark record to the table in memory
        public async Task AddAsync(ExamMark examMark)
        {
            await _context.ExamMarks.AddAsync(examMark);
        }



        // Tell EF Core this exam mark object has been modified
        public void Update(ExamMark examMark)
        {
            _context.ExamMarks.Update(examMark);
        }



        // Tell EF Core this exam mark record should be deleted
        public void Delete(ExamMark examMark)
        {
            _context.ExamMarks.Remove(examMark);
        }



        // Send all pending changes to MySQL
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}