using SchoolManagementSystem.DTOs.Attendance;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public AttendanceService(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }



        // 1. Get all attendance records for a specific enrollment
        public async Task<IEnumerable<AttendanceResponseDto>> GetByEnrollmentAsync(
            int enrollmentId)
        {
            var records = await _attendanceRepository
                .GetByEnrollmentAsync(enrollmentId);

            return records.Select(a => MapToResponseDto(a)); // Convert each Attendance model to an AttendanceResponseDto
        }



        // 2. Get all attendance records for a class on a specific date
        public async Task<IEnumerable<AttendanceResponseDto>> GetByClassAndDateAsync(
            int classId, DateTime date)
        {
            var records = await _attendanceRepository
                .GetByClassAndDateAsync(classId, date);

            return records.Select(a => MapToResponseDto(a));
        }



        // 3. Get all attendance records for a class for a specific month
        public async Task<IEnumerable<AttendanceResponseDto>> GetByClassAndMonthAsync(
            int classId, int month, int year)
        {
            var records = await _attendanceRepository
                .GetByClassAndMonthAsync(classId, month, year);

            return records.Select(a => MapToResponseDto(a));
        }



        // 4. Get all attendance records across the whole school for a specific date
        public async Task<IEnumerable<AttendanceResponseDto>> GetBySchoolAndDateAsync(
            DateTime date)
        {
            var records = await _attendanceRepository
                .GetBySchoolAndDateAsync(date);

            return records.Select(a => MapToResponseDto(a));
        }



        // 5. Get one attendance record by ID
        public async Task<AttendanceResponseDto?> GetByIdAsync(int id)
        {
            var attendance = await _attendanceRepository.GetByIdAsync(id);

            if (attendance == null) return null; // return null if not found

            return MapToResponseDto(attendance);
        }



        // 6. Get an attendance summary for a specific enrollment
        public async Task<AttendanceSummaryDto> GetAttendanceSummaryAsync(int enrollmentId)
        {
            // Get the attendance records
            var records = await _attendanceRepository
                .GetByEnrollmentAsync(enrollmentId);

            // Get present and absent counts
            var (present, absent) = await _attendanceRepository
                .GetAttendanceSummaryAsync(enrollmentId);

            int totalDays = present + absent;

            // Business rule: avoid dividing by zero if no attendance recorded yet
            decimal percentage = totalDays > 0
                ? Math.Round((decimal)present / totalDays * 100, 2)
                : 0;

            // Get student and class info from the first record if records exist
            var firstRecord = records.FirstOrDefault();

            return new AttendanceSummaryDto
            {
                EnrollmentId = enrollmentId,

                StudentName = firstRecord?.Enrollment?.Student?.FullName ?? string.Empty,
                AdmissionNumber = firstRecord?.Enrollment?.Student?.AdmissionNumber ?? string.Empty,
                Grade = firstRecord?.Enrollment?.Class?.Grade ?? string.Empty,
                Section = firstRecord?.Enrollment?.Class?.Section ?? string.Empty,
                Year = firstRecord?.Enrollment?.AcademicYear?.Year ?? string.Empty,

                TotalDaysPresent = present,
                TotalDaysAbsent = absent,
                TotalDays = totalDays,
                AttendancePercentage = percentage
            };
        }



        // 7. Create a single attendance record for one student
        public async Task<AttendanceResponseDto> CreateAsync(CreateAttendanceDto dto, int? loggedInUserId)
        {
            // If a userId was passed, this is a Teacher
            if (loggedInUserId.HasValue)
            {
                await ValidateTeacherOwnsClassAsync(dto.EnrollmentId, loggedInUserId.Value);
            }

            // Business rule: only one attendance record per student per day
            bool alreadyExists = await _attendanceRepository
                .AttendanceExistsAsync(dto.EnrollmentId, dto.Date);

                if (alreadyExists)
            {
                throw new InvalidOperationException(
                    "An attendance record already exists for this student on this date.");
            }

            // Map the incoming DTO fields onto a new Attendance model object
            var attendance = new Attendance
            {
                Date = dto.Date,
                Status = dto.Status,
                TeacherId = dto.TeacherId,
                EnrollmentId = dto.EnrollmentId
            };

            // Ask the repository to add this record and save to the database
            await _attendanceRepository.AddAsync(attendance);
            await _attendanceRepository.SaveChangesAsync();

            // Reload the record with all navigation properties included
            var savedAttendance = await _attendanceRepository
                .GetByIdAsync(attendance.AttendanceId);

            return MapToResponseDto(savedAttendance!);
        }




        // 8. Mark attendance for multiple students at once (marks attendance for a whole class)
        public async Task<IEnumerable<AttendanceResponseDto>> BulkCreateAsync(
            IEnumerable<CreateAttendanceDto> dtos, int? loggedInUserId )
        {
            var dtoList = dtos.ToList();

            // If a userId was passed, check ownership using the first record's enrollmentId
            // All records in a bulk create should be for the same class
            if (loggedInUserId.HasValue && dtoList.Any())
            {
                await ValidateTeacherOwnsClassAsync(
                    dtoList.First().EnrollmentId, loggedInUserId.Value);
            }

            var newRecords = new List<Attendance>();

            foreach (var dto in dtoList)
            {
                // Business rule: skip this student if attendance already exists for this date
                bool alreadyExists = await _attendanceRepository
                    .AttendanceExistsAsync(dto.EnrollmentId, dto.Date);

                if (alreadyExists) continue;

                newRecords.Add(new Attendance
                {
                    Date = dto.Date,
                    Status = dto.Status,
                    TeacherId = dto.TeacherId,
                    EnrollmentId = dto.EnrollmentId
                });
            }

            // If every student already had a record, return an empty list
            if (!newRecords.Any())
                return Enumerable.Empty<AttendanceResponseDto>();

            // Add all new records to the database in ONE single call
            await _attendanceRepository.AddRangeAsync(newRecords);
            await _attendanceRepository.SaveChangesAsync();

            // Reload all saved records with navigation properties included
            var newIds = newRecords.Select(r => r.AttendanceId).ToList();

            var savedRecords = new List<AttendanceResponseDto>();
            foreach (var id in newIds)
            {
                var record = await _attendanceRepository.GetByIdAsync(id);
                if (record != null)
                    savedRecords.Add(MapToResponseDto(record));
            }

            return savedRecords;
        }




        // 9. Update an attendance record
        public async Task<AttendanceResponseDto?> UpdateAsync(int id, UpdateAttendanceDto dto, int? loggedInUserId)
        {
            // First fetch the existing record from the database
            var attendance = await _attendanceRepository.GetByIdAsync(id);

            if (attendance == null) return null; // return null if the record doesn't exist

            // If a userId was passed, verify the teacher owns this class
            if (loggedInUserId.HasValue)
            {
                await ValidateTeacherOwnsClassAsync(
                    attendance.EnrollmentId, loggedInUserId.Value);
            }

            // Only Status is updatable
            attendance.Status = dto.Status;

            // Tell the repository the record has changed, then save
            _attendanceRepository.Update(attendance);
            await _attendanceRepository.SaveChangesAsync();

            return MapToResponseDto(attendance);
        }




        // 10. Delete an attendance record
        public async Task<bool> DeleteAsync(int id, int? loggedInUserId)
        {
            var attendance = await _attendanceRepository.GetByIdAsync(id);

            if (attendance == null) return false;

            if (loggedInUserId.HasValue)
            {
                await ValidateTeacherOwnsClassAsync(
                    attendance.EnrollmentId, loggedInUserId.Value);
            }

            _attendanceRepository.Delete(attendance);
            await _attendanceRepository.SaveChangesAsync();

            return true;
        }





        // ----------------------Private Validation Helper-------------------------------------------------------------
        // Checks that the logged-in teacher is the class teacher for the enrollment
        // If they are not, throws UnauthorizedAccessException
        private async Task ValidateTeacherOwnsClassAsync(int enrollmentId, int loggedInUserId)
        {
            // Find the Teacher record linked to this user account
            var teacher = await _attendanceRepository
                .GetTeacherByUserIdAsync(loggedInUserId);

            // If no teacher profile is linked to this user account, reject the action
            if (teacher == null)
            {
                throw new UnauthorizedAccessException(
                    "No teacher profile is linked to your account.");
            }

            // Get the ClassId for this enrollment
            var classId = await _attendanceRepository
                .GetClassIdByEnrollmentAsync(enrollmentId);

            // If the enrollment doesn't exist, reject the action
            if (classId == null)
            {
                throw new KeyNotFoundException(
                    "The enrollment record was not found.");
            }

            // Get the class and check if this teacher is the class teacher
            var cls = await _attendanceRepository.GetClassByIdAsync(classId.Value);

            // If the class doesn't exist, reject the action
            if (cls == null)
            {
                throw new KeyNotFoundException(
                    "The class record was not found.");
            }

            // The class teacher must match the logged-in teacher
            if (cls.TeacherId != teacher.TeacherId)
            {
                // If they don't match, the teacher is trying to access another class
                throw new UnauthorizedAccessException(
                    "You can only mark attendance for your own class.");
            }
        }





        // ----------------------Private Helper------------------------------------------------------------
        // Converts a raw Attendance model into an AttendanceResponseDto for the frontend
        private static AttendanceResponseDto MapToResponseDto(Attendance a)
        {
            return new AttendanceResponseDto
            {
                AttendanceId = a.AttendanceId,
                Date = a.Date,
                Status = a.Status.ToString(),

                // From Teacher
                TeacherId = a.TeacherId ?? 0,
                TeacherName = a.Teacher?.Name ?? string.Empty,

                // From Enrollment
                EnrollmentId = a.EnrollmentId,

                // From Enrollment -> Student (nested navigation via ThenInclude)
                StudentName = a.Enrollment?.Student?.FullName ?? string.Empty,
                AdmissionNumber = a.Enrollment?.Student?.AdmissionNumber ?? string.Empty
            };
        }

    }    
}