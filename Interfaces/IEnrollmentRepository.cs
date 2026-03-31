using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Interfaces
{
    public interface IEnrollmentRepository
    {
        // Get all enrollment records from the database
        Task<IEnumerable<Enrollment>> GetAllAsync();

        // Get all enrollments for a specific class
        Task<IEnumerable<Enrollment>> GetByClassAsync(int classId);

        // Get all enrollments for a specific student
        Task<IEnumerable<Enrollment>> GetByStudentAsync(int studentId);

        // Get one enrollment by its ID, or returns null if not found
        Task<Enrollment?> GetByIdAsync(int id);

        // Check if a student is already enrolled in any class for a specific year
        // A student can only be enrolled ONCE per academic year
        Task<bool> EnrollmentExistsAsync(int studentId, int yearId);

        // Add a new enrollment record to the database
        Task AddAsync(Enrollment enrollment);

        // Mark an enrollment record as modified in memory
        void Update(Enrollment enrollment);

        // Mark an enrollment record for deletion in memory
        void Delete(Enrollment enrollment);

        // Commit all pending changes to the database
        Task SaveChangesAsync();
    }
}