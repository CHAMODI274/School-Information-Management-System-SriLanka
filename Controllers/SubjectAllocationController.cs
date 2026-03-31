using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.DTOs.SubjectAllocation;
using SchoolManagementSystem.Interfaces;

namespace SchoolManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SubjectAllocationController : ControllerBase
    {
        private readonly ISubjectAllocationService _subjectAllocationService;

        public SubjectAllocationController(ISubjectAllocationService subjectAllocationService)
        {
            _subjectAllocationService = subjectAllocationService;
        }



        // GET api/subjectallocation
        // 1. Returns all allocations across all teachers and classes
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var allocations = await _subjectAllocationService.GetAllAsync();
            return Ok(allocations); // 200 OK with the full list
        }



        // GET api/subjectallocation/teacher/{teacherId}
        // 2. Returns all subjects allocated to a specific teacher
        [HttpGet("teacher/{teacherId:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetByTeacher(int teacherId)
        {
            var allocations = await _subjectAllocationService
                .GetByTeacherAsync(teacherId);

            return Ok(allocations); // 200 OK, or returns empty list if no allocations
        }



        // GET api/subjectallocation/curriculum/{classCurriculumId}
        // 3. Returns all allocations for a specific curriculum entry
        [HttpGet("curriculum/{classCurriculumId:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetByCurriculum(int classCurriculumId)
        {
            var allocations = await _subjectAllocationService
                .GetByCurriculumAsync(classCurriculumId);

            return Ok(allocations); // 200 OK, or returns empty list if no allocations
        }



        // GET api/subjectallocation/{id}
        // 4. Returns one allocation by its ID
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetById(int id)
        {
            var allocation = await _subjectAllocationService.GetByIdAsync(id);

            // Service returns null if not found -> 404 notfound
            if (allocation == null)
                return NotFound(new { message = $"Subject allocation with ID {id} not found." });

            return Ok(allocation); // 200 OK with allocation data
        }



        // POST api/subjectallocation
        // 5. Assigns a teacher to a curriculum entry
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateSubjectAllocationDto dto)
        {
            // Check that all required fields passed their validation rules
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400 Bad Request with validation errors

            try
            {
                var allocation = await _subjectAllocationService.CreateAsync(dto);

                // 201 Created
                // Sets the Location header to api/subjectallocation/{id}
                return CreatedAtAction(nameof(GetById),
                    new { id = allocation.AllocationId },
                    allocation);
            }
            catch (InvalidOperationException ex)
            {
                // Service threw this because the teacher is already allocated to this curriculum entry
                return Conflict(new { message = ex.Message }); // return 409 Conflict with the specific error message
            }
        }



        // PUT api/subjectallocation/{id}
        // 6. Reassigns a curriculum entry to a different teacher
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSubjectAllocationDto dto)
        {
            // Validate the incoming data
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var allocation = await _subjectAllocationService.UpdateAsync(id, dto);

            // Service returns null if the allocation was not found
            if (allocation == null)
                return NotFound(new { message = $"Subject allocation with ID {id} not found." });

            return Ok(allocation); // 200 OK with updated allocation data
        }



        // DELETE api/subjectallocation/{id}
        // 7. Removes a teacher from a curriculum entry
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _subjectAllocationService.DeleteAsync(id);

            if (!result)
                return NotFound(new { message = $"Subject allocation with ID {id} not found." });

            return NoContent();
        }
    }
}