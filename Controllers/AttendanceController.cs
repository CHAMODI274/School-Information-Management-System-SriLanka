using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.DTOs.Attendance;
using SchoolManagementSystem.Interfaces;
using System.Security.Claims;

namespace SchoolManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }



        // GET api/attendance/enrollment/{enrollmentId}
        // 1. Returns the full attendance history for a specific enrollment
        [HttpGet("enrollment/{enrollmentId:int}")]
        [Authorize(Roles = "Admin,Teacher,Student,Parent")]
        public async Task<IActionResult> GetByEnrollment(int enrollmentId)
        {
            var records = await _attendanceService
                .GetByEnrollmentAsync(enrollmentId);

            return Ok(records); // 200 OK, or returns empty list if no records
        }



        // GET api/attendance/class/{classId}/date/{date}
        // 2. Returns the attendance sheet for a whole class on a specific date
        [HttpGet("class/{classId:int}/date/{date:datetime}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetByClassAndDate(int classId, DateTime date)
        {
            var records = await _attendanceService
                .GetByClassAndDateAsync(classId, date);

            return Ok(records); // 200 OK, or returns empty list if none marked yet
        }



        // GET api/attendance/class/{classId}/month/{month}/year/{year}
        // 3. Returns all attendance records for a class for a specific month
        [HttpGet("class/{classId:int}/month/{month:int}/year/{year:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetByClassAndMonth(
            int classId, int month, int year)
        {
            // Validate month is a valid month number
            if (month < 1 || month > 12)
                return BadRequest(new { message = "Month must be between 1 and 12." });

            var records = await _attendanceService
                .GetByClassAndMonthAsync(classId, month, year);

            return Ok(records); // 200 OK, or returns empty list if no records
        }



        // GET api/attendance/school/date/{date}
        // 4. Returns attendance for the WHOLE school on a specific date
        [HttpGet("school/date/{date:datetime}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBySchoolAndDate(DateTime date)
        {
            var records = await _attendanceService
                .GetBySchoolAndDateAsync(date);

            return Ok(records); // 200 OK, or returns empty list if none marked
        }



        // GET api/attendance/summary/{enrollmentId}
        // 5. Returns an attendance summary for a specific enrollment
        [HttpGet("summary/{enrollmentId:int}")]
        [Authorize(Roles = "Admin,Teacher,Student,Parent")]  // Student and Parent can view their own, Admin and Teacher can view any        
        public async Task<IActionResult> GetAttendanceSummary(int enrollmentId)
        {
            var summary = await _attendanceService
                .GetAttendanceSummaryAsync(enrollmentId);

            return Ok(summary); // 200 OK with the summary data
        }



        // GET api/attendance/{id}
        // 6. Returns one attendance record by its ID
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetById(int id)
        {
            var attendance = await _attendanceService.GetByIdAsync(id);

            // Service returns null if not found -> 404 notFound
            if (attendance == null)
                return NotFound(new { message = $"Attendance record with ID {id} not found." });

            return Ok(attendance); // 200 OK with attendance data
        }



        // POST api/attendance
        // 7. Creates a single attendance record for one student
        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Create([FromBody] CreateAttendanceDto dto)
        {
            // Check that all required fields passed their validation rules
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400 Bad Request

            try
            {
                // If the logged-in user is a Teacher, pass their userId -> the service will verify they own the class
                // If Admin, pass null -> the ownership check is skipped
                var userId = GetLoggedInUserRole() == "Teacher" 
                    ? GetLoggedInUserId() 
                    : null;

                var attendance = await _attendanceService.CreateAsync(dto, userId);

                return CreatedAtAction(nameof(GetById),
                    new { id = attendance.AttendanceId },
                    attendance); // 201 Created
            }
            catch (UnauthorizedAccessException)
            {
                // Teacher tried to mark attendance for a class that isn't theirs
                return Forbid(); // 403 Forbidden
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message }); // 409 Conflict
            }
        }



        // POST api/attendance/bulk
        // 8. Marks attendance for multiple students at once
        [HttpPost("bulk")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> BulkCreate(
            [FromBody] IEnumerable<CreateAttendanceDto> dtos)
        {
            // Validate that the list is not null or empty
            if (dtos == null || !dtos.Any())
                return BadRequest(new { message = "No attendance records provided." });

            try
            {
                var userId = GetLoggedInUserRole() == "Teacher"
                    ? GetLoggedInUserId()
                    : null;

                var records = await _attendanceService.BulkCreateAsync(dtos, userId);
                return Ok(records);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }




        // 9. PUT api/attendance/{id}
        // Updates an attendance record  
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAttendanceDto dto)
        {
            // Validate the incoming data
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userId = GetLoggedInUserRole() == "Teacher"
                    ? GetLoggedInUserId()
                    : null;

                var attendance = await _attendanceService.UpdateAsync(id, dto, userId);

                // Service returns null if the record was not found
                if (attendance == null)
                    return NotFound(new
                    {
                        message = $"Attendance record with ID {id} not found."
                    });

                return Ok(attendance); // 200 OK with updated attendance data
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid(); // 403 Forbidden
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }



        // 10. DELETE api/attendance/{id}
        // Permanently removes an attendance record
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _attendanceService.DeleteAsync(id, null);

            if (!result)
                return NotFound(new
                {
                    message = $"Attendance record with ID {id} not found."
                });

            return NoContent();
        }





        // ------------------------Private Helpers-----------------------------//


        // 01
        // Reads the logged-in user's ID from their JWT token claims
        // Returns null if the claim is not found or cannot be parsed
        // Used to pass the userId to the service for teacher ownership validation
        private int? GetLoggedInUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                return userId;

                return null;
        }


        // 02
        // Reads the logged-in user's role from their JWT token claims
        // Returns null if the role claim is not found
        // Used to decide whether to apply the teacher ownership check
        private string? GetLoggedInUserRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value;
        }
    }
}