using SchoolManagementSystem.DTOs.Teacher;

namespace SchoolManagementSystem.Interfaces
{
    public interface ITeacherService
    {
        // Get all teachers
        Task<IEnumerable<TeacherResponseDto>> GetAllAsync();

        // Get one teacher by ID, returns null if not found
        Task<TeacherResponseDto?> GetByIdAsync(int id);

        // Create a new teacher record
        // Throws an exception if the NIC or email is already taken
        Task<TeacherResponseDto> CreateAsync(CreateTeacherDto dto);

        // Update an existing teacher, returns null if not found
        Task<TeacherResponseDto?> UpdateAsync(int id, UpdateTeacherDto dto);

        // Delete a teacher, returns true if deleted, false if not found
        Task<bool> DeleteAsync(int id);
    }
}