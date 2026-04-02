using System.ComponentModel.DataAnnotations;
using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.DTOs.Promotion
{
    public class PromoteStudentDto // promotes a SINGLE student
    {
        [Required(ErrorMessage = "Current enrollment is required.")] public int CurrentEnrollmentId { get; set; }
        [Required(ErrorMessage = "Outcome status is required.")] public EnrollmentStatus OutcomeStatus { get; set; }
        public int? NewClassId { get; set; }
        public int? NewYearId { get; set; }
    }


    public class BulkPromoteDto // promotes ALL students in a class at once
    {
        [Required(ErrorMessage = "Current class is required.")] public int CurrentClassId { get; set; }
        [Required(ErrorMessage = "Current year is required.")] public int CurrentYearId { get; set; }
        [Required(ErrorMessage = "New class is required.")] public int NewClassId { get; set; }
        [Required(ErrorMessage = "New year is required.")] public int NewYearId { get; set; }
        public List<int> ExcludeStudentIds { get; set; } = new();
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

        // The new enrollment that was created (if OutcomeStatus was Graduated strings will be empty -> graduate students don't get a new enrollment)
        public int? NewEnrollmentId { get; set; }
        public string NewGrade { get; set; } = string.Empty;
        public string NewSection { get; set; } = string.Empty;
        public string NewYear { get; set; } = string.Empty;
    }



    // returned after a bulk promotion
    public class BulkPromotionResultDto
    {
        public int TotalProcessed { get; set; } // Total number of students processed
        public int TotalSucceeded { get; set; } // Number of students successfully promoted
        public int TotalSkipped { get; set; } // Number of students skipped 
        public List<PromotionResultDto> Results { get; set; } = new(); // The detailed result for each student (students who already promoted, or excluded from the bulk operation)
    }
}