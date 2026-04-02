using SchoolManagementSystem.DTOs.ExaminationClass;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Services
{
    public class ExaminationClassService : IExaminationClassService
    {
        private readonly IExaminationClassRepository _examinationClassRepository;

        public ExaminationClassService(IExaminationClassRepository examinationClassRepository)
        {
            _examinationClassRepository = examinationClassRepository;
        }



        // Get all examination class records and convert each to a response DTO
        public async Task<IEnumerable<ExaminationClassResponseDto>> GetAllAsync()
        {
            var records = await _examinationClassRepository.GetAllAsync();
            return records.Select(ec => MapToResponseDto(ec));
        }



        // Get all classes scheduled for a specific examination
        public async Task<IEnumerable<ExaminationClassResponseDto>> GetByExamAsync(int examId)
        {
            var records = await _examinationClassRepository.GetByExamAsync(examId);
            return records.Select(ec => MapToResponseDto(ec));
        }



        // Get all exams scheduled for a specific class
        public async Task<IEnumerable<ExaminationClassResponseDto>> GetByClassAsync(int classId)
        {
            var records = await _examinationClassRepository.GetByClassAsync(classId);
            return records.Select(ec => MapToResponseDto(ec));
        }



        // Get one examination class record by ID and convert to a response DTO
        public async Task<ExaminationClassResponseDto?> GetByIdAsync(int id)
        {
            var record = await _examinationClassRepository.GetByIdAsync(id);
            if (record == null) return null; // return null if not found
            return MapToResponseDto(record);
        }



        // Create a new examination class record
        public async Task<ExaminationClassResponseDto> CreateAsync(CreateExaminationClassDto dto)
        {
            // Business rule: the same exam cannot be scheduled for the same class twice
            bool recordExists = await _examinationClassRepository
                .ExamClassExistsAsync(dto.ExamId, dto.ClassId);

                if (recordExists)
            {
                throw new InvalidOperationException(
                    "This examination is already scheduled for the selected class.");
            }

            // Map the incoming DTO fields onto a new ExaminationClass model object
            var examinationClass = new ExaminationClass
            {
                ExamId = dto.ExamId,
                ClassId = dto.ClassId,
                ScheduledDate = dto.ScheduledDate,
                ScheduledTime = dto.ScheduledTime
            };

            // Ask the repository to add this record and save to the database
            await _examinationClassRepository.AddAsync(examinationClass);
            await _examinationClassRepository.SaveChangesAsync();

            // Reload the record with Examination and Class included
            var savedRecord = await _examinationClassRepository
                .GetByIdAsync(examinationClass.ExamClassId);

            return MapToResponseDto(savedRecord!);
        }



        // Update an existing examination class record
        public async Task<ExaminationClassResponseDto?> UpdateAsync(int id, UpdateExaminationClassDto dto)
        {
            // First fetch the existing record from the database
            var record = await _examinationClassRepository.GetByIdAsync(id);

            if (record == null) return null; // return null if the record doesn't exist

            // only schedulr date and time can be change
            record.ScheduledDate = dto.ScheduledDate;
            record.ScheduledTime = dto.ScheduledTime;

            // Tell the repository the record has changed, then save
            _examinationClassRepository.Update(record);
            await _examinationClassRepository.SaveChangesAsync();

            return MapToResponseDto(record);
        }



        // Delete an examination class record
        public async Task<bool> DeleteAsync(int id)
        {
            var record = await _examinationClassRepository.GetByIdAsync(id);

            if (record == null) return false;

            _examinationClassRepository.Delete(record);
            await _examinationClassRepository.SaveChangesAsync();

            return true; // successfully deleted
        }




        // Converts a raw ExaminationClass model into an ExaminationClassResponseDto for the frontend
        private static ExaminationClassResponseDto MapToResponseDto(ExaminationClass ec)
        {
            return new ExaminationClassResponseDto
            {
                ExamClassId   = ec.ExamClassId,

                // From included Examination
                ExamId = ec.ExamId,
                ExamName = ec.Examination?.ExamName ?? string.Empty,

                // From included Class
                ClassId = ec.ClassId,
                Grade = ec.Class?.Grade ?? string.Empty,
                Section = ec.Class?.Section ?? string.Empty,

                ScheduledDate = ec.ScheduledDate,
                ScheduledTime = ec.ScheduledTime
            };
        }
    }
}