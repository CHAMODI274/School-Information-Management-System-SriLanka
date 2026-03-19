using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.Models
{
    public class ExamMark
    {
        [Key]
        public int MarkId { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal MarksObtained { get; set; }
        
        [MaxLength(10)] public GradeType Grade { get; set; } // Uses the new enum
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal Percentage { get; set; }
        [MaxLength(500)] public string? Remarks { get; set; }
        public DateTime EntryDate { get; set; }
        public bool ExamAbsent { get; set; }

        public int? TeacherId { get; set; } 
        public Teacher? Teacher { get; set; }

        public int ExamId { get; set; }
        public Examination Examination { get; set; } = null!;

        public int ClassCurriculumId { get; set; }
        public ClassCurriculum ClassCurriculum { get; set; } = null!;

        public int EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; } = null!;
    }
    
}