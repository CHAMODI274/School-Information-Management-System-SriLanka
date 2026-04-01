using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Interfaces
{
    public interface IAttendanceRepository
    {
        // Get all attendance records for a specific enrollment
        Task<IEnumerable<Attendance>> GetByEnrollmentAsync(int enrollmentId);

        // Get all attendance records for a specific class on a specific date
        Task<IEnumerable<Attendance>> GetByClassAndDateAsync(int classId, DateTime date);

        // Get all attendance records for a specific class within a month and year
        Task<IEnumerable<Attendance>> GetByClassAndMonthAsync(
            int classId, int month, int year);

        // Get all attendance records across the whole school on a specific date
        Task<IEnumerable<Attendance>> GetBySchoolAndDateAsync(DateTime date);

        // Get one attendance record by its ID, or returns null if not found
        Task<Attendance?> GetByIdAsync(int id);

        // Check if an attendance record already exists for a specific enrollment on a date
        // only can have one attendance record per student per day 
        Task<bool> AttendanceExistsAsync(int enrollmentId, DateTime date);

        // Get present and absent counts for a specific enrollment
        Task<(int present, int absent)> GetAttendanceSummaryAsync(int enrollmentId);

        // Get a Teacher record by their linked UserId
        // Returns null if no teacher is linked to this user account
        Task<Teacher?> GetTeacherByUserIdAsync(int userId);

        // Get the ClassId for a specific enrollment
        // Returns null if the enrollment doesn't exist
        Task<int?> GetClassIdByEnrollmentAsync(int enrollmentId);

        // Get a Class record by its ID
        Task<Class?> GetClassByIdAsync(int classId);

        // Add a single attendance record to the database
        Task AddAsync(Attendance attendance);

        // Add multiple attendance records at once -> marks attendance for a whole class in one go
        Task AddRangeAsync(IEnumerable<Attendance> attendances);

        // Mark an attendance record as modified in memory
        void Update(Attendance attendance);

        // Mark an attendance record for deletion in memory
        void Delete(Attendance attendance);

        // Commit all pending changes to the database
        Task SaveChangesAsync();
    }
}