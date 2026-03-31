using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly SchoolDbContext _context;

        public EnrollmentRepository(SchoolDbContext context)
        {
            _context = context;
        }



        // Fetch all enrollments from the Enrollments table
        public async Task<IEnumerable<Enrollment>> GetAllAsync()
        {
            return await _context.Enrollments
                .AsNoTracking()
                .Include(e => e.Student) // loads Student navigation property
                .Include(e => e.Class)  // loads Class navigation property
                .Include(e => e.AcademicYear)  // loads AcademicYear navigation property
                .OrderBy(e => e.AcademicYear.Year)
                .ThenBy(e => e.Class.Grade)
                .ThenBy(e => e.Student.FullName)
                .ToListAsync();
        }



        // Fetch all enrollments for a specific class
        public async Task<IEnumerable<Enrollment>> GetByClassAsync(int classId)
        {
            return await _context.Enrollments
                .AsNoTracking()
                .Include(e => e.Student)
                .Include(e => e.Class)
                .Include(e => e.AcademicYear)
                .Where(e => e.ClassId == classId)
                .OrderBy(e => e.Student.FullName)
                .ToListAsync();
        }



        // Fetch all enrollments for a specific student
        public async Task<IEnumerable<Enrollment>> GetByStudentAsync(int studentId)
        {
            return await _context.Enrollments
                .AsNoTracking()
                .Include(e => e.Student)
                .Include(e => e.Class)
                .Include(e => e.AcademicYear)
                .Where(e => e.StudentId == studentId)
                .OrderByDescending(e => e.AcademicYear.Year) // Ordered by year
                .ToListAsync();
        }



        // Fetch a single enrollment by its ID
        public async Task<Enrollment?> GetByIdAsync(int id)
        {
            return await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Class)
                .Include(e => e.AcademicYear)
                .FirstOrDefaultAsync(e => e.EnrollmentId == id);
        }



        // Check whether a student is already enrolled for a specific academic year
        public async Task<bool> EnrollmentExistsAsync(int studentId, int yearId)
        {
            return await _context.Enrollments
                .AnyAsync(e => e.StudentId == studentId &&
                               e.YearId == yearId);
        }



        // Add a brand new enrollment record to the table in memory
        public async Task AddAsync(Enrollment enrollment)
        {
            await _context.Enrollments.AddAsync(enrollment);
        }



        // Tell EF Core this enrollment object has been modified
        public void Update(Enrollment enrollment)
        {
            _context.Enrollments.Update(enrollment);
        }



        // Tell EF Core this enrollment record should be deleted
        public void Delete(Enrollment enrollment)
        {
            _context.Enrollments.Remove(enrollment);
        }



        // Send all pending changes to MySQL
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}