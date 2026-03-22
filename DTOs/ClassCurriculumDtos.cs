namespace SchoolManagementSystem.DTOs.ClassCurriculum
{
    public class CreateClassCurriculumDto
    {
        // Fields required when adding a subject to a class for a specific year
        public int ClassId { get; set; }  // Class the subject is assigned to
        public int SubjectId { get; set; }  // Subject being assigned
        public int YearId { get; set; } 
    }


    public class UpdateClassCurriculumDto
    {
        // Fields allowed to be updated on a class curriculum record
        public int SubjectId { get; set; }
    }


    public class ClassCurriculumResponseDto
    {
        // Fields returned to the client when fetching class curriculum data
        public int ClassCurriculumId { get; set; }
        public int ClassId { get; set; }
        public string Grade { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string SubjectCode { get; set; } = string.Empty;
        public int YearId { get; set; }
        public string Year { get; set; } = string.Empty;
    }
}