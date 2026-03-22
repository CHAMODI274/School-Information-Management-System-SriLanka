using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.Models
{
    public class Class
    {
        [Key]
        public int ClassId { get; set; }
        [Required] [MaxLength(20)] public string Grade { get; set; } = string.Empty;
        [MaxLength(20)] public string Section { get; set; } = string.Empty;
        public int MaxStudents { get; set; }
        [MaxLength(20)] public string? RoomNumber { get; set; }
        
        public MediumType Medium { get; set; } // Use enum

        public int TeacherId { get; set; } // The primary class teacher
        public Teacher Teacher { get; set; } = null!;

        public int YearId { get; set; }
        public AcademicYear AcademicYear { get; set; } = null!;

        
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<ClassCurriculum> ClassCurriculums { get; set; } = new List<ClassCurriculum>();
        public ICollection<ExaminationClass> ExaminationClasses { get; set; } = new List<ExaminationClass>();
    }
}