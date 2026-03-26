using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Interfaces
{
    public interface IParentRepository
    {
        Task<IEnumerable<Parent>> GetAllAsync(); // Get all parent records from the database

        Task<Parent?> GetByIdAsync(int id); // Get one parent by their ID, returns null if not found

        Task AddAsync(Parent parent); // Add a new parent record to the database
        
        void Update(Parent parent); // Mark a parent record as modified in memory

        void Delete(Parent parent); // Mark a parent record for deletion in memory

        Task SaveChangesAsync(); // Commit all pending changes to the database

    }
}