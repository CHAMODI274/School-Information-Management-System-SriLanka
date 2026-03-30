using SchoolManagementSystem.DTOs.AcademicYear;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Services
{
    // contains all BUSINESS LOGIC for academic year operations
    // BUSINESS RULE: only ONE academic year can be active at a time

    public class AcademicYearService : IAcademicYearService
    {
        private readonly IAcademicYearRepository _academicYearRepository;

        public AcademicYearService(IAcademicYearRepository academicYearRepository)
        {
            _academicYearRepository = academicYearRepository;
        }



        // Get all academic years and convert each to a response DTO
        public async Task<IEnumerable<AcademicYearResponseDto>> GetAllAsync()
        {
            // Ask the repository for all raw AcademicYear model objects
            var years = await _academicYearRepository.GetAllAsync();

            // Convert each AcademicYear model to an AcademicYearResponseDto
            return years.Select(a => MapToResponseDto(a));
        }



        // Get one academic year by ID and convert to a response DTO
        public async Task<AcademicYearResponseDto?> GetByIdAsync(int id)
        {
            var year = await _academicYearRepository.GetByIdAsync(id);

            if (year == null) return null; // Return null if not found

            return MapToResponseDto(year);
        }



        // Get the currently active academic year
        public async Task<AcademicYearResponseDto?> GetActiveAsync()
        {
            var year = await _academicYearRepository.GetActiveAsync();

            if (year == null) return null; // Return null if no year is currently active

            return MapToResponseDto(year);
        }



        // Create a new academic year record
        public async Task<AcademicYearResponseDto> CreateAsync(CreateAcademicYearDto dto)
        {
            // Business rule: year labels must be unique e.g. cannot have two "2025" years
            bool yearExists = await _academicYearRepository.YearExistsAsync(dto.Year);
            if (yearExists)
            {
                // The Controller will catch this
                throw new InvalidOperationException(
                    $"An academic year '{dto.Year}' already exists.");
            }

            // Map the incoming DTO fields onto a new AcademicYear model object
            var academicYear = new AcademicYear
            {
                Year      = dto.Year,
                StartDate = dto.StartDate,
                EndDate   = dto.EndDate,
                IsActive  = false // Business rule: a newly created year is NOT active by default
            };

            // Ask the repository to add this year and save to the database
            await _academicYearRepository.AddAsync(academicYear);
            await _academicYearRepository.SaveChangesAsync();

            return MapToResponseDto(academicYear); // Return the saved year as a DTO
        }



        // Update an existing academic year's details
        public async Task<AcademicYearResponseDto?> UpdateAsync(int id, UpdateAcademicYearDto dto)
        {
            // 1st fetch the existing year from the database
            var academicYear = await _academicYearRepository.GetByIdAsync(id);

            // Return null if the year doesn't exist
            if (academicYear == null) return null;


            /* Businness Rule: When the admin sets a year as active, 
               the service needs to find all other currently actived years and
               decativate them. This ensure only one year is ever active at a time. */
            if (dto.IsActive)
            {
                // Get all years that are currently marked as active
                var currentlyActiveYears = await _academicYearRepository.GetAllActiveAsync();

                // Deactivate each one (except the one about to activate)
                foreach (var activeYear in currentlyActiveYears)
                {
                    // Skip the year we are updating
                    if (activeYear.YearId == id) continue;

                    // Deactivate this year
                    activeYear.IsActive = false;
                    _academicYearRepository.Update(activeYear);
                }
            }
            // Now update the year we actually want to change
            academicYear.Year = dto.Year;
            academicYear.StartDate = dto.StartDate;
            academicYear.EndDate = dto.EndDate;
            academicYear.IsActive = dto.IsActive;

            _academicYearRepository.Update(academicYear);

            //Save -> Both the deactivations and the update are committed together
            await _academicYearRepository.SaveChangesAsync();

            return MapToResponseDto(academicYear);
        }



        // Delete an academic year
        public async Task<bool> DeleteAsync(int id)
        {
            var academicYear = await _academicYearRepository.GetByIdAsync(id);

            // Nothing to delete if the year doesn't exist
            if (academicYear == null) return false;

            _academicYearRepository.Delete(academicYear);
            await _academicYearRepository.SaveChangesAsync();

            return true;
        }



        // Converts a raw AcademicYear model into an AcademicYearResponseDto for the frontend
        private static AcademicYearResponseDto MapToResponseDto(AcademicYear a)
        {
            return new AcademicYearResponseDto
            {
                YearId = a.YearId,
                Year = a.Year,
                StartDate = a.StartDate,
                EndDate = a.EndDate,
                IsActive = a.IsActive
            };
            
        }


    }
}