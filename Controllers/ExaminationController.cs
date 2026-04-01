using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.DTOs.Examination;
using SchoolManagementSystem.Interfaces;

namespace SchoolManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExaminationController : ControllerBase
    {
        private readonly IExaminationService _examinationService;

        public ExaminationController(IExaminationService examinationService)
        {
            _examinationService = examinationService;
        }



        // GET api/examination
        // 1. Returns all examinations across all years
        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetAll()
        {
            var examinations = await _examinationService.GetAllAsync();
            return Ok(examinations); // 200 OK with the full list
        }



        // GET api/examination/year/{yearId}
        // 2. Returns all examinations for a specific academic year
        [HttpGet("year/{yearId:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetByYear(int yearId)
        {
            var examinations = await _examinationService.GetByYearAsync(yearId);

            return Ok(examinations); // 200 OK, or returns empty list if no exams
        }



        // GET api/examination/{id}
        // 3. Returns one examination by its ID
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetById(int id)
        {
            var examination = await _examinationService.GetByIdAsync(id);

            // Service returns null if not found -> 404 not found
            if (examination == null)
                return NotFound(new { message = $"Examination with ID {id} not found." });

            return Ok(examination); // 200 OK with examination data
        }



        // POST api/examination
        // 4. Creates a new examination record
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateExaminationDto dto)
        {
            // Check that all required fields passed their validation rules
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400 Bad Request with validation errors

            try
            {
                var examination = await _examinationService.CreateAsync(dto);

                // 201 Created
                // Sets the Location header to api/examination/{id}
                return CreatedAtAction(nameof(GetById),
                    new { id = examination.ExamId },
                    examination);
            }
            catch (InvalidOperationException ex)
            {
                // the same name already exists for this academic year
                return Conflict(new { message = ex.Message });
            }
        }



        // PUT api/examination/{id}
        // 5. Updates an existing examination
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateExaminationDto dto)
        {
            // Validate the incoming data
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var examination = await _examinationService.UpdateAsync(id, dto);

            // Service returns null if the examination was not found
            if (examination == null)
                return NotFound(new { message = $"Examination with ID {id} not found." });

            return Ok(examination); // 200 OK with updated examination data
        }



        // DELETE api/examination/{id}
        // 6. Deletes an examination
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _examinationService.DeleteAsync(id);

            if (!result)
                return NotFound(new { message = $"Examination with ID {id} not found." });

                return NoContent();
        }

    }    
}