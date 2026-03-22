using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.DTOs.Parent
{
    public class CreateParentDto
    {
        // Fields required when creating a new parent record
        public Title Title { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NIC { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Occupation { get; set; }
        public string? WorkPlace { get; set; }
        public string? WorkPhone { get; set; }
        public string? EmergencyContact { get; set; }
    }


    public class UpdateParentDto
    {
        // Fields allowed to be updated on a parent record
        public Title Title { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NIC { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Occupation { get; set; }
        public string? WorkPlace { get; set; }
        public string? WorkPhone { get; set; }
        public string? EmergencyContact { get; set; }
    }


    public class ParentResponseDto
    {
        // Fields returned to the client when fetching parent data
        public int ParentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string NIC { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Occupation { get; set; }
        public string? WorkPlace { get; set; }
        public string? WorkPhone { get; set; }
        public string? EmergencyContact { get; set; }
    }
}