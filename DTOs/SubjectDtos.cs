namespace SchoolManagementSystem.DTOs.Subject
{
    public class CreateSubjectDto
    {
        // Fields required when creating a new subject
        public string SubjectName { get; set; } = string.Empty;
        public string SubjectCode { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Credits { get; set; }
        public bool IsCompulsory { get; set; }
    }


    public class UpdateSubjectDto
    {
        // Fields allowed to be updated on a subject record
        public string SubjectName { get; set; } = string.Empty;
        public string SubjectCode { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Credits { get; set; }
        public bool IsCompulsory { get; set; }
        public bool IsActive { get; set; }     
    }


    public class SubjectResponseDto
    {
        // Fields returned to the client when fetching subject data
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string SubjectCode { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Credits { get; set; }
        public bool IsCompulsory { get; set; }
        public bool IsActive { get; set; }  
    }
}