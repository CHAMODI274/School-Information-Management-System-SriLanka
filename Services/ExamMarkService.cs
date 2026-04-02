using SchoolManagementSystem.DTOs.ExamMarks;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.Services
{
    public class ExamMarkService : IExamMarkService
    {
        private readonly IExamMarkRepository _examMarkRepository;

        public ExamMarkService(IExamMarkRepository examMarkRepository)
        {
            _examMarkRepository = examMarkRepository;
        }



        // 1. Get all marks for a specific exam and class
        public async Task<IEnumerable<ExamMarkResponseDto>> GetByExamAndClassAsync(int examId, int classId)
        {
            var marks = await _examMarkRepository.GetByExamAndClassAsync(examId, classId);
            return marks.Select(em => MapToResponseDto(em));
        }



        // 2. Get all marks for a specific enrollment (student's full results)
        public async Task<IEnumerable<ExamMarkResponseDto>> GetByEnrollmentAsync(int enrollmentId)
        {
            var marks = await _examMarkRepository.GetByEnrollmentAsync(enrollmentId);
            return marks.Select(em => MapToResponseDto(em));
        }



        // 3. Get all marks for a specific examination across all classes
        public async Task<IEnumerable<ExamMarkResponseDto>> GetByExamAsync(int examId)
        {
            var marks = await _examMarkRepository.GetByExamAsync(examId);
            return marks.Select(em => MapToResponseDto(em));
        }



        // 4. Get one exam mark by ID and convert to a response DTO
        public async Task<ExamMarkResponseDto?> GetByIdAsync(int id)
        {
            var mark = await _examMarkRepository.GetByIdAsync(id);
            if (mark == null) return null; // return null if not found
            return MapToResponseDto(mark);
        }



        // 5. Create a brand new exam mark record
        public async Task<ExamMarkResponseDto> CreateAsync(CreateExamMarkDto dto, int? loggedInUserId)
        {
            // checking subject teacher
            if (loggedInUserId.HasValue)
            {
                await ValidateTeacherIsSubjectTeacherAsync(
                    dto.ClassCurriculumId, loggedInUserId.Value);
            }

            // Business rule: one mark per student per subject per exam
            bool markExists = await _examMarkRepository
                .MarkExistsAsync(dto.EnrollmentId, dto.ClassCurriculumId, dto.ExamId);

                if (markExists)
            {
                throw new InvalidOperationException(
                    "A mark already exists for this student for this subject in this exam.");
            }

            // Fetch the examination to get MaxMark for calculation
            var examination = await _examMarkRepository.GetExaminationByIdAsync(dto.ExamId);

                if (examination == null)
            {
                throw new KeyNotFoundException(
                    $"Examination with ID {dto.ExamId} not found.");
            }

            // Business rule: calculate percentage and grade automatically
            decimal percentage = 0;
            GradeType grade    = GradeType.Null;

            // Business rule: if student was absent, percentage stays 0 and grade = Null (do not calculate anything for absent students)
            if (!dto.ExamAbsent && examination.MaxMark > 0)
            {
                percentage = Math.Round((dto.MarksObtained / examination.MaxMark) * 100, 2); // // Percentage = (MarksObtained / MaxMark) * 100 , rounded to 2 decimal points
                
                grade = CalculateGrade(percentage); // grade assignment
            }

            // Map the incoming DTO fields onto a new ExamMark model object
            var examMark = new ExamMark
            {
                MarksObtained = dto.MarksObtained,
                Grade = grade, // auto calculated
                Percentage = percentage, // auto calculated
                Remarks = dto.Remarks,
                EntryDate = DateTime.UtcNow,
                ExamAbsent = dto.ExamAbsent,
                TeacherId = dto.TeacherId,
                ExamId = dto.ExamId,
                ClassCurriculumId = dto.ClassCurriculumId,
                EnrollmentId = dto.EnrollmentId
            };
            
            // Ask the repository to add this mark and save to the database
            await _examMarkRepository.AddAsync(examMark);
            await _examMarkRepository.SaveChangesAsync();

            // Reload the mark with all four navigation properties included
            var savedMark = await _examMarkRepository.GetByIdAsync(examMark.MarkId);

            return MapToResponseDto(savedMark!);
        }



        // 6. Update an existing exam mark
        public async Task<ExamMarkResponseDto?> UpdateAsync(int id, UpdateExamMarkDto dto, int? loggedInUserId)
        {
            // First fetch the existing mark from the database
            var examMark = await _examMarkRepository.GetByIdAsync(id);

            if (examMark == null) return null; // return null if the mark doesn't exist

            // If a userId was passed, verify they are the subject teacher
            if (loggedInUserId.HasValue)
            {
                await ValidateTeacherIsSubjectTeacherAsync(examMark.ClassCurriculumId, loggedInUserId.Value);
            }

            // Fetch the examination again to get MaxMark for recalculation
            var examination = await _examMarkRepository.GetExaminationByIdAsync(examMark.ExamId);

            if (examination == null)
            {
                throw new KeyNotFoundException(
                    $"Examination with ID {examMark.ExamId} not found.");
            }

            // Business rule: recalculate percentage and grade on every update
            decimal percentage = 0;
            GradeType grade    = GradeType.Null;

            if (!dto.ExamAbsent && examination.MaxMark > 0)
            {
                percentage = Math.Round((dto.MarksObtained / examination.MaxMark) * 100, 2);

                grade = CalculateGrade(percentage);
            }

            examMark.MarksObtained = dto.MarksObtained;
            examMark.ExamAbsent = dto.ExamAbsent;
            examMark.Remarks = dto.Remarks;
            examMark.Percentage = percentage;
            examMark.Grade = grade;

            // Tell the repository the mark has changed, then save
            _examMarkRepository.Update(examMark);
            await _examMarkRepository.SaveChangesAsync();

            return MapToResponseDto(examMark);
        }



        // 7. Delete an exam mark
        public async Task<bool> DeleteAsync(int id)
        {
            var examMark = await _examMarkRepository.GetByIdAsync(id);

            if (examMark == null) return false;

            _examMarkRepository.Delete(examMark);
            await _examMarkRepository.SaveChangesAsync();

            return true;
        }





        // -------------Sri Lankan Grading Scale---------------------
        // Calculates the grade based on the percentage scored
        private static GradeType CalculateGrade(decimal percentage)
        {
            return percentage switch
            {
                >= 75 => GradeType.A,   // 75% and above -> A 
                >= 65 => GradeType.B,   // 65% to 74%  -> B 
                >= 55 => GradeType.C,   // 55% to 64% -> C
                >= 35 => GradeType.S,   // 35% to 54% -> S
                _     => GradeType.W    // below 35% -> W
            };
        }



        // ---------------Subject Teacher Validation-----------------
        // Checks that the logged-in teacher is the SUBJECT TEACHER for this specific ClassCurriculumId
        private async Task ValidateTeacherIsSubjectTeacherAsync(int classCurriculumId, int loggedInUserId)
        {
            // 1. first Find the Teacher record linked to this user account
            var teacher = await _examMarkRepository.GetTeacherByUserIdAsync(loggedInUserId);

            // 2. if no teacher profile is linked to this user account, reject the action
            if (teacher == null)
            {
                throw new UnauthorizedAccessException(
                    "No teacher profile is linked to your account.");
            }

            // 3. Check the SubjectAllocations table
            // If this record does not exist, the teacher does not teach this subject
            bool isAllocated = await _examMarkRepository.IsTeacherAllocatedToSubjectAsync(classCurriculumId, teacher.TeacherId);

            // 4. Reject if no allocation found
            if (!isAllocated)
            {
                throw new UnauthorizedAccessException(
                    "You can only enter marks for subjects you are allocated to teach.");
            }
        }



        // Converts a raw ExamMark model into an ExamMarkResponseDto for the frontend
        private static ExamMarkResponseDto MapToResponseDto(ExamMark em)
        {
            return new ExamMarkResponseDto
            {
                MarkId = em.MarkId,
                MarksObtained = em.MarksObtained,
                Grade  = em.Grade.ToString(),
                Percentage = em.Percentage,
                Remarks = em.Remarks,
                EntryDate = em.EntryDate,
                ExamAbsent = em.ExamAbsent,

                // From Teacher (direct navigation property)
                TeacherId = em.TeacherId ?? 0,
                TeacherName = em.Teacher?.Name ?? string.Empty,

                // From Examination (direct navigation property)
                ExamId = em.ExamId,
                ExamName = em.Examination?.ExamName ?? string.Empty,

                // From Enrollment → Student (nested navigation)
                EnrollmentId = em.EnrollmentId,
                StudentName = em.Enrollment?.Student?.FullName ?? string.Empty,
                AdmissionNumber = em.Enrollment?.Student?.AdmissionNumber ?? string.Empty
            };
        }

    }
}