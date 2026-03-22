using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.DTOs.NonAcademicStaff
{
    public class CreateNonAcademicStaffDto
    {
        // Fields required when creating a new non-academic staff record
        public Title Title { get; set; }                             
        public string Name { get; set; } = string.Empty;
        public string NIC { get; set; } = string.Empty;             
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfJoining { get; set; }
        public EmployeeStatus EmploymentStatus { get; set; }        
        public string EmployeeType { get; set; } = string.Empty;    
        public string Position { get; set; } = string.Empty;       
    }


    public class UpdateNonAcademicStaffDto
    {
        // Fields allowed to be updated on a non-academic staff record
        public Title Title { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NIC { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public EmployeeStatus EmploymentStatus { get; set; }        
        public string EmployeeType { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;

    }


    public class NonAcademicStaffResponseDto
    {
        // Fields returned to the client when fetching non-academic staff data
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