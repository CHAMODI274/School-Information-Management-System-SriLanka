using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.DTOs.Teacher;
using SchoolManagementSystem.Interfaces;

namespace SchoolManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }



        // GET api/teacher
        // 1. Returns a list of all teachers
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var teachers = await _teacherService.GetAllAsync();
            return Ok(teachers); // 200 OK with the list
        }



        // GET api/teacher/{id}
        // 2. Returns one teacher by their database ID
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Teacher")] // Admin & Teacher (Teacher can view their own profile)
        public async Task<IActionResult> GetById(int id)
        {
            var teacher = await _teacherService.GetByIdAsync(id);

            // Service returns null if not found -> 404 not found
            if (teacher == null)
                return NotFound(new { message = $"Teacher with ID {id} not found." });

            return Ok(teacher); // 200 OK with teacher data
        }



        // POST api/teacher
        // 3. Creates a new teacher record
        [HttpPost]
        [Authorize(Roles = "Admin")] // Admin only
        public async Task<IActionResult> Create([FromBody] CreateTeacherDto dto)
        {
            // Check that all required fields passed their validation rules
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400 Bad Request with validation errors

            try
            {
                var teacher = await _teacherService.CreateAsync(dto);

                // 201 Created
                // Sets the Location header to api/teacher/{id}
                return CreatedAtAction(nameof(GetById),
                    new { id = teacher.TeacherId },
                    teacher);
            }
            catch (InvalidOperationException ex)
            {
                // Service threw this because NIC or email is already taken
                // Controller return -> 409 Conflict with the error message
                return Conflict(new { message = ex.Message });
            }
        }



        // PUT api/teacher/{id}
        // 4. Updates all fields of an existing teacher record
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")] // Admin only
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTeacherDto dto)
        {
            // Validate the incoming data
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var teacher = await _teacherService.UpdateAsync(id, dto);

            // Service returns null if the teacher was not found
            if (teacher == null)
                return NotFound(new { message = $"Teacher with ID {id} not found." });

            return Ok(teacher); // 200 OK with updated teacher data
        }



        // DELETE api/teacher/{id}
        // 5. Permanently deletes a teacher record
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")] // Admin only
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _teacherService.DeleteAsync(id);

            if (!result)
                return NotFound(new { message = $"Teacher with ID {id} not found." });

            // 204 No Content (successful delete, nothing to return)
            return NoContent();
        }
    }
}