using System.ComponentModel.DataAnnotations;
using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.DTOs.Attendance
{
    public class CreateAttendanceDto
    {
        // Fields required when marking attendance for a student
        public DateTime Date { get; set; }
        public AttendanceStatus Status { get; set; }

        [Required(ErrorMessage = "Teacher is required.")]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "Enrollment is required.")]
        public int EnrollmentId { get; set; }
    }


    public class UpdateAttendanceDto
    {
        public AttendanceStatus Status { get; set; } // Only Status can be updated
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



    public class AttendanceSummaryDto
    {
        // Returned when fetching an attendance summary for a student
        public int EnrollmentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string AdmissionNumber { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public int TotalDaysPresent { get; set; }
        public int TotalDaysAbsent { get; set; }
        public int TotalDays { get; set; }
        public decimal AttendancePercentage { get; set; } // attendance precentage -> calculate by the service
    }



    
}