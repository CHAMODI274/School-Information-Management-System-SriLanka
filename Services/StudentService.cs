using SchoolManagementSystem.DTOs.Student;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.Services
{
    // StudentService contains all BUSINESS LOGIC for student operations.
    // calls the repository for data access and returns DTOs to the Controller.
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository; //depend on IStudentRepository

        // Constructor injection
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        // Get all students and convert each to a response DTO
         public async Task<IEnumerable<StudentResponseDto>> GetAllAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            return students.Select(s => MapToResponseDto(s));
        }


        // Get one student by ID and convert to a response DTO
        public async Task<StudentResponseDto?> GetByIdAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null) return null; // Return null if not found
            return MapToResponseDto(student);
        }


        // Get one student by admission number and convert to a response DTO
        public async Task<StudentResponseDto?> GetByAdmissionNumberAsync(string admissionNumber)
        {
            var student = await _studentRepository.GetByAdmissionNumberAsync(admissionNumber);
            if (student == null) return null;
            return MapToResponseDto(student);
        }


        // Create a new student record
        public async Task<StudentResponseDto> CreateAsync(CreateStudentDto dto)
        {
            // Business rule: admission numbers must be unique across the school
            bool admissionNumberTaken = await _studentRepository
                .AdmissionNumberExistsAsync(dto.AdmissionNumber); // check admission number already exist or not

                if (admissionNumberTaken)
            {   // if admission number is taken throw an exception
                throw new InvalidOperationException(
                    $"Admission number '{dto.AdmissionNumber}' is already in use.");
            }

            // Map the incoming DTO fields onto a new Student model object
            var student = new Student
            {
                AdmissionNumber = dto.AdmissionNumber,
                FullName = dto.FullName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                EnrollmentDate = dto.EnrollmentDate,
                Nationality = dto.Nationality,  // optional
                Religion = dto.Religion,  // optional
                PreviousSchool = dto.PreviousSchool,  // optional
                ClassEntered = dto.ClassEntered,  // optional
                Photo = dto.Photo,  // optional
                ParentId = dto.ParentId,
                CurrentStatus = StudentStatus.Active // Business rule: every new student starts with Active status
            };

            // Ask the repository to add this student and save to the database
            await _studentRepository.AddAsync(student); 
            await _studentRepository.SaveChangesAsync();

            return MapToResponseDto(student); // Return the saved student as a DTO
        }


        // Update an existing student's details
        public async Task<StudentResponseDto?> UpdateAsync(int id, UpdateStudentDto dto)
        {
            // First fetch the existing student from the database
            var student = await _studentRepository.GetByIdAsync(id);

            // Return null if the student doesn't exist
            if (student == null) return null;

            // Overwrite the existing fields with the new values from the DTO
            student.FullName = dto.FullName;
            student.FirstName = dto.FirstName;
            student.LastName = dto.LastName;
            student.Gender = dto.Gender;
            student.DateOfBirth = dto.DateOfBirth;
            student.Nationality = dto.Nationality;
            student.Religion = dto.Religion;
            student.PreviousSchool = dto.PreviousSchool;
            student.Photo = dto.Photo;
            student.CurrentStatus = dto.CurrentStatus;
            student.PassOutDate = dto.PassOutDate;
            student.ParentId = dto.ParentId;

            // Tell the repository the student object has changed, then save
            _studentRepository.Update(student);
            await _studentRepository.SaveChangesAsync();

            return MapToResponseDto(student);
        }



        // Delete a student
        public async Task<bool> DeleteAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);

            if (student == null) return false; // Nothing to delete if the student doesn't exist

            _studentRepository.Delete(student);
            await _studentRepository.SaveChangesAsync();

            return true;
        }



        // Converts a raw Student model into a StudentResponseDto for the frontend
        private static StudentResponseDto MapToResponseDto(Student s)
        {
            return new StudentResponseDto
            {
                StudentId = s.StudentId,
                AdmissionNumber = s.AdmissionNumber,
                FullName = s.FullName,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Gender = s.Gender.ToString(), // enum to string
                DateOfBirth = s.DateOfBirth,
                EnrollmentDate = s.EnrollmentDate,
                Nationality = s.Nationality,
                Religion = s.Religion,
                PreviousSchool = s.PreviousSchool,
                ClassEntered = s.ClassEntered,
                PassOutDate = s.PassOutDate,
                CurrentStatus = s.CurrentStatus.ToString(), // enum to string
                Photo = s.Photo,
                ParentId = s.ParentId
            };
        }
    }
}