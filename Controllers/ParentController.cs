using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.DTOs.Parent;
using SchoolManagementSystem.Interfaces;

namespace SchoolManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ParentController : ControllerBase
    {
        private readonly IParentService _parentService;

        public ParentController(IParentService parentService)
        {
            _parentService = parentService;
        }


        // GET api/parent
        // Returns a list of all parents
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var parents = await _parentService.GetAllAsync();
            return Ok(parents); // 200 OK with the list
        }



        // GET api/parent/{id}
        // Returns one parent by their ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var parent = await _parentService.GetByIdAsync(id);

            // Service returns null if not found 404
            if (parent == null)
                return NotFound(new { message = $"Parent with ID {id} not found." });

            return Ok(parent); // 200 OK with parent data
        }



        // POST api/parent
        // Creates a new parent record
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateParentDto dto)
        {
            // Check that all required fields passed their validation rules
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400 Bad Request with validation errors

            var parent = await _parentService.CreateAsync(dto);

            // response: 201 Created
            // Also sets the Location header to api/parent/{id} so the client knows where to find it
            return CreatedAtAction(nameof(GetById),
                new { id = parent.ParentId },
                parent);
        }



        // PUT api/parent/{id}
        // Updates all fields of an existing parent record
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateParentDto dto)
        {
            // Validate the incoming data
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var parent = await _parentService.UpdateAsync(id, dto);

            // Service returns null if the parent was not found
            if (parent == null)
                return NotFound(new { message = $"Parent with ID {id} not found." });

            return Ok(parent); // 200 OK with updated parent data
        }



        // DELETE api/parent/{id}
        // Permanently deletes a parent record
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _parentService.DeleteAsync(id);

            if (!result)
                return NotFound(new { message = $"Parent with ID {id} not found." });

            return NoContent(); // 204 No Content: successful delete, nothing to return
        }
    }
}