using SchoolManagementSystem.DTOs.Teacher;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;

        // Constructor injection 
        public TeacherService(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }



        // Get all teachers and convert each to a response DTO
        public async Task<IEnumerable<TeacherResponseDto>> GetAllAsync()
        {
            // Ask the repository for all raw Teacher model objects
            var teachers = await _teacherRepository.GetAllAsync();

            // Convert each Teacher model to a TeacherResponseDto
            return teachers.Select(t => MapToResponseDto(t));
        }



        // Get one teacher by ID and convert to a response DTO
        public async Task<TeacherResponseDto?> GetByIdAsync(int id)
        {
            var teacher = await _teacherRepository.GetByIdAsync(id);

            if (teacher == null) return null; // Return null if not found

            return MapToResponseDto(teacher);
        }



        // Create a new teacher record
        public async Task<TeacherResponseDto> CreateAsync(CreateTeacherDto dto)
        {
            // Business rule: each teacher must have a unique NIC number
            bool nicTaken = await _teacherRepository.NICExistsAsync(dto.NIC);
            if (nicTaken)
            {
                // The Controller will catch this and return 409 Conflict
                throw new InvalidOperationException(
                    $"A teacher with NIC '{dto.NIC}' already exists.");
            }

            // Business rule: each teacher must have a unique email address
            bool emailTaken = await _teacherRepository.EmailExistsAsync(dto.Email);
            if (emailTaken)
            {
                throw new InvalidOperationException(
                    $"A teacher with email '{dto.Email}' already exists.");
            }

            // Map the incoming DTO fields onto a new Teacher model object
            var teacher = new Teacher
            {
                Title = dto.Title,
                Name = dto.Name,
                NIC = dto.NIC,
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                Address = dto.Address,
                Phone = dto.Phone,
                Email = dto.Email,
                DateOfJoining = dto.DateOfJoining,
                EmploymentStatus = dto.EmploymentStatus,
                EmployeeType = dto.EmployeeType,
                Position = dto.Position,
                Service = dto.Service,
                ServiceClass = dto.ServiceClass,
                ServiceGrade = dto.ServiceGrade,
                Qualifications = dto.Qualifications // optional
            };

            // Ask the repository to add this teacher and save to the database
            await _teacherRepository.AddAsync(teacher);
            await _teacherRepository.SaveChangesAsync();

            return MapToResponseDto(teacher); // // Return the saved teacher as a DTO
        }



        // Update an existing teacher's details
        public async Task<TeacherResponseDto?> UpdateAsync(int id, UpdateTeacherDto dto)
        {
            // First fetch the existing teacher from the database
            var teacher = await _teacherRepository.GetByIdAsync(id);

            // Return null if the teacher doesn't exist
            if (teacher == null) return null;

            // Overwrite the existing fields with the new values from the DTO
            teacher.Title = dto.Title;
            teacher.Name = dto.Name;
            teacher.NIC = dto.NIC;
            teacher.Gender = dto.Gender;
            teacher.DateOfBirth = dto.DateOfBirth;
            teacher.Address = dto.Address;
            teacher.Phone = dto.Phone;
            teacher.Email = dto.Email;
            teacher.EmploymentStatus = dto.EmploymentStatus;
            teacher.EmployeeType = dto.EmployeeType;
            teacher.Position = dto.Position;
            teacher.Service = dto.Service;
            teacher.ServiceClass = dto.ServiceClass;
            teacher.ServiceGrade = dto.ServiceGrade;
            teacher.Qualifications = dto.Qualifications;

            // Tell the repository the teacher object has changed, then save
            _teacherRepository.Update(teacher);
            await _teacherRepository.SaveChangesAsync();

            return MapToResponseDto(teacher);
        }



        // Delete a teacher permanently
        public async Task<bool> DeleteAsync(int id)
        {
            var teacher = await _teacherRepository.GetByIdAsync(id);

            if (teacher == null) return false; // Nothing to delete if the teacher doesn't exist

            _teacherRepository.Delete(teacher);
            await _teacherRepository.SaveChangesAsync();

            return true;
        }


        // Converts a raw Teacher model into a TeacherResponseDto for the frontend
        private static TeacherResponseDto MapToResponseDto(Teacher t)
        {
            return new TeacherResponseDto
            {
                TeacherId = t.TeacherId,
                Title = t.Title.ToString(),
                Name = t.Name,
                NIC = t.NIC,
                Gender = t.Gender.ToString(),
                DateOfBirth = t.DateOfBirth,
                Address = t.Address,
                Phone = t.Phone,
                Email = t.Email,
                DateOfJoining = t.DateOfJoining,
                EmploymentStatus = t.EmploymentStatus.ToString(),
                EmployeeType = t.EmployeeType,
                Position = t.Position,
                Service = t.Service,
                ServiceClass = t.ServiceClass,
                ServiceGrade = t.ServiceGrade,
                Qualifications = t.Qualifications
            };
        }
    }
}