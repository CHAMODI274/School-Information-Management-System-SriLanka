using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class SubjectAllocation
    {
        [Key]
        public int AllocationId { get; set; }

        public int ClassCurriculumId { get; set; }
        public ClassCurriculum ClassCurriculum { get; set; } = null!;

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null!; 
    }
}