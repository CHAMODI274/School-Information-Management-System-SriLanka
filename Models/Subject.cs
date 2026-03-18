using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.Models
{
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }
        [Required] [MaxLength(100)] public string SubjectName { get; set; } = string.Empty;
        [Required] [MaxLength(20)] public string SubjectCode { get; set; } = string.Empty;
        [MaxLength(500)] public string? Description { get; set; }
        public int Credits { get; set; }
        public bool IsCompulsory { get; set; }
        public bool IsActive { get; set; }


        public ICollection<ClassCurriculum> ClassCurriculums { get; set; } = new List<ClassCurriculum>();
    }
}