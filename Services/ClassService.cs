using SchoolManagementSystem.DTOs.Class;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;

        public ClassService(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }



        // Get all classes and convert each to a response DTO
        public async Task<IEnumerable<ClassResponseDto>> GetAllAsync()
        {
            
            var classes = await _classRepository.GetAllAsync();

            return classes.Select(c => MapToResponseDto(c));
        }



         // Get all classes for a specific academic year
        public async Task<IEnumerable<ClassResponseDto>> GetByYearAsync(int yearId)
        {
            var classes = await _classRepository.GetByYearAsync(yearId);

            return classes.Select(c => MapToResponseDto(c));
        }



        // Get one class by ID and convert to a response DTO
        public async Task<ClassResponseDto?> GetByIdAsync(int id)
        {
            var cls = await _classRepository.GetByIdAsync(id);

            if (cls == null) return null;  // Return null if not found

            return MapToResponseDto(cls);
        }



        // Create a brand new class record
        public async Task<ClassResponseDto> CreateAsync(CreateClassDto dto)
        {
            // Business rule: the same Grade + Section combination cannot exist more than once in the same academic year
            bool classExists = await _classRepository
                .ClassExistsAsync(dto.Grade, dto.Section, dto.YearId);

            if (classExists)
            {
                throw new InvalidOperationException(
                    $"A class '{dto.Grade} - {dto.Section}' already exists " +
                    $"for the selected academic year.");
            }

            // Map the incoming DTO fields onto a new Class model object
            var cls = new Class
            {
                Grade = dto.Grade,
                Section = dto.Section,
                MaxStudents = dto.MaxStudents,
                RoomNumber = dto.RoomNumber,  // optional field
                Medium = dto.Medium,
                TeacherId = dto.TeacherId, // class teacher
                YearId = dto.YearId
            };

            // Ask the repository to add this class and save to the database
            await _classRepository.AddAsync(cls);
            await _classRepository.SaveChangesAsync();

            // Reload the class with Teacher and AcademicYear included
            var savedClass = await _classRepository.GetByIdAsync(cls.ClassId);

            return MapToResponseDto(savedClass!);
        }



        // Update an existing class's details
        public async Task<ClassResponseDto?> UpdateAsync(int id, UpdateClassDto dto)
        {
            // First fetch the existing class from the database
            var cls = await _classRepository.GetByIdAsync(id);

            // Return null if the class doesn't exist
            if (cls == null) return null;

            // Overwrite the existing fields with the new values from the DTO
            cls.Grade = dto.Grade;
            cls.Section = dto.Section;
            cls.MaxStudents = dto.MaxStudents;
            cls.RoomNumber = dto.RoomNumber;
            cls.Medium = dto.Medium;
            cls.TeacherId = dto.TeacherId;  // class teacher can be reassigned

            // Tell the repository the class object has changed, then save
            _classRepository.Update(cls);
            await _classRepository.SaveChangesAsync();

            return MapToResponseDto(cls);
        }



        // Delete a class permanently
        public async Task<bool> DeleteAsync(int id)
        {
            var cls = await _classRepository.GetByIdAsync(id);

            if (cls == null) return false;

            _classRepository.Delete(cls);
            await _classRepository.SaveChangesAsync();

            return true;
        }



        // Converts a raw Class model into a ClassResponseDto for the frontend
        // Note: Teacher and AcademicYear must be loaded (via Include) before calling this
        private static ClassResponseDto MapToResponseDto(Class c)
        {
            return new ClassResponseDto
            {
                ClassId = c.ClassId,
                Grade = c.Grade,
                Section = c.Section,
                MaxStudents = c.MaxStudents,
                RoomNumber = c.RoomNumber,
                Medium = c.Medium.ToString(),
                TeacherId = c.TeacherId,
                TeacherName = c.Teacher?.Name ?? string.Empty, // from included Teacher
                YearId = c.YearId,
                Year = c.AcademicYear?.Year ?? string.Empty // from included AcademicYear
                
            };
        }



    }
}