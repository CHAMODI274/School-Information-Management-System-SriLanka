using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.DTOs.Enrollment;
using SchoolManagementSystem.Interfaces;

namespace SchoolManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }



        // GET api/enrollment
        // 1. Returns all enrollments across all students and classes
        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetAll()
        {
            var enrollments = await _enrollmentService.GetAllAsync();
            return Ok(enrollments); // 200 OK with the full list
        }



        // GET api/enrollment/class/{classId}
        // 2. Returns all students enrolled in a specific class
        [HttpGet("class/{classId:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetByClass(int classId)
        {
            var enrollments = await _enrollmentService.GetByClassAsync(classId);
            return Ok(enrollments); // 200 OK, or returns empty list if no students
        }



        // GET api/enrollment/student/{studentId}
        // 3. Returns all enrollments for a specific student across all years
        [HttpGet("student/{studentId:int}")]
        [Authorize(Roles = "Admin,Teacher,Student,Parent")]
        public async Task<IActionResult> GetByStudent(int studentId)
        {
            var enrollments = await _enrollmentService.GetByStudentAsync(studentId);
            return Ok(enrollments); // 200 OK, or returns empty list if no enrollments
        }



        // GET api/enrollment/{id}
        // 4. Returns one enrollment by its ID
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetById(int id)
        {
            var enrollment = await _enrollmentService.GetByIdAsync(id);

            // Service returns null if not found -> 404 not found
            if (enrollment == null)
                return NotFound(new { message = $"Enrollment with ID {id} not found." });

            return Ok(enrollment); // 200 OK with enrollment data
        }



        // POST api/enrollment
        // 5. Enrolls a student into a class for an academic year
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateEnrollmentDto dto)
        {
            // Check that all required fields passed their validation rules
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400 Bad Request with validation errors

            try
            {
                var enrollment = await _enrollmentService.CreateAsync(dto);

                // 201 Created
                // Sets the Location header to api/enrollment/{id}
                return CreatedAtAction(nameof(GetById),
                    new { id = enrollment.EnrollmentId },
                    enrollment);
            }
            catch (InvalidOperationException ex)
            {
                // the student is already enrolled for the selected academic year
                return Conflict(new { message = ex.Message }); // return 409 Conflict with the specific error message
            }
        } 



        // PUT api/enrollment/{id}
        // 6. Updates an enrollment's status or moves the student to a different class
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEnrollmentDto dto)
        {
            // Validate the incoming data
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var enrollment = await _enrollmentService.UpdateAsync(id, dto);

            // Service returns null if the enrollment was not found
            if (enrollment == null)
                return NotFound(new { message = $"Enrollment with ID {id} not found." });

            return Ok(enrollment); // 200 OK with updated enrollment data
        }



        // DELETE api/enrollment/{id}
        // 7. Permanently removes an enrollment record
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _enrollmentService.DeleteAsync(id);

            if (!result)
                return NotFound(new { message = $"Enrollment with ID {id} not found." });

            return NoContent(); // 204 no content to return
        }
    }

}