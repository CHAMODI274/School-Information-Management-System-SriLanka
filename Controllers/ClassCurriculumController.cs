using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.DTOs.ClassCurriculum;
using SchoolManagementSystem.Interfaces;

namespace SchoolManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClassCurriculumController : ControllerBase
    {
        private readonly IClassCurriculumService _classCurriculumService;

        public ClassCurriculumController(IClassCurriculumService classCurriculumService)
        {
            _classCurriculumService = classCurriculumService;
        }



        // GET api/classcurriculum
        // 1. Returns all curriculum entries across all classes and years
        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetAll()
        {
            var entries = await _classCurriculumService.GetAllAsync();
            return Ok(entries); // 200 OK with the full list
        }



        // GET api/classcurriculum/class/{classId}
        // 2. Returns all subjects assigned to a specific class
        [HttpGet("class/{classId:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetByClass(int classId)
        {
            var entries = await _classCurriculumService.GetByClassAsync(classId);
            return Ok(entries); // 200 OK, or returns empty list if no subjects assigned
        }



        // GET api/classcurriculum/year/{yearId}
        // 3. Returns all class-subject assignments for a specific academic year
        [HttpGet("year/{yearId:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetByYear(int yearId)
        {
            var entries = await _classCurriculumService.GetByYearAsync(yearId);
            return Ok(entries); // 200 OK, or returns empty list if no entries for that year
        }



        // GET api/classcurriculum/{id}
        // 4. Returns one curriculum entry by its ID
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetById(int id)
        {
            var entry = await _classCurriculumService.GetByIdAsync(id);

            // Service returns null if not found -> 404 not found
            if (entry == null)
                return NotFound(new { message = $"Class curriculum entry with ID {id} not found." });

            return Ok(entry); // 200 OK with curriculum entry data
        }



        // POST api/classcurriculum
        // 5. Assigns a subject to a class for a specific academic year — Admin only
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateClassCurriculumDto dto)
        {
            // Check that all required fields passed their validation rules
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400 Bad Request with validation errors

            try
            {
                var entry = await _classCurriculumService.CreateAsync(dto);

                // 201 Created
                // Sets the Location header to api/classcurriculum/{id}
                return CreatedAtAction(nameof(GetById),
                    new { id = entry.ClassCurriculumId },
                    entry);
            }
            catch (InvalidOperationException ex)
            {
                // same subject is already assigned to this class for this year
                return Conflict(new { message = ex.Message }); 
                // 409 Conflict with the specific error message
            }
        }



        // PUT api/classcurriculum/{id}
        // 6. Updates the subject of a curriculum entry
        // Only SubjectId can be changed
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateClassCurriculumDto dto)
        {
            // Validate the incoming data
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entry = await _classCurriculumService.UpdateAsync(id, dto);

            // Service returns null if the entry was not found -> 404
            if (entry == null)
                return NotFound(new { message = $"Class curriculum entry with ID {id} not found." });

            return Ok(entry); // 200 OK with updated curriculum entry data
        }



        // DELETE api/classcurriculum/{id}
        // 7. Remove a subject from a class for a year
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _classCurriculumService.DeleteAsync(id);

            if (!result)
                return NotFound(new { message = $"Class curriculum entry with ID {id} not found." });

            return NoContent(); // 204 No Content -> successful delete, nothing to return
        }
    }
}