namespace SchoolManagementSystem.DTOs.Examination
{
    public class CreateExaminationClassDto
    {
        // Fields required when scheduling an exam for a class
        public int ExamId { get; set; }
        public int ClassId { get; set; }  // Class the exam is scheduled for
        public DateTime ScheduledDate { get; set; }
        public TimeSpan ScheduledTime { get; set; }
    }


    public class UpdateExaminationClassDto
    {
        // Fields allowed to be updated on an examination class record
        public DateTime ScheduledDate { get; set; }
        public TimeSpan ScheduledTime { get; set; }
    }


    public class ExaminationClassResponseDto
    {
        //Fields returned to the client when fetching examination class data
        public int ExamClassId { get; set; }
        public int ExamId { get; set; }
        public string ExamName { get; set; } = string.Empty;
        public int ClassId { get; set; }
        public string Grade { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public TimeSpan ScheduledTime { get; set; }
    }
}