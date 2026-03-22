using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.DTOs.Class
{
    public class CreateClassDto
    {
        // Fields required when creating a new class
        public string Grade { get; set; } = string.Empty; // e.g. "Grade 10", "Grade 11"
        public string Section { get; set; } = string.Empty;  // e.g. "A", "B", "C"
        public int MaxStudents { get; set; }                         
        public string? RoomNumber { get; set; }  // Optional: classroom number
        public MediumType Medium { get; set; }  // Medium enum: English, Sinhala, Tamil
        public int TeacherId { get; set; }  // Class teacher assigned to this class
        public int YearId { get; set; }  // Academic year this class belongs to
    }


    public class UpdateClassDto
    {
        // Fields allowed to be updated on a class record
        public string Grade { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        public int MaxStudents { get; set; }
        public string? RoomNumber { get; set; }
        public MediumType Medium { get; set; }
        public int TeacherId { get; set; } // Class teacher can be reassigned
    }


    public class ClassResponseDto
    {
        // Fields returned to the client when fetching class data
        public int ClassId { get; set; }
        public string Grade { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        public int MaxStudents { get; set; }
        public string? RoomNumber { get; set; }
        public string Medium { get; set; } = string.Empty;          
        public int TeacherId { get; set; }                           // ID of the assigned class teacher
        public string TeacherName { get; set; } = string.Empty;     // Name of the assigned class teacher
        public int YearId { get; set; }                              // Academic year ID
        public string Year { get; set; } = string.Empty;            // Academic year e.g. "2026"
    }
}