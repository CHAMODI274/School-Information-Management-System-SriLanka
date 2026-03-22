using System.ComponentModel.DataAnnotations;
using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.DTOs.Examination
{
    public class CreateExaminationDto
    {
       // Fields required when creating a new examination 

       [Required(ErrorMessage = "Exam name is required.")]
       public string ExamName { get; set; } = string.Empty;
       
       public ExamType ExamType { get; set; }
       [Required] public string Term { get; set; } = string.Empty;
       public decimal MaxMark { get; set; }

       public string? Description { get; set; } // optional

       [Required(ErrorMessage = "Academic year is required.")]
       public int YearId { get; set; }
    }


    public class UpdateExaminationDto
    {
        // Fields allowed to be updated on an examination record

        [Required(ErrorMessage = "Exam name is required.")]
        public string ExamName { get; set; } = string.Empty;
        
        public ExamType ExamType { get; set; }

        [Required(ErrorMessage = "Term is required.")]
        public string Term { get; set; } = string.Empty;

        public decimal MaxMark { get; set; }

        public string? Description { get; set; }
    }


    public class ExaminationResponseDto
    {
        // Fields returned to the client when fetching examination data
        public int ExamId { get; set; }
        public string ExamName { get; set; } = string.Empty;
        public string ExamType { get; set; } = string.Empty;
        public string Term { get; set; } = string.Empty;
        public decimal MaxMark { get; set; }
        public string? Description { get; set; }
        public int YearId { get; set; }
        public string Year { get; set; } = string.Empty; 
    }


}