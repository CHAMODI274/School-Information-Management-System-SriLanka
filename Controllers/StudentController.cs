using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.DTOs.Student;
using SchoolManagementSystem.Interfaces;

namespace SchoolManagementSystem.Controllers
{
    [ApiController] // enables automatic model validation and JSON handling
    [Route("api/[controller]")] // all endpoints start with api/student
    [Authorize] // every endpoint requires a valid JWT token (logged in)

    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }



        // GET api/student
        // 1. Returns a list of all students 
        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")] // Both Admin and Teacher can view all students
        public async Task<IActionResult> GetAll()
        {
            var students = await _studentService.GetAllAsync();
            return Ok(students); // 200 OK with the list
        }



        // GET api/student/{id}
        // 2. Returns one student by their database ID
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _studentService.GetByIdAsync(id);

            // 404 not found
            if (student == null)
                return NotFound(new { message = $"Student with ID {id} not found." });

            return Ok(student); // 200 OK with student data
        }



        // GET api/student/admission/{admissionNumber}
        // 3. Returns one student by their admission number
        [HttpGet("admission/{admissionNumber}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetByAdmissionNumber(string admissionNumber)
        {
            var student = await _studentService
                .GetByAdmissionNumberAsync(admissionNumber);

            if (student == null)
                return NotFound(new
                {
                    message = $"No student found with admission number '{admissionNumber}'."
                });

            return Ok(student);
        }



        // POST api/student
        // 4. Creates a new student record 
        [HttpPost]
        [Authorize(Roles = "Admin")] // Admin only
        public async Task<IActionResult> Create([FromBody] CreateStudentDto dto)
        {
            // Check that all required fields passed their validation rules
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400 Bad Request with validation errors

            try
            {
                var student = await _studentService.CreateAsync(dto);

                // 201 Created
                // Sets the Location header to api/student/{id}
                return CreatedAtAction(nameof(GetById),
                    new { id = student.StudentId },
                    student);
            }
            catch (InvalidOperationException ex)
            {
                // Service threw this exception because admission number is already taken
                // controller return 409 Conflict with the error message
                return Conflict(new { message = ex.Message });
            }
        }



        // PUT api/student/{id}
        // 5. Updates all fields of an existing student record
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")] // Admin only
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStudentDto dto)
        {
            // Validate the incoming data
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var student = await _studentService.UpdateAsync(id, dto);

            // Service returns null if the student was not found
            if (student == null)
                return NotFound(new { message = $"Student with ID {id} not found." });

            return Ok(student); // 200 OK with updated student data
        }



        // DELETE api/student/{id}
        // 6. Permanently deletes a student record
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")] // Admin only
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _studentService.DeleteAsync(id);

            if (!result)
                return NotFound(new { message = $"Student with ID {id} not found." });

            // 204 No Content (successful delete, nothing to return)
            return NoContent();
        }
    }
}