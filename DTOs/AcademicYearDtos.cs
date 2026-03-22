namespace SchoolManagementSystem.DTOs.AcademicYear
{
    public class CreateAcademicYearDto
    {
        //Fields required when creating a new academic year
        public string Year { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }   // Start date of the academic year
        public DateTime EndDate { get; set; }   
    }


    public class UpdateAcademicYearDto
    {
        // Fields allowed to be updated on an academic year record
        public string Year { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }


    public class AcademicYearResponseDto
    {
        // Fields returned to the client when fetching academic year data
        public int YearId { get; set; }
        public string Year { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } 
    }
}