using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }
        public DateTime Date { get; set; }
        
        [MaxLength(10)] public AttendanceStatus Status { get; set; } // Use enum

        public int? TeacherId { get; set; } 
        public Teacher? Teacher { get; set; }

        public int EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; } = null!;
    }
}