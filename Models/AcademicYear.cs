using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class AcademicYear
    {
        [Key]
        public int YearId { get; set; }
        [Required] [MaxLength(10)] public string Year { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }


        public ICollection<Class> Classes { get; set; } = new List<Class>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<ClassCurriculum> ClassCurriculums { get; set; } = new List<ClassCurriculum>();
        public ICollection<Examination> Examinations { get; set; } = new List<Examination>();
    }
}