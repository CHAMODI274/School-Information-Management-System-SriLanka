using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.Models
{
    public class Parent
    {
        [Key]
        public int ParentId { get; set; }

        public Title Title { get; set; } // Title enum
        [Required] [MaxLength(150)] public string Name { get; set; } = string.Empty;
        [MaxLength(20)] public string NIC { get; set; } = string.Empty;
        [MaxLength(20)] public string Phone { get; set; } = string.Empty;
        [MaxLength(250)] public string Address { get; set; } = string.Empty;
        [MaxLength(100)] public string? Occupation { get; set; }
        [MaxLength(150)] public string? WorkPlace { get; set; }
        [MaxLength(20)] public string? WorkPhone { get; set; }
        [MaxLength(20)] public string? EmergencyContact { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}