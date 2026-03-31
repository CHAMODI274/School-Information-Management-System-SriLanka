using SchoolManagementSystem.DTOs.Subject;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;

        public SubjectService(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }



        // Get all subjects (active and inactive) and convert each to a response DTO
        public async Task<IEnumerable<SubjectResponseDto>> GetAllAsync()
        {
            var subjects = await _subjectRepository.GetAllAsync();

            return subjects.Select(s => MapToResponseDto(s));
        }



        // Get only active subjects and convert each to a response DTO
        public async Task<IEnumerable<SubjectResponseDto>> GetAllActiveAsync()
        {
            var subjects = await _subjectRepository.GetAllActiveAsync();

            return subjects.Select(s => MapToResponseDto(s));
        }



        // Get one subject by ID and convert to a response DTO
        public async Task<SubjectResponseDto?> GetByIdAsync(int id)
        {
            var subject = await _subjectRepository.GetByIdAsync(id);

            if (subject == null) return null; // Return null if not found

            return MapToResponseDto(subject);
        }



        // Create a brand new subject record
        public async Task<SubjectResponseDto> CreateAsync(CreateSubjectDto dto)
        {
            // Business rule: subject codes must be unique across the school
            bool codeExists = await _subjectRepository
                .SubjectCodeExistsAsync(dto.SubjectCode);

            if (codeExists)
            {
                throw new InvalidOperationException(
                    $"A subject with code '{dto.SubjectCode}' already exists.");
            }

            // Map the incoming DTO fields onto a new Subject model object
            var subject = new Subject
            {
                SubjectName = dto.SubjectName,
                SubjectCode = dto.SubjectCode,
                Description = dto.Description, // optional field
                Credits = dto.Credits,
                IsCompulsory = dto.IsCompulsory,
                IsActive = true // Business rule: every new subject starts as active
            };

            await _subjectRepository.AddAsync(subject);
            await _subjectRepository.SaveChangesAsync();
            
            return MapToResponseDto(subject);

        }



        // Update an existing subject's details
        public async Task<SubjectResponseDto?> UpdateAsync(int id, UpdateSubjectDto dto)
        {
            // First fetch the existing subject from the database
            var subject = await _subjectRepository.GetByIdAsync(id);

            if (subject == null) return null; // Return null if the subject doesn't exist

            // Overwrite the existing fields with the new values from the DTO
            subject.SubjectName  = dto.SubjectName;
            subject.SubjectCode  = dto.SubjectCode;
            subject.Description  = dto.Description;
            subject.Credits      = dto.Credits;
            subject.IsCompulsory = dto.IsCompulsory;
            subject.IsActive     = dto.IsActive;

            // Tell the repository the subject object has changed, then save
            _subjectRepository.Update(subject);
            await _subjectRepository.SaveChangesAsync();

            return MapToResponseDto(subject);
        }



        // Delete a subject permanently
        // Deleting removes all historical references to this subject
        public async Task<bool> DeleteAsync(int id)
        {
            var subject = await _subjectRepository.GetByIdAsync(id);

            // Nothing to delete if the subject doesn't exist
            if (subject == null) return false;

            _subjectRepository.Delete(subject);
            await _subjectRepository.SaveChangesAsync();

            return true;
        }



        // Converts a raw Subject model into a SubjectResponseDto for the frontend
        private static SubjectResponseDto MapToResponseDto(Subject s)
        {
            return new SubjectResponseDto
            {
                SubjectId = s.SubjectId,
                SubjectName = s.SubjectName,
                SubjectCode = s.SubjectCode,
                Description = s.Description,
                Credits = s.Credits,
                IsCompulsory = s.IsCompulsory,
                IsActive = s.IsActive
            };
        }
    }
}