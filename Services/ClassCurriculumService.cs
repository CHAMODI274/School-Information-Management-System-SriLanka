using SchoolManagementSystem.DTOs.ClassCurriculum;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Services
{
    public class ClassCurriculumService : IClassCurriculumService
    {
        private readonly IClassCurriculumRepository _classCurriculumRepository;

        public ClassCurriculumService(IClassCurriculumRepository classCurriculumRepository)
        {
            _classCurriculumRepository = classCurriculumRepository;
        }



        // Get all curriculum entries and convert each to a response DTO
        public async Task<IEnumerable<ClassCurriculumResponseDto>> GetAllAsync()
        {           
            var entries = await _classCurriculumRepository.GetAllAsync();
            return entries.Select(cc => MapToResponseDto(cc));
        }



        // Get all curriculum entries for a specific class
        public async Task<IEnumerable<ClassCurriculumResponseDto>> GetByClassAsync(int classId)
        {
            var entries = await _classCurriculumRepository.GetByClassAsync(classId);
            return entries.Select(cc => MapToResponseDto(cc));
        }



        // Get all curriculum entries for a specific academic year
        public async Task<IEnumerable<ClassCurriculumResponseDto>> GetByYearAsync(int yearId)
        {
            var entries = await _classCurriculumRepository.GetByYearAsync(yearId);
            return entries.Select(cc => MapToResponseDto(cc));
        }



        // Get one curriculum entry by ID and convert to a response DTO
        public async Task<ClassCurriculumResponseDto?> GetByIdAsync(int id)
        {
            var entry = await _classCurriculumRepository.GetByIdAsync(id);

            if (entry == null) return null; // Return null if not found

            return MapToResponseDto(entry);
        }



        // Create a brand new curriculum entry
        public async Task<ClassCurriculumResponseDto> CreateAsync(CreateClassCurriculumDto dto)
        {
            // Business rule: the same subject cannot be assigned to the same class more than once in the same academic year
            bool entryExists = await _classCurriculumRepository
                .EntryExistsAsync(dto.ClassId, dto.SubjectId, dto.YearId);

            if (entryExists)
            {
                throw new InvalidOperationException(
                    "This subject is already assigned to this class for the selected year.");
            }

            // Map the incoming DTO fields onto a new ClassCurriculum model object
            var classCurriculum = new ClassCurriculum 
            {
                ClassId = dto.ClassId,
                SubjectId = dto.SubjectId,
                YearId = dto.YearId
            };

            // Ask the repository to add this entry and save to the database
            await _classCurriculumRepository.AddAsync(classCurriculum);
            await _classCurriculumRepository.SaveChangesAsync();

            // Reload the entry with Class, Subject, and AcademicYear included
            var savedEntry = await _classCurriculumRepository
                .GetByIdAsync(classCurriculum.ClassCurriculumId);

                return MapToResponseDto(savedEntry!);
        }



        // Update an existing curriculum entry
         public async Task<ClassCurriculumResponseDto?> UpdateAsync(int id, UpdateClassCurriculumDto dto)
        {
            // First fetch the existing entry from the database
            var entry = await _classCurriculumRepository.GetByIdAsync(id);

            // Return null if the entry doesn't exist
            if (entry == null) return null;

            // Only the SubjectId is updatable on a curriculum entry
            // ClassId and YearId are fixed
            entry.SubjectId = dto.SubjectId;

            // Tell the repository the entry has changed, then save
            _classCurriculumRepository.Update(entry);
            await _classCurriculumRepository.SaveChangesAsync();

            return MapToResponseDto(entry);
        }



        // Delete a curriculum entry permanently
        public async Task<bool> DeleteAsync(int id)
        {
            var entry = await _classCurriculumRepository.GetByIdAsync(id);

            if (entry == null) return false;

            _classCurriculumRepository.Delete(entry);
            await _classCurriculumRepository.SaveChangesAsync();

            return true;
        }




        // Converts a raw ClassCurriculum model into a ClassCurriculumResponseDto for the frontend
        private static ClassCurriculumResponseDto MapToResponseDto(ClassCurriculum cc)
        {
            return new ClassCurriculumResponseDto
            {
                ClassCurriculumId = cc.ClassCurriculumId,
                ClassId = cc.ClassId,
                Grade = cc.Class?.Grade ?? string.Empty,  // from included Class
                Section = cc.Class?.Section ?? string.Empty,  // from included Class
                SubjectId = cc.SubjectId,
                SubjectName = cc.Subject?.SubjectName ?? string.Empty, // from included Subject
                SubjectCode = cc.Subject?.SubjectCode ?? string.Empty, // from included Subject
                YearId = cc.YearId,
                Year = cc.AcademicYear?.Year ?? string.Empty  // from included AcademicYear
            };
        }
    }
}