using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class ClassCurriculum
    {
        [Key]
        public int ClassCurriculumId { get; set; }

        public int ClassId { get; set; }
        public Class Class { get; set; } = null!;

        public int SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;

        public int YearId { get; set; }
        public AcademicYear AcademicYear { get; set; } = null!;


        public ICollection<SubjectAllocation> SubjectAllocations { get; set; } = new List<SubjectAllocation>();
        public ICollection<ExamMark> ExamMarks { get; set; } = new List<ExamMark>();
    }
}