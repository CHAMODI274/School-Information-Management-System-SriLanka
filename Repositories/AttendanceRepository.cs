using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly SchoolDbContext _context;

        public AttendanceRepository(SchoolDbContext context)
        {
            _context = context;
        }



        // 1. Fetch all attendance records for a specific enrollment
        public async Task<IEnumerable<Attendance>> GetByEnrollmentAsync(int enrollmentId)
        {
            return await _context.Attendances
                .AsNoTracking()
                .Include(a => a.Enrollment)
                    .ThenInclude(e => e.Student) // for StudentName
                .Include(a => a.Enrollment)
                    .ThenInclude(e => e.Class) // for Grade and Section in summary
                .Include(a => a.Enrollment)
                    .ThenInclude(e => e.AcademicYear) // for Year in summary
                .Include(a => a.Teacher) // for TeacherName
                .Where(a => a.EnrollmentId == enrollmentId)
                .OrderBy(a => a.Date)  
                .ToListAsync();
        }



        // 2. Fetch all attendance records for a specific class on a specific date
        public async Task<IEnumerable<Attendance>> GetByClassAndDateAsync(
            int classId, DateTime date)
        {
            return await _context.Attendances
                .AsNoTracking()
                .Include(a => a.Enrollment)
                    .ThenInclude(e => e.Student)
                .Include(a => a.Teacher)
                .Where(a => a.Enrollment.ClassId == classId &&
                            a.Date.Date == date.Date)
                .OrderBy(a => a.Enrollment.Student.FullName)
                .ToListAsync();
        }



        // 3. Fetch all attendance records for a class within a specific month and year
        public async Task<IEnumerable<Attendance>> GetByClassAndMonthAsync(
            int classId, int month, int year)
        {
            return await _context.Attendances
                .AsNoTracking()
                .Include(a => a.Enrollment)
                    .ThenInclude(e => e.Student)
                .Include(a => a.Teacher)
                .Where(a => a.Enrollment.ClassId == classId &&
                            a.Date.Month == month &&
                            a.Date.Year == year)
                .OrderBy(a => a.Date)
                .ThenBy(a => a.Enrollment.Student.FullName)
                .ToListAsync();
        }



        // 4. Fetch all attendance records across every class in the school on a date
        public async Task<IEnumerable<Attendance>> GetBySchoolAndDateAsync(DateTime date)
        {
            return await _context.Attendances
                .AsNoTracking()
                .Include(a => a.Enrollment)
                    .ThenInclude(e => e.Student)
                .Include(a => a.Enrollment)
                    .ThenInclude(e => e.Class)
                .Include(a => a.Teacher)
                .Where(a => a.Date.Date == date.Date)
                .OrderBy(a => a.Enrollment.Class.Grade)
                .ThenBy(a => a.Enrollment.Class.Section)
                .ThenBy(a => a.Enrollment.Student.FullName)
                .ToListAsync();
        }



        // 5. Fetch a single attendance record by its ID
        public async Task<Attendance?> GetByIdAsync(int id)
        {
            return await _context.Attendances
                .Include(a => a.Enrollment)
                    .ThenInclude(e => e.Student)
                .Include(a => a.Teacher)
                .FirstOrDefaultAsync(a => a.AttendanceId == id);
        }



        // 6. Check whether an attendance record already exists for a specific enrollment on a specific date
        public async Task<bool> AttendanceExistsAsync(int enrollmentId, DateTime date)
        {
            return await _context.Attendances
                .AnyAsync(a => a.EnrollmentId == enrollmentId &&
                               a.Date.Date == date.Date);
        }



        // 7. Count how many days a student was Present vs Absent for an enrollment
        public async Task<(int present, int absent)> GetAttendanceSummaryAsync(
            int enrollmentId)
        {
            // Count all Present records for this enrollment
            var present = await _context.Attendances
                .CountAsync(a => a.EnrollmentId == enrollmentId &&
                                 a.Status == Models.Enums.AttendanceStatus.Present);

            // Count all Absent records for this enrollment
            var absent = await _context.Attendances
                .CountAsync(a => a.EnrollmentId == enrollmentId &&
                                 a.Status == Models.Enums.AttendanceStatus.Absent);

            // Return both counts as a tuple
            return (present, absent);
        }



        // 8. Find a Teacher record by looking up their UserId
        public async Task<Teacher?> GetTeacherByUserIdAsync(int userId)
        {
            return await _context.Teachers
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.UserId == userId);
        }



        // 9. Find the ClassId for a specific enrollment
        public async Task<int?> GetClassIdByEnrollmentAsync(int enrollmentId)
        {
            // Find the enrollment and return its ClassId
            var enrollment = await _context.Enrollments
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);

            return enrollment?.ClassId; // return null if the enrollment does not exist
        }




        // 10. Get a Class record by ClassId
        // to check which teacher is assigned as the class teacher
        public async Task<Class?> GetClassByIdAsync(int classId)
        {
            return await _context.Classes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ClassId == classId);
        }



        // 11. Add a single attendance record to the table in memory
        public async Task AddAsync(Attendance attendance)
        {
            await _context.Attendances.AddAsync(attendance);
        }



        // 12. Add multiple attendance records at once in memory
        public async Task AddRangeAsync(IEnumerable<Attendance> attendances)
        {
            await _context.Attendances.AddRangeAsync(attendances);
        }



        // 13. Tell EF Core this attendance record has been modified
        public void Update(Attendance attendance)
        {
            _context.Attendances.Update(attendance);
        }



        // 14. Tell EF Core this attendance record should be deleted
        public void Delete(Attendance attendance)
        {
            _context.Attendances.Remove(attendance);
        }



        // Send all pending changes to MySQL
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }


    }
}