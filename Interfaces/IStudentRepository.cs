using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Interfaces
{
    // Defines WHAT database operations are available for Students.
    public interface IStudentRepository
    {
        // Get all student records from the database
        Task<IEnumerable<Student>> GetAllAsync();

        // Get one student by their ID, returns null if not found
        Task<Student?> GetByIdAsync(int id);

        // Get one student by their admission number — returns null if not found
        Task<Student?> GetByAdmissionNumberAsync(string admissionNumber);

        // Check if an admission number already exists in the database & Returns true if taken, false if available
        // to prevent duplicate admission numbers
        Task<bool> AdmissionNumberExistsAsync(string admissionNumber);

        // Add a new student record to the database
        Task AddAsync(Student student);

        // Mark a student record as modified in memory
        void Update(Student student);

        // Mark a student record for deletion in memory
        void Delete(Student student);

        // Commit all pending changes to the database
        Task SaveChangesAsync();

    }
}