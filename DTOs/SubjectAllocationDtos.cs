using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.DTOs.SubjectAllocation
{
    public class CreateSubjectAllocationDto
    {
        // Fields required when assigning a teacher to a subject in a class

        [Required(ErrorMessage = "Class curriculum is required.")]
        public int ClassCurriculumId { get; set; }   // The class-subject combination

        [Required(ErrorMessage = "Teacher is required.")]
        public int TeacherId { get; set; }   // Teacher being assigned
    }


    public class UpdateSubjectAllocationDto
    {
        // Fields allowed to be updated on a subject allocation record

        [Required(ErrorMessage = "Teacher is required.")]
        public int TeacherId { get; set; } 
    }


    public class SubjectAllocationResponseDto
    {
        // Fields returned to the client when fetching subject allocation data
        public int AllocationId { get; set; }
        public int ClassCurriculumId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string SubjectCode { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
    }
}