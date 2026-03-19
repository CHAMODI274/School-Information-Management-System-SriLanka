using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        
        [MaxLength(20)] public EnrollmentStatus Status { get; set; } // Uses enum

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public int ClassId { get; set; }
        public Class Class { get; set; } = null!;

        public int YearId { get; set; }
        public AcademicYear AcademicYear { get; set; } = null!;

        
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public ICollection<ExamMark> ExamMarks { get; set; } = new List<ExamMark>();
    }
}