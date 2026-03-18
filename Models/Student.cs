using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        [MaxLength(30)]
        public string AdmissionNumber { get; set; } = string.Empty;

        [Required] [MaxLength(150)] public string FullName { get; set; } = string.Empty;
        [Required] [MaxLength(50)] public string FirstName { get; set; } = string.Empty;
        [MaxLength(50)] public string LastName { get; set; } = string.Empty;
        
        public Gender Gender { get; set; } // Uses Gender enum

        public DateTime DateOfBirth { get; set; }
        public DateTime EnrollmentDate { get; set; }
        
        [MaxLength(50)] public string? Nationality { get; set; }
        [MaxLength(50)] public string? Religion { get; set; }
        [MaxLength(150)] public string? PreviousSchool { get; set; }
        [MaxLength(20)] public string? ClassEntered { get; set; }

        public DateTime? PassOutDate { get; set; }
        
        public StudentStatus CurrentStatus { get; set; }
        
        [MaxLength(500)] public string? Photo { get; set; }

        // Foreign Keys
        public int? UserId { get; set; }
        public User? User { get; set; }

        public int ParentId { get; set; }
        public Parent Parent { get; set; } = null!;
        
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}