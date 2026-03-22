namespace SchoolManagementSystem.DTOs.ExamMarks
{
    public class CreateExamMarkDto
    {
        // Fields required when entering marks for a student
        public decimal MarksObtained { get; set; }
        public bool ExamAbsent { get; set; }
        public string? Remarks { get; set; }   // Optional: teacher remarks
        public int TeacherId { get; set; }
        public int ExamId { get; set; }
        public int ClassCurriculumId { get; set; }   // Subject the marks belong to
        public int EnrollmentId { get; set; }     
    }


    public class UpdateExamMarkDto
    {
        // Fields allowed to be updated on an exam mark record
        public decimal MarksObtained { get; set; }
        public bool ExamAbsent { get; set; }
        public string? Remarks { get; set; }
    }


    public class ExamMarkResponseDto
    {
        public int MarkId { get; set; }
        public decimal MarksObtained { get; set; }
        public string Grade { get; set; } = string.Empty;
        public decimal Percentage { get; set; }
        public string? Remarks { get; set; }
        public DateTime EntryDate { get; set; }
        public bool ExamAbsent { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public int ExamId { get; set; }
        public string ExamName { get; set; } = string.Empty;
        public int EnrollmentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string AdmissionNumber { get; set; } = string.Empty;
    }
}