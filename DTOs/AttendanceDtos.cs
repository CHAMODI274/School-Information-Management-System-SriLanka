using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.DTOs.Attendance
{
    public class CreateAttendanceDto
    {
        // Fields required when marking attendance for a student
        public DateTime Date { get; set; }                           
        public AttendanceStatus Status { get; set; }                 
        public int TeacherId { get; set; }                           
        public int EnrollmentId { get; set; }                        
    }


    public class UpdateAttendanceDto
    {
        // Fields allowed to be updated on an attendance record
        public AttendanceStatus Status { get; set; }              
    }


    public class AttendanceResponseDto
    {
        // Fields returned to the client when fetching attendance data
        public int AttendanceId { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;          
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;     
        public int EnrollmentId { get; set; }
        public string StudentName { get; set; } = string.Empty;     
        public string AdmissionNumber { get; set; } = string.Empty; 
    }
}