using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Repositories
{
    public class ParentRepository : IParentRepository
    {
        private readonly SchoolDbContext _context;

        public ParentRepository(SchoolDbContext context)
        {
            _context = context;
        }


        // Fetch all parents from the Parents table
        public async Task<IEnumerable<Parent>> GetAllAsync()
        {
            return await _context.Parents
                .AsNoTracking() // only reading
                .ToListAsync();
        }


        // Fetch a single parent by their ID
        public async Task<Parent?> GetByIdAsync(int id)
        {
            return await _context.Parents
                .FirstOrDefaultAsync(p => p.ParentId == id);
        }


        // Add a new parent to the Parents table in memory
        public async Task AddAsync(Parent parent)
        {
            await _context.Parents.AddAsync(parent);
        }


        // Tell EF Core this parent object has been modified
        public void Update(Parent parent)
        {
            _context.Parents.Update(parent);
        }


        // Tell EF Core this parent should be deleted
        public void Delete(Parent parent)
        {
            _context.Parents.Remove(parent);
        }


        // Send all pending changes (inserts, updates, and deletes) to MySQL
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}