using SchoolManagementSystem.DTOs.Enrollment;

namespace SchoolManagementSystem.Interfaces
{
    public interface IEnrollmentService
    {
        // Get all enrollments across all students and classes
        Task<IEnumerable<EnrollmentResponseDto>> GetAllAsync();

        // Get all enrollments for a specific class
        Task<IEnumerable<EnrollmentResponseDto>> GetByClassAsync(int classId);

        // Get all enrollments for a specific student
        Task<IEnumerable<EnrollmentResponseDto>> GetByStudentAsync(int studentId);

        // Get one enrollment by ID, or returns null if not found
        Task<EnrollmentResponseDto?> GetByIdAsync(int id);

        // Create a new enrollment linking a Student to a Class for a Year
        // Throws an exception if the student is already enrolled for that year
        Task<EnrollmentResponseDto> CreateAsync(CreateEnrollmentDto dto);

        // Update an enrollment's status or move to a different class
        // Returns null if not found
        Task<EnrollmentResponseDto?> UpdateAsync(int id, UpdateEnrollmentDto dto);

        // Delete an enrollment, returns true if deleted, false if not found
        Task<bool> DeleteAsync(int id);
    }
}