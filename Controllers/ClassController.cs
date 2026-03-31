using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.DTOs.Class;
using SchoolManagementSystem.Interfaces;

namespace SchoolManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            _classService = classService;
        }



        // GET api/class
        // 1. Returns a list of all classes across all years
        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetAll()
        {
            var classes = await _classService.GetAllAsync();
            return Ok(classes); // 200 OK with the list
        }



        // GET api/class/year/{yearId}
        // 2. Returns all classes belonging to a specific academic year
        [HttpGet("year/{yearId:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetByYear(int yearId)
        {
            var classes = await _classService.GetByYearAsync(yearId);
            return Ok(classes); // 200 OK, returns empty list if no classes found
        }



        // GET api/class/{id}
        // 3. Returns one class by its ID
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetById(int id)
        {
            var cls = await _classService.GetByIdAsync(id);

            // Service returns null if not found -> 404 not found
            if (cls == null)
                return NotFound(new { message = $"Class with ID {id} not found." });

            return Ok(cls); // 200 OK with class data
        }



        // POST api/class
        // 4. Creates a new class
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateClassDto dto)
        {
            // Check that all required fields passed their validation rules
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400 Bad Request with validation errors

            try
            {
                var cls = await _classService.CreateAsync(dto);

                // 201 Created
                // Sets the Location header to api/class/{id}
                return CreatedAtAction(nameof(GetById),
                    new { id = cls.ClassId },
                    cls);
            }
            catch (InvalidOperationException ex)
            {
                // Grade + Section + Year combination already exists
                return Conflict(new { message = ex.Message }); // 409
            }
        }



        // PUT api/class/{id}
        // 5. Updates all fields of an existing class — Admin only
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateClassDto dto)
        {
            // Validate the incoming data
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cls = await _classService.UpdateAsync(id, dto);

            // Service returns null if the class was not found
            if (cls == null)
                return NotFound(new { message = $"Class with ID {id} not found." }); // 404

            return Ok(cls); // 200 OK with updated class data
        }




        // DELETE api/class/{id}
        // 6. Deletes a class
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _classService.DeleteAsync(id);

            if (!result)
                return NotFound(new { message = $"Class with ID {id} not found." });

            return NoContent(); // successful delete, nothing to return
        }
    }
}