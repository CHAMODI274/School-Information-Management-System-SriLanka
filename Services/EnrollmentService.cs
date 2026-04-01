using SchoolManagementSystem.DTOs.Enrollment;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public EnrollmentService(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }



        // Get all enrollments and convert each to a response DTO
        public async Task<IEnumerable<EnrollmentResponseDto>> GetAllAsync()
        {
            var enrollments = await _enrollmentRepository.GetAllAsync();
            return enrollments.Select(e => MapToResponseDto(e));
        }




        // Get all enrollments for a specific class
        public async Task<IEnumerable<EnrollmentResponseDto>> GetByClassAsync(int classId)
        {
            var enrollments = await _enrollmentRepository.GetByClassAsync(classId);
            return enrollments.Select(e => MapToResponseDto(e));
        }



        // Get all enrollments for a specific student
        public async Task<IEnumerable<EnrollmentResponseDto>> GetByStudentAsync(int studentId)
        {
            var enrollments = await _enrollmentRepository.GetByStudentAsync(studentId);
            return enrollments.Select(e => MapToResponseDto(e));
        }



        // Get one enrollment by ID and convert to a response DTO
        public async Task<EnrollmentResponseDto?> GetByIdAsync(int id)
        {
            var enrollment = await _enrollmentRepository.GetByIdAsync(id);

            if (enrollment == null) return null; // Return null if not found

            return MapToResponseDto(enrollment);
        }




        // Create a brand new enrollment record
        public async Task<EnrollmentResponseDto> CreateAsync(CreateEnrollmentDto dto)
        {
            // Business rule: a student can only be enrolled in ONE class per academic year
            bool alreadyEnrolled = await _enrollmentRepository
                .EnrollmentExistsAsync(dto.StudentId, dto.YearId);

                if (alreadyEnrolled)
            {
                throw new InvalidOperationException(
                    "This student is already enrolled in a class for the selected academic year.");
            }

            // Map the incoming DTO fields onto a new Enrollment model object
            var enrollment = new Enrollment
            {
                EnrollmentDate = dto.EnrollmentDate,
                StudentId = dto.StudentId,
                ClassId = dto.ClassId,
                YearId = dto.YearId,
                Status = EnrollmentStatus.Active // every new enrollment starts with Active status
            };

            // Ask the repository to add this enrollment and save to the database
            await _enrollmentRepository.AddAsync(enrollment);
            await _enrollmentRepository.SaveChangesAsync();

            // Reload the enrollment with Student, Class and AcademicYear included
            var savedEnrollment = await _enrollmentRepository
                .GetByIdAsync(enrollment.EnrollmentId);

            return MapToResponseDto(savedEnrollment!);

        }




        // Update an existing enrollment
        public async Task<EnrollmentResponseDto?> UpdateAsync(int id, UpdateEnrollmentDto dto)
        {
            // First fetch the existing enrollment from the database
            var enrollment = await _enrollmentRepository.GetByIdAsync(id);

            if (enrollment == null) return null; // Return null if the enrollment doesn't exist

            enrollment.Status = dto.Status; // Status can be updated
            enrollment.ClassId = dto.ClassId; // // ClassId can be updated

            // Tell the repository the enrollment has changed, then save
            _enrollmentRepository.Update(enrollment);
            await _enrollmentRepository.SaveChangesAsync();

            return MapToResponseDto(enrollment);
        }



        // Delete an enrollment
        public async Task<bool> DeleteAsync(int id)
        {
            var enrollment = await _enrollmentRepository.GetByIdAsync(id);

            if (enrollment == null) return false;

            _enrollmentRepository.Delete(enrollment);
            await _enrollmentRepository.SaveChangesAsync();

            return true;
        }




        // Converts a raw Enrollment model into an EnrollmentResponseDto for the frontend
        private static EnrollmentResponseDto MapToResponseDto(Enrollment e)
        {
            return new EnrollmentResponseDto
            {
                EnrollmentId = e.EnrollmentId,
                EnrollmentDate = e.EnrollmentDate,
                Status = e.Status.ToString(),

                // From included Student
                StudentId = e.StudentId,
                StudentName = e.Student?.FullName ?? string.Empty,
                AdmissionNumber = e.Student?.AdmissionNumber ?? string.Empty,

                // From included Class
                ClassId = e.ClassId,
                Grade = e.Class?.Grade ?? string.Empty,
                Section = e.Class?.Section ?? string.Empty,

                // From included AcademicYear
                YearId = e.YearId,
                Year = e.AcademicYear?.Year ?? string.Empty
            };
        }
        
    }
}