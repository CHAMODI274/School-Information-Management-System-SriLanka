using System.ComponentModel.DataAnnotations;
using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.DTOs.Student
{
    public class CreateStudentDto
    {
        // Fields required when admitting a new student

        [Required(ErrorMessage = "Admission number is required.")]
        public string AdmissionNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Full name is required.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; } = string.Empty;

        [Required] public Gender Gender { get; set; }
        [Required] public DateTime DateOfBirth { get; set; }
        [Required] public DateTime EnrollmentDate { get; set; }

        public string? Nationality { get; set; }
        public string? Religion { get; set; }
        public string? PreviousSchool { get; set; }
        public string? ClassEntered { get; set; }
        public string? Photo { get; set; }

        [Required(ErrorMessage = "Parent is required.")]
        public int ParentId { get; set; }
    }



    public class UpdateStudentDto
    {
        // Fields allowed to be changed after admission

        [Required(ErrorMessage = "Full name is required.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; } = string.Empty;

        [Required] public Gender Gender { get; set; }
        [Required] public DateTime DateOfBirth { get; set; }

        public string? Nationality { get; set; }
        public string? Religion { get; set; }
        public string? PreviousSchool { get; set; }
        public string? Photo { get; set; }

        [Required] public StudentStatus CurrentStatus { get; set; }

        public DateTime? PassOutDate { get; set; }

        [Required(ErrorMessage = "Parent is required.")]
        public int ParentId { get; set; }
    }



    public class StudentResponseDto
    {
        // Fields returned to the client when fetching student data
        public int StudentId { get; set; }
        public string AdmissionNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string? Nationality { get; set; }
        public string? Religion { get; set; }
        public string? PreviousSchool { get; set; }
        public string? ClassEntered { get; set; }
        public DateTime? PassOutDate { get; set; }
        public string CurrentStatus { get; set; } = string.Empty;
        public string? Photo { get; set; }
        public int ParentId { get; set; }
    }
}