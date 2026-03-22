using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.Models
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; } 

        public Title Title { get; set; } // Title enum
        [Required] [MaxLength(150)] public string Name { get; set; } = string.Empty;
        [MaxLength(20)] public string NIC { get; set; } = string.Empty;

        public Gender Gender { get; set; } // Uses Gender enum
        public DateTime DateOfBirth { get; set; }

        [MaxLength(250)] public string Address { get; set; } = string.Empty;
        [MaxLength(20)] public string Phone { get; set; } = string.Empty;
        [MaxLength(100)] public string Email { get; set; } = string.Empty;
        public DateTime DateOfJoining { get; set; }
        
        public EmployeeStatus EmploymentStatus { get; set; }
        
        [MaxLength(50)] public string EmployeeType { get; set; } = string.Empty;
        [MaxLength(100)] public string Position { get; set; } = string.Empty;
        [MaxLength(100)] public string Service { get; set; } = string.Empty;
        [MaxLength(50)] public string ServiceClass { get; set; } = string.Empty;
        [MaxLength(50)] public string ServiceGrade { get; set; } = string.Empty;
        [MaxLength(500)] public string? Qualifications { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }


        public ICollection<Class> HostedClasses { get; set; } = new List<Class>();
        public ICollection<SubjectAllocation> SubjectAllocations { get; set; } = new List<SubjectAllocation>();
        public ICollection<Attendance> AttendancesMarked { get; set; } = new List<Attendance>();
        public ICollection<ExamMark> ExamMarksEntered { get; set; } = new List<ExamMark>();
    }
}