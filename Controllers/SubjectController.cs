using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.DTOs.Subject;
using SchoolManagementSystem.Interfaces;

namespace SchoolManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }



        // GET api/subject
        // 1. Returns ALL subjects
        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetAll()
        {
            var subjects = await _subjectService.GetAllAsync();
            return Ok(subjects); // 200 OK with the full list
        }



        // GET api/subject/active
        // 2. Returns only active subjects
        [HttpGet("active")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetAllActive()
        {
            var subjects = await _subjectService.GetAllActiveAsync();
            return Ok(subjects); // 200 OK, or returns empty list if none are active
        }



        // GET api/subject/{id}
        // 3. Returns one subject by ID
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetById(int id)
        {
            var subject = await _subjectService.GetByIdAsync(id);

            // Service returns null if not found -> 404 not found
            if (subject == null)
                return NotFound(new { message = $"Subject with ID {id} not found." });

            return Ok(subject); // 200 OK with subject data
        }



        // POST api/subject
        // 4. Creates a new subject
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateSubjectDto dto)
        {
            // Check that all required fields passed their validation rules
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400 Bad Request with validation errors

            try
            {
                var subject = await _subjectService.CreateAsync(dto);

                // 201 Created 
                // Sets the Location header to api/subject/{id}
                return CreatedAtAction(nameof(GetById),
                    new { id = subject.SubjectId },
                    subject);
            }
            catch (InvalidOperationException ex)
            {
                // subject code is already taken
                //return 409 Conflict with the specific error message
                return Conflict(new { message = ex.Message });
            }
        }



        // PUT api/subject/{id}
        // 5. Updates all fields of an existing subject
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSubjectDto dto)
        {
            // Validate the incoming data
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var subject = await _subjectService.UpdateAsync(id, dto);

            // Service returns null if the subject was not found -> 404
            if (subject == null)
                return NotFound(new { message = $"Subject with ID {id} not found." });

            return Ok(subject); // 200 OK with updated subject data
        }



        // DELETE api/subject/{id}
        // 6. Permanently deletes a subject
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _subjectService.DeleteAsync(id);

            if (!result)
                return NotFound(new { message = $"Subject with ID {id} not found." });

            return NoContent();
        }
    }
}