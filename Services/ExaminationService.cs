using SchoolManagementSystem.DTOs.Examination;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Services
{
    public class ExaminationService : IExaminationService
    {
        private readonly IExaminationRepository _examinationRepository ;

        public ExaminationService(IExaminationRepository examinationRepository)
        {
            _examinationRepository = examinationRepository;
        }



        // Get all examinations and convert each to a response DTO
        public async Task<IEnumerable<ExaminationResponseDto>> GetAllAsync()
        {
            var examinations = await _examinationRepository.GetAllAsync();
            return examinations.Select(e => MapToResponseDto(e));
        }



        // Get all examinations for a specific academic year
        public async Task<IEnumerable<ExaminationResponseDto>> GetByYearAsync(int yearId)
        {
            var examinations = await _examinationRepository.GetByYearAsync(yearId);
            return examinations.Select(e => MapToResponseDto(e));
        }



        // Get one examination by ID and convert to a response DTO
        public async Task<ExaminationResponseDto?> GetByIdAsync(int id)
        {
            var examination = await _examinationRepository.GetByIdAsync(id);
            if (examination == null) return null; // return null if not found
            return MapToResponseDto(examination);
        }



        // Create a new examination record
        public async Task<ExaminationResponseDto> CreateAsync(CreateExaminationDto dto)
        {
            // Business rule: exam names must be unique within the same academic year
            bool examExists = await _examinationRepository
                .ExamExistsAsync(dto.ExamName, dto.YearId);

            if (examExists)
            {
                throw new InvalidOperationException(
                    $"An examination named '{dto.ExamName}' already exists " +
                    $"for the selected academic year.");
            }

            var examination = new Examination
            {
                ExamName = dto.ExamName,
                ExamType = dto.ExamType,
                Term = dto.Term,
                MaxMark = dto.MaxMark,
                Description = dto.Description,
                YearId = dto.YearId
            };

            await _examinationRepository.AddAsync(examination);
            await _examinationRepository.SaveChangesAsync();

            // Reload the examination with AcademicYear included
            var savedExamination = await _examinationRepository
                .GetByIdAsync(examination.ExamId);

            return MapToResponseDto(savedExamination!);
        }



        // Update an existing examination's details
        public async Task<ExaminationResponseDto?> UpdateAsync(int id, UpdateExaminationDto dto)
        {
            // First fetch the existing examination from the database
            var examination = await _examinationRepository.GetByIdAsync(id);

            // Return null if the examination doesn't exist
            if (examination == null) return null;

            // Overwrite the existing fields with the new values from the DTO
            examination.ExamName = dto.ExamName;
            examination.ExamType = dto.ExamType;
            examination.Term = dto.Term;
            examination.MaxMark = dto.MaxMark;
            examination.Description = dto.Description;

            // Tell the repository the examination has changed, then save
            _examinationRepository.Update(examination);
            await _examinationRepository.SaveChangesAsync();

            return MapToResponseDto(examination);
        }



        // Delete an examination permanently
        public async Task<bool> DeleteAsync(int id)
        {
            var examination = await _examinationRepository.GetByIdAsync(id);

            if (examination == null) return false;

            _examinationRepository.Delete(examination);
            await _examinationRepository.SaveChangesAsync();

            return true;
        }




        private static ExaminationResponseDto MapToResponseDto(Examination e)
        {
            return new ExaminationResponseDto
            {
                ExamId = e.ExamId,
                ExamName = e.ExamName,
                ExamType = e.ExamType.ToString(),
                Term = e.Term,
                MaxMark = e.MaxMark,
                Description = e.Description,

                // From included AcademicYear
                YearId = e.YearId,
                Year = e.AcademicYear?.Year ?? string.Empty
            };
        }
    }
}