using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class ExaminationClass
    {
        [Key]
        public int ExamClassId { get; set; }
        public int ExamId { get; set; }
        public Examination Examination { get; set; } = null!;
        public int ClassId { get; set; }
        public Class Class { get; set; } = null!;
        [DataType(DataType.Date)]
        public DateTime ScheduledDate { get; set; }
        public TimeSpan ScheduledTime { get; set; }
    }
}