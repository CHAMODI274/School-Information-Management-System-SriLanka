using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.Models
{
    public class Examination
    {
        [Key]
        public int ExamId { get; set; }
        [Required] [MaxLength(150)] public string ExamName { get; set; } = string.Empty;
        
        [MaxLength(20)] public ExamType ExamType { get; set; } // Use enum
        
        [MaxLength(50)] public string Term { get; set; } = string.Empty;
        [Column(TypeName = "decimal(5,2)")]
        public decimal MaxMark { get; set; }
        [MaxLength(500)] public string? Description { get; set; }

        public int YearId { get; set; }
        public AcademicYear AcademicYear { get; set; } = null!;

        
        public ICollection<ExaminationClass> ExaminationClasses { get; set; } = new List<ExaminationClass>();
        public ICollection<ExamMark> ExamMarks { get; set; } = new List<ExamMark>();
    }
}