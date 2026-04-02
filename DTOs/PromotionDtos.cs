using System.ComponentModel.DataAnnotations;
using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.DTOs.Promotion
{
    public class PromoteStudentDto // promotes a SINGLE student individually
    {
        [Required(ErrorMessage = "Current enrollment is required.")] public int CurrentEnrollmentId { get; set; }
        [Required(ErrorMessage = "New year is required.")] public int NewYearId { get; set; }
    }


    public class ClassPromoteDto // promotes ALL students in a one specific CLASS at once
    {
        [Required(ErrorMessage = "Current class is required.")] public int CurrentClassId { get; set; }
        [Required(ErrorMessage = "Current year is required.")] public int CurrentYearId { get; set; }
        [Required(ErrorMessage = "New class is required.")] public int NewClassId { get; set; }
        [Required(ErrorMessage = "New year is required.")] public int NewYearId { get; set; }
    }


    public class SchoolPromotionDto // promotes the WHOLE SCHOOL at once
    {
        [Required(ErrorMessage = "Current year is required.")] public int CurrentYearId { get; set; }
        [Required(ErrorMessage = "New year is required.")] public int NewYearId { get; set; }
    }





     public class PromotionResultDto // Returned after promoting a single student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string AdmissionNumber { get; set; } = string.Empty;

        // The old enrollment that was closed
        public int OldEnrollmentId { get; set; }
        public string OldGrade { get; set; } = string.Empty;
        public string OldSection { get; set; } = string.Empty;
        public string OldYear { get; set; } = string.Empty;
        public string OldStatus { get; set; } = string.Empty;

        // The new enrollment that was created 
        public int? NewEnrollmentId { get; set; } // // NewEnrollmentId will be null if the student was Graduated
        public string NewGrade { get; set; } = string.Empty;
        public string NewSection { get; set; } = string.Empty;
        public string NewYear { get; set; } = string.Empty;
    }



    // Returned after promoting an entire class
    public class BulkPromotionResultDto
    {
        public int TotalProcessed { get; set; } // Total number of students processed
        public int TotalSucceeded { get; set; } // Number of students successfully promoted
        public int TotalSkipped { get; set; } // Number of students skipped 

        public List<PromotionResultDto> Results { get; set; } = new(); 
    }



    // Returned after whole school promotion
    public class SchoolPromotionResultDto
    {
        public int TotalClassesProcessed  { get; set; }
        public int TotalStudentsProcessed { get; set; }
        public int TotalPromoted { get; set; }
        public int TotalGraduated { get; set; }
        public int TotalSkipped { get; set; }

        public List<ClassPromotionSummaryDto> ClassResults { get; set; } = new();
    }



    // Summary for one class during school wide promotion
    public class ClassPromotionSummaryDto
    {
        public string CurrentGrade { get; set; } = string.Empty;
        public string CurrentSection { get; set; } = string.Empty;
        public string NewGrade { get; set; } = string.Empty;
        public string NewSection { get; set; } = string.Empty;
        public int StudentsPromoted { get; set; }
        public int StudentsGraduated { get; set; }
        public int StudentsSkipped { get; set; }
    }
}