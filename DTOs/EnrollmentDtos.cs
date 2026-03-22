using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.DTOs.Enrollment
{
    public class CreateEnrollmentDto
    {
        // Fields required when enrolling a student into a class
        public DateTime EnrollmentDate { get; set; }                 
        public int StudentId { get; set; }                           
        public int ClassId { get; set; }                 
        public int YearId { get; set; }                  
    }


    public class UpdateEnrollmentDto
    {
        // Fields allowed to be updated on an enrollment record
        public EnrollmentStatus Status { get; set; }                 
        public int ClassId { get; set; }                             
    }


    public class EnrollmentResponseDto
    {
        // Fields returned to the client when fetching enrollment data
        public int EnrollmentId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Status { get; set; } = string.Empty;       
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;     
        public string AdmissionNumber { get; set; } = string.Empty; 
        public int ClassId { get; set; }
        public string Grade { get; set; } = string.Empty;          
        public string Section { get; set; } = string.Empty;         
        public int YearId { get; set; }
        public string Year { get; set; } = string.Empty;           
    }
}