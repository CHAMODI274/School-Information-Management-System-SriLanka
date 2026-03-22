using System.ComponentModel.DataAnnotations;
using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.DTOs.Teacher
{
    public class CreateTeacherDto
    {
        // Fields required when creating a new teacher record
        public Title Title { get; set; } 

        [Required(ErrorMessage = "Name is required.")]                           
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "NIC is required.")]
        public string NIC { get; set; } = string.Empty;

        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone is required.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        public DateTime DateOfJoining { get; set; }
        public EmployeeStatus EmploymentStatus { get; set; }
        
        [Required(ErrorMessage = "Employee type is required.")]
        public string EmployeeType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Position is required.")]
        public string Position { get; set; } = string.Empty;

        [Required(ErrorMessage = "Service is required.")]
        public string Service { get; set; } = string.Empty;

        [Required(ErrorMessage = "Service class is required.")]
        public string ServiceClass { get; set; } = string.Empty;

        [Required(ErrorMessage = "Service grade is required.")]
        public string ServiceGrade { get; set; } = string.Empty;

        public string? Qualifications { get; set; } //optional filed
    }



    public class UpdateTeacherDto
    {
        // Fields allowed to be updated on a teacher record
        public Title Title { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "NIC is required.")]
        public string NIC { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone is required.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        public EmployeeStatus EmploymentStatus { get; set; }

        [Required(ErrorMessage = "Employee type is required.")]
        public string EmployeeType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Position is required.")]
        public string Position { get; set; } = string.Empty;

        [Required(ErrorMessage = "Service is required.")]
        public string Service { get; set; } = string.Empty;

        [Required(ErrorMessage = "Service class is required.")]
        public string ServiceClass { get; set; } = string.Empty;

        [Required(ErrorMessage = "Service grade is required.")]
        public string ServiceGrade { get; set; } = string.Empty;

        public string? Qualifications { get; set; } //optional
    }


    public class TeacherResponseDto
    {
        // Fields returned to the client when fetching teacher data
        public int TeacherId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string NIC { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfJoining { get; set; }
        public string EmploymentStatus { get; set; } = string.Empty; 
        public string EmployeeType { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Service { get; set; } = string.Empty;
        public string ServiceClass { get; set; } = string.Empty;
        public string ServiceGrade { get; set; } = string.Empty;
        public string? Qualifications { get; set; }
    }


}