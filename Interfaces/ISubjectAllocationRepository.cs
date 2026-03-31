using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Interfaces
{
    public interface ISubjectAllocationRepository
    {
        // Get all allocation records from the database
        Task<IEnumerable<SubjectAllocation>> GetAllAsync();

        // Get all allocations assigned to a specific teacher
        Task<IEnumerable<SubjectAllocation>> GetByTeacherAsync(int teacherId);

        // Get all allocations for a specific curriculum entry
        Task<IEnumerable<SubjectAllocation>> GetByCurriculumAsync(int classCurriculumId);

        // Get one allocation by its ID, or returns null if not found
        Task<SubjectAllocation?> GetByIdAsync(int id);

        // Check if a teacher is already allocated to a specific curriculum entry
        // The same teacher cannot be allocated to the same curriculum entry twice
        Task<bool> AllocationExistsAsync(int classCurriculumId, int teacherId);

        // Add a new allocation record to the database
        Task AddAsync(SubjectAllocation subjectAllocation);

        // Mark an allocation record as modified in memory
        void Update(SubjectAllocation subjectAllocation);

        // Mark an allocation record for deletion in memory
        void Delete(SubjectAllocation subjectAllocation);

        // Commit all pending changes to the database
        Task SaveChangesAsync();
    }
}