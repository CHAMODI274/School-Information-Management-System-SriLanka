using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.DTOs.Promotion;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.Services
{
    public class PromotionService : IPromotionService
    {
        private const string FinalGrade = "Grade 11"; // hardcoded the final grade

        private readonly SchoolDbContext _context;

        public PromotionService(SchoolDbContext context)
        {
            _context = context;
        }




        //--------------- 1. Single Student Promotion-----------------------------------------------------------------

        public async Task<PromotionResultDto> PromoteStudentAsync(PromoteStudentDto dto)
        {
            // first fetch the current enrollment with all navigation properties
            var currentEnrollment = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Class)
                .Include(e => e.AcademicYear)
                .FirstOrDefaultAsync(e => e.EnrollmentId == dto.CurrentEnrollmentId);

            // Reject if the enrollment does not exist
            if (currentEnrollment == null)
            {
                throw new KeyNotFoundException($"Enrollment with ID {dto.CurrentEnrollmentId} not found.");
            }

            // Business rule: only Active enrollments can be promoted
            if (currentEnrollment.Status != EnrollmentStatus.Active)
            {
                throw new InvalidOperationException(
                    $"Student '{currentEnrollment.Student.FullName}' has an enrollment " +
                    $"status of '{currentEnrollment.Status}' and cannot be promoted. " +
                    $"Only Active enrollments can be promoted.");
            }

            // Check if the student is in the final grade (Grade 11)
            bool isFinalGrade = currentEnrollment.Class.Grade.Trim()
                .Equals(FinalGrade, StringComparison.OrdinalIgnoreCase);

            // Capture old enrollment details for the result DTO
            var result = new PromotionResultDto
            {
                StudentId = currentEnrollment.StudentId,
                StudentName = currentEnrollment.Student.FullName,
                AdmissionNumber = currentEnrollment.Student.AdmissionNumber,
                OldEnrollmentId = currentEnrollment.EnrollmentId,
                OldGrade = currentEnrollment.Class.Grade,
                OldSection = currentEnrollment.Class.Section,
                OldYear = currentEnrollment.AcademicYear.Year
            };

            // Handle Graduation
            if (isFinalGrade)
            {
                currentEnrollment.Status = EnrollmentStatus.Graduated;
                _context.Enrollments.Update(currentEnrollment);
                await _context.SaveChangesAsync();

                result.OldStatus = EnrollmentStatus.Graduated.ToString();
                result.NewGrade = "Graduated";
                result.NewSection = string.Empty;
                result.NewYear = string.Empty;

                return result;
            }

            // Handle Promotion
            var nextGradeName = GetNextGrade(currentEnrollment.Class.Grade);

            if (nextGradeName == null)
            {
                throw new InvalidOperationException(
                    $"Could not calculate the next grade for " + 
                    $"'{currentEnrollment.Class.Grade}'.");
            }

            // Find the matching class in the new year
            var nextClass = await _context.Classes
                .Include(c => c.AcademicYear)
                .FirstOrDefaultAsync(c =>
                    c.Grade.Trim() == nextGradeName.Trim() &&
                    c.Section.Trim() == currentEnrollment.Class.Section.Trim() &&
                    c.YearId == dto.NewYearId);

            if (nextClass == null)
            {
                throw new KeyNotFoundException(
                    $"{nextGradeName} {currentEnrollment.Class.Section} " +
                    $"not found in the new academic year. Please create it first.");
            }

            // Business rule: student cannot be enrolled twice in the same year
            bool alreadyEnrolled = await _context.Enrollments
                .AnyAsync(e => e.StudentId == currentEnrollment.StudentId &&
                               e.YearId == dto.NewYearId);

            if (alreadyEnrolled)
            {
                throw new InvalidOperationException(
                    $"Student '{currentEnrollment.Student.FullName}' is already " +
                    $"enrolled for the selected academic year.");
            }

            // Close old enrollment and create new enrollment
            currentEnrollment.Status = EnrollmentStatus.Promoted;
            _context.Enrollments.Update(currentEnrollment);

            var newEnrollment = new Enrollment
            {
                EnrollmentDate = DateTime.UtcNow,
                StudentId = currentEnrollment.StudentId,
                ClassId = nextClass.ClassId,
                YearId = dto.NewYearId,

                // Every new enrollment always starts as Active
                Status = EnrollmentStatus.Active
            };

            _context.Enrollments.Add(newEnrollment);

            await _context.SaveChangesAsync();

            result.OldStatus = EnrollmentStatus.Promoted.ToString();
            result.NewEnrollmentId = newEnrollment.EnrollmentId;
            result.NewGrade  = nextClass.Grade;
            result.NewSection = nextClass.Section;
            result.NewYear = nextClass.AcademicYear?.Year ?? string.Empty;

            return result;
        }




        // ------------------------2. Class Promotion ---------------------------------------------------------

        public async Task<ClassPromotionResultDto> ClassPromoteAsync(ClassPromoteDto dto)
        {
            // Fetch the new class for result DTO details
            var newClass = await _context.Classes
                .Include(c => c.AcademicYear)
                .FirstOrDefaultAsync(c => c.ClassId == dto.NewClassId);

            if (newClass == null)
            {
                throw new KeyNotFoundException($"New class with ID {dto.NewClassId} not found.");
            }

            //  fetch all ACTIVE enrollments for this class in the current year
            var enrollments = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Class)
                .Include(e => e.AcademicYear)
                .Where(e => e.ClassId == dto.CurrentClassId &&
                            e.YearId == dto.CurrentYearId &&
                            e.Status == EnrollmentStatus.Active)
                .ToListAsync();

            var classResult = new ClassPromotionResultDto();
            var results    = new List<PromotionResultDto>();

            // Process each student in the class
            foreach (var enrollment in enrollments)
            {
                classResult.TotalProcessed++;

                // Skip students already enrolled in the new year
                bool alreadyEnrolled = await _context.Enrollments
                    .AnyAsync(e => e.StudentId == enrollment.StudentId &&
                                   e.YearId == dto.NewYearId);

                if (alreadyEnrolled)
                {
                    classResult.TotalSkipped++;
                    continue;
                }

                // Close old enrollment as Promoted
                enrollment.Status = EnrollmentStatus.Promoted;
                _context.Enrollments.Update(enrollment);

                // Create new enrollment in the new class and year
                var newEnrollment = new Enrollment
                {
                    EnrollmentDate = DateTime.UtcNow,
                    StudentId = enrollment.StudentId,
                    ClassId = dto.NewClassId,
                    YearId = dto.NewYearId,
                    Status = EnrollmentStatus.Active
                };

                _context.Enrollments.Add(newEnrollment);

                results.Add(new PromotionResultDto
                {
                    StudentId = enrollment.StudentId,
                    StudentName = enrollment.Student.FullName,
                    AdmissionNumber = enrollment.Student.AdmissionNumber,
                    OldEnrollmentId = enrollment.EnrollmentId,
                    OldGrade = enrollment.Class.Grade,
                    OldSection = enrollment.Class.Section,
                    OldYear = enrollment.AcademicYear.Year,
                    OldStatus = EnrollmentStatus.Promoted.ToString(),
                    NewGrade = newClass.Grade,
                    NewSection = newClass.Section,
                    NewYear = newClass.AcademicYear?.Year ?? string.Empty
                });

                classResult.TotalSucceeded++;
            }

            //Save ALL changes
            await _context.SaveChangesAsync();

            foreach (var result in results)
            {
                var newEnrollment = await _context.Enrollments
                    .FirstOrDefaultAsync(e => e.StudentId == result.StudentId &&
                                              e.YearId == dto.NewYearId &&
                                              e.ClassId == dto.NewClassId);

                if (newEnrollment != null)
                    result.NewEnrollmentId = newEnrollment.EnrollmentId;
            }

            classResult.Results = results;
            return classResult;
        }




        // ---------------------Whole School Promotion ----------------------------------------
        public async Task<SchoolPromotionResultDto> PromoteWholeSchoolAsync(SchoolPromotionDto dto)
        {
            // Get all classes in the current year
            var currentClasses = await _context.Classes
                .Include(c => c.AcademicYear)
                .Where(c => c.YearId == dto.CurrentYearId)
                .ToListAsync();

            if (!currentClasses.Any())
            {
                throw new KeyNotFoundException("No classes found for the current academic year.");
            }

            var schoolResult = new SchoolPromotionResultDto();

            // process each class one by one
            foreach (var currentClass in currentClasses)
            {
                schoolResult.TotalClassesProcessed++;

                // Check if this is Grade 11
                bool isFinalGrade = currentClass.Grade.Trim()
                    .Equals(FinalGrade, StringComparison.OrdinalIgnoreCase);

                var classSummary = new ClassPromotionSummaryDto
                {
                    CurrentGrade = currentClass.Grade,
                    CurrentSection = currentClass.Section
                };

                // Find the next class automatically for other grades
                Models.Class? nextClass = null;

                if (!isFinalGrade)
                {
                    var nextGradeName = GetNextGrade(currentClass.Grade);

                    if (nextGradeName == null)
                    {
                        classSummary.CurrentGrade += " (SKIPPED — could not parse grade number)";
                        schoolResult.ClassResults.Add(classSummary);
                        continue;
                    }

                    // Find the matching class in the new year
                    nextClass = await _context.Classes
                        .Include(c => c.AcademicYear)
                        .FirstOrDefaultAsync(c =>
                            c.Grade.Trim() == nextGradeName.Trim() &&
                            c.Section.Trim() == currentClass.Section.Trim() &&
                            c.YearId == dto.NewYearId);

                    if (nextClass == null)
                    {
                        classSummary.CurrentGrade +=
                            $" (SKIPPED — {nextGradeName} {currentClass.Section} " +
                            $"not found in new year. Please create it first.)";
                        schoolResult.ClassResults.Add(classSummary);
                        continue;
                    }

                    classSummary.NewGrade   = nextClass.Grade;
                    classSummary.NewSection = nextClass.Section;
                }
                else
                {
                    // Grade 11 -> all students in this class Graduate
                    classSummary.NewGrade   = "Graduated";
                    classSummary.NewSection = string.Empty;
                }

                // Get all ACTIVE enrollments for this class in the current year
                var enrollments = await _context.Enrollments
                    .Include(e => e.Student)
                    .Where(e => e.ClassId == currentClass.ClassId &&
                                e.YearId == dto.CurrentYearId &&
                                e.Status == EnrollmentStatus.Active)
                    .ToListAsync();

                    // Process each student in this class
                    foreach (var enrollment in enrollments)
                {
                    schoolResult.TotalStudentsProcessed++;

                    // Skip students already enrolled in the new year
                    bool alreadyEnrolled = await _context.Enrollments
                        .AnyAsync(e => e.StudentId == enrollment.StudentId &&
                                       e.YearId == dto.NewYearId);

                    if (alreadyEnrolled)
                    {
                        schoolResult.TotalSkipped++;
                        classSummary.StudentsSkipped++;
                        continue;
                    }

                    if (isFinalGrade)
                    {
                        enrollment.Status = EnrollmentStatus.Graduated;
                        _context.Enrollments.Update(enrollment);
                        schoolResult.TotalGraduated++;
                        classSummary.StudentsGraduated++;
                    }
                    else
                    {
                        enrollment.Status = EnrollmentStatus.Promoted;
                        _context.Enrollments.Update(enrollment);

                        _context.Enrollments.Add(new Enrollment
                        {
                            EnrollmentDate = DateTime.UtcNow,
                            StudentId = enrollment.StudentId,
                            ClassId = nextClass!.ClassId,
                            YearId = dto.NewYearId,
                            Status = EnrollmentStatus.Active
                        });

                        schoolResult.TotalPromoted++;
                        classSummary.StudentsPromoted++;
                    }
                }
                schoolResult.ClassResults.Add(classSummary);
            }
            await _context.SaveChangesAsync();

            return schoolResult;
        }





        // Private Helper -> Automatically calculates the next grade name from the current grade
        private static string? GetNextGrade(string currentGrade)
        {
            var parts = currentGrade.Trim().Split(' '); // "Grade 10" -> ["Grade", "10"]

            if (parts.Length < 2) // expect at least two parts -> word and number
                return null;

            if (!int.TryParse(parts[^1], out int gradeNumber)) // "10" -> 10
                return null;

            // Increment the grade number and rebuild the string
            // ["Grade", "10"] → ["Grade", "11"] → "Grade 11"
            parts[^1] = (gradeNumber + 1).ToString();

            return string.Join(" ", parts);
        }

    }
}