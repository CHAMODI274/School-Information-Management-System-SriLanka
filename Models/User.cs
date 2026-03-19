using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [MaxLength(20)] public UserRole Role { get; set; }
        [MaxLength(10)] public UserStatus Status { get; set; }
        public DateTime CreateDate { get; set; }


        // Navigation Properties (0..1 relationships)
        public Student? Student { get; set; }
        public Parent? Parent { get; set; }
        public Teacher? Teacher { get; set; }
        public ManagementStaff? ManagementStaff { get; set; }
        public NonAcademicStaff? NonAcademicStaff { get; set; }
    }
}