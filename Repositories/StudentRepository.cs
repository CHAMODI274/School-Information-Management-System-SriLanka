using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Repositories
{
    // Talks to the database for Student operations
    public class StudentRepository : IStudentRepository
    {
        //SchoolDbContext gives access to all database tables
        private readonly SchoolDbContext _context;

        // Constructor injection
        public StudentRepository(SchoolDbContext context)
        {
            _context = context;
        }


        // Fetch all students from the Students table
        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students
                .AsNoTracking() //only reading
                .ToListAsync();
        }


        // Fetch a single student by their ID
        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.StudentId == id);
        }


        // Fetch a single student by their admission number
        public async Task<Student?> GetByAdmissionNumberAsync(string admissionNumber)
        {
            return await _context.Students
                .AsNoTracking() // searching only, used for display
                .FirstOrDefaultAsync(s => s.AdmissionNumber == admissionNumber);
        }


        // Check whether an admission number already exists in the database
        public async Task<bool> AdmissionNumberExistsAsync(string admissionNumber)
        {
            return await _context.Students
                .AnyAsync(s => s.AdmissionNumber == admissionNumber);
                // AnyAsync() stops as soon as it finds one match. It doesn't load the full student record
        }


        // Add a new student to the Students table in memory
        public async Task AddAsync(Student student)
        {
            await _context.Students.AddAsync(student);
        }


        // Tell EF Core this student object has been modified
        public void Update(Student student)
        {
            _context.Students.Update(student);
        }


        // Tell EF Core this student should be deleted
        public void Delete(Student student)
        {
            _context.Students.Remove(student);
        }


        // Send all pending changes (inserts, updates, and deletes) to MySQL
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}