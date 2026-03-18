using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.Models
{
    public class NonAcademicStaff
    {
        [Key]
        public int EmployeeId { get; set; }

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
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}