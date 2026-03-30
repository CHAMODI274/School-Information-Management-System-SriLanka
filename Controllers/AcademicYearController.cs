using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.DTOs.AcademicYear;
using SchoolManagementSystem.Interfaces;

namespace SchoolManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AcademicYearController : ControllerBase
    {
        private readonly IAcademicYearService _academicYearService;

        public AcademicYearController(IAcademicYearService academicYearService)
        {
            _academicYearService = academicYearService;
        }



        // GET api/academicyear
        // 1. Returns a list of all academic years
        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetAll()
        {
            var years = await _academicYearService.GetAllAsync();
            return Ok(years); // 200 OK with the list
        }



        // GET api/academicyear/active
        // 2. Returns the currently active academic year
        [HttpGet("active")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetActive()
        {
            var year = await _academicYearService.GetActiveAsync();

            // Service returns null if no year is currently active -> 404
            if (year == null)
                return NotFound(new { message = "No active academic year found." });

            return Ok(year); // 200 OK with the active year
        }




        // GET api/academicyear/{id}
        // 3. Returns one academic year by its ID
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetById(int id)
        {
            var year = await _academicYearService.GetByIdAsync(id);

            // Service returns null if not found -> 404
            if (year == null)
                return NotFound(new { message = $"Academic year with ID {id} not found." });

            return Ok(year); // 200 OK with academic year data
        }




        // POST api/academicyear
        // 4. Creates a new academic year
        // New years are created as inactive by default
        [HttpPost]
        [Authorize(Roles = "Admin")] // admin only
        public async Task<IActionResult> Create([FromBody] CreateAcademicYearDto dto)
        {
            // Check that all required fields passed their validation rules
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400 Bad Request with validation errors

            try
            {
                var year = await _academicYearService.CreateAsync(dto);

                // 201 Created
                // Sets the Location header to api/academicyear/{id}
                return CreatedAtAction(nameof(GetById),
                    new { id = year.YearId },
                    year);
            }
            catch (InvalidOperationException ex)
            {
                // Service threw this because the year label already exists
                // return 409 Conflict with the specific error message
                return Conflict(new { message = ex.Message });
            }
        }




        // PUT api/academicyear/{id}
        // 5. Updates an academic year
        // If IsActive is set to true, all other years are automatically deactivated
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAcademicYearDto dto)
        {
            // Validate the incoming data
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400

            var year = await _academicYearService.UpdateAsync(id, dto);

            // Service returns null if the year was not found
            if (year == null)
                return NotFound(new { message = $"Academic year with ID {id} not found." });

            return Ok(year); // 200 OK with updated academic year data
        }




        // DELETE api/academicyear/{id}
        // 6. Deletes an academic year
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _academicYearService.DeleteAsync(id);

            if (!result)
                return NotFound(new { message = $"Academic year with ID {id} not found." });

            return NoContent();
        }
    }
}