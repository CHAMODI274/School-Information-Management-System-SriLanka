using SchoolManagementSystem.DTOs.Student;

namespace SchoolManagementSystem.Interfaces
{
    public interface IStudentService
    {
        // Get all students
        Task<IEnumerable<StudentResponseDto>> GetAllAsync();

        // Get one student by ID, returns null if not found
        Task<StudentResponseDto?> GetByIdAsync(int id);

        // Get one student by admission number, returns null if not found
        Task<StudentResponseDto?> GetByAdmissionNumberAsync(string admissionNumber);

        // Create a new student record
        // Throws an exception if the admission number is already taken
        Task<StudentResponseDto> CreateAsync(CreateStudentDto dto);

        // Update an existing student ,returns null if not found
        Task<StudentResponseDto?> UpdateAsync(int id, UpdateStudentDto dto);

        // Delete a student, returns true if deleted, false if not found
        Task<bool> DeleteAsync(int id);
    }
}