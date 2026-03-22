using System.ComponentModel.DataAnnotations;
using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.DTOs.ManagementStaff
{
    public class CreateManagementStaffDto
    {
        // Fields required when creating a new management staff record
        [Required] public Title Title { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "NIC is required.")]
        public string NIC { get; set; } = string.Empty;

        [Required] public Gender Gender { get; set; }
        [Required] public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone is required.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required] public DateTime DateOfJoining { get; set; }
        [Required] public EmployeeStatus EmploymentStatus { get; set; } 

        [Required(ErrorMessage = "Employee type is required.")]      
        public string EmployeeType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Position is required.")]
        public string Position { get; set; } = string.Empty;
    }


    public class UpdateManagementStaffDto
    {
        // Fields allowed to be updated on a management staff record
        [Required] public Title Title { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "NIC is required.")]
        public string NIC { get; set; } = string.Empty;

        [Required] public Gender Gender { get; set; }
        [Required] public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone is required.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required] public EmployeeStatus EmploymentStatus { get; set; }

        [Required(ErrorMessage = "Employee type is required.")]
        public string EmployeeType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Position is required.")]
        public string Position { get; set; } = string.Empty;        
    }


    public class ManagementStaffResponseDto
    {
        // Fields returned to the client when fetching management staff data
        public int EmployeeId { get; set; }
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
    }
}