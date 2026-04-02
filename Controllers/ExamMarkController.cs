using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.DTOs.ExamMarks;
using SchoolManagementSystem.Interfaces;
using System.Security.Claims;

namespace SchoolManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExamMarkController : ControllerBase
    {
        private readonly IExamMarkService _examMarkService;

        public ExamMarkController(IExamMarkService examMarkService)
        {
            _examMarkService = examMarkService;
        }



        // GET api/exammark/exam/{examId}/class/{classId}
        // 1. Returns all student marks for a specific exam in a specific class
        [HttpGet("exam/{examId:int}/class/{classId:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetByExamAndClass(int examId, int classId)
        {
            var marks = await _examMarkService.GetByExamAndClassAsync(examId, classId);
            return Ok(marks); // 200 OK, or returns empty list if no marks entered yet
        }



        // GET api/exammark/enrollment/{enrollmentId}
        // 2. Returns all results for a specific student across all exams and subjects
        [HttpGet("enrollment/{enrollmentId:int}")]
        [Authorize(Roles = "Admin,Teacher,Student,Parent")]
        public async Task<IActionResult> GetByEnrollment(int enrollmentId)
        {
            var marks = await _examMarkService.GetByEnrollmentAsync(enrollmentId);
            return Ok(marks); // 200 OK, or returns empty list if no marks yet
        }



        // GET api/exammark/exam/{examId}
        // 3. Returns all marks for a specific examination across all classes
        [HttpGet("exam/{examId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByExam(int examId)
        {
            var marks = await _examMarkService.GetByExamAsync(examId);
            return Ok(marks); // 200 OK, or returns empty list if no marks entered
        }



        // GET api/exammark/{id}
        // 4. Returns one exam mark record by its ID
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetById(int id)
        {
            var mark = await _examMarkService.GetByIdAsync(id);

            // Service returns null if not found
            if (mark == null)
                return NotFound(new { message = $"Exam mark with ID {id} not found." });

            return Ok(mark); // 200 OK with exam mark data
        }



        // POST api/exammark
        // 5. Creates a new exam mark for a student
        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Create([FromBody] CreateExamMarkDto dto)
        {
            // Check that all required fields passed their validation rules
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400 Bad Request with validation errors

            try
            {
                // If the logged-in user is a Teacher, pass their userId (the service will verify the subject teacher), or user is a If Admin, pass null
                var userId = GetLoggedInUserRole() == "Teacher" ? GetLoggedInUserId(): null;

                var mark = await _examMarkService.CreateAsync(dto, userId);

                // 201 Created
                // Sets the Location header to api/exammark/{id}
                return CreatedAtAction(nameof(GetById),
                    new { id = mark.MarkId },
                    mark);
            }
            catch (UnauthorizedAccessException)
            {
                // a teacher tried to enter marks for a subject they do not teach
                return Forbid(); // 403 Forbidden
            }
            catch (KeyNotFoundException ex)
            {
                // Examination or enrollment record not found
                return NotFound(new { message = ex.Message }); // 404 not found
            }
            catch (InvalidOperationException ex)
            {
                // Mark already exists for this student/subject/exam combination
                return Conflict(new { message = ex.Message }); // 409 Conflict
            }
        }



        // // PUT api/exammark/{id}
        // 6. Updates an existing exam mark
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateExamMarkDto dto)
        {
            // Validate the incoming data
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // subject teacher check
                var userId = GetLoggedInUserRole() == "Teacher" ? GetLoggedInUserId() : null;

                var mark = await _examMarkService.UpdateAsync(id, dto, userId);

                // Service returns null if the mark was not found
                if (mark == null)
                    return NotFound(new
                    {
                        message = $"Exam mark with ID {id} not found."
                    });

                return Ok(mark); // 200 OK with updated mark data
            }
            catch (UnauthorizedAccessException)
            {
                // Teacher tried to update marks for a subject they do not teach
                return Forbid(); // 403 Forbidden
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message }); // 404
            }
        }



        // DELETE api/exammark/{id}
        // 7. Permanently removes an exam mark
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _examMarkService.DeleteAsync(id);

            if (!result)
                return NotFound(new
                {
                    message = $"Exam mark with ID {id} not found."
                });

            return NoContent();
        }



        //-----------------Private Helpers---------------------

        // Reads the logged-in user's ID from their JWT token claims
        // Returns null if the claim is not found or cannot be parsed
        // Used to pass the userId to the service for subject teacher validation
        private int? GetLoggedInUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
             return userId;

             return null;
        }


        // Reads the logged-in user's role from their JWT token claims
        // Returns null if the role claim is not found
        // Used to decide whether to apply the subject teacher ownership check
        private string? GetLoggedInUserRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value;
        }
    }
}