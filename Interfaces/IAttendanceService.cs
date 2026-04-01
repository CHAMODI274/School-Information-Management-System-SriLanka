using SchoolManagementSystem.DTOs.Attendance;

namespace SchoolManagementSystem.Interfaces
{
    public interface IAttendanceService
    {
        // Get all attendance records for a specific enrollment
        Task<IEnumerable<AttendanceResponseDto>> GetByEnrollmentAsync(int enrollmentId);

        // Get attendance records for a whole class on a specific date
        Task<IEnumerable<AttendanceResponseDto>> GetByClassAndDateAsync(int classId, DateTime date);

        // Get all attendance records for a class for a specific month        
        Task<IEnumerable<AttendanceResponseDto>> GetByClassAndMonthAsync(int classId, int month, int year);

        // Get all attendance records across the whole school for a specific date
        Task<IEnumerable<AttendanceResponseDto>> GetBySchoolAndDateAsync(DateTime date);

        // Get one attendance record by ID, or returns null if not found
        Task<AttendanceResponseDto?> GetByIdAsync(int id);

        // Get an attendance summary for a specific enrollment
        Task<AttendanceSummaryDto> GetAttendanceSummaryAsync(int enrollmentId);

        // Create a single attendance record for one student
        Task<AttendanceResponseDto> CreateAsync(
            CreateAttendanceDto dto, int? loggedInUserId);

        // mark attendance for multiple students at once        
        Task<IEnumerable<AttendanceResponseDto>> BulkCreateAsync(
            IEnumerable<CreateAttendanceDto> dtos, int? loggedInUserId);

        // Update an attendance record
        Task<AttendanceResponseDto?> UpdateAsync(
            int id, UpdateAttendanceDto dto, int? loggedInUserId);

        // Delete an attendance record        
        Task<bool> DeleteAsync(int id, int? loggedInUserId);
    }
}