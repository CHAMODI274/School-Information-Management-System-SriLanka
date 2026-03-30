using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.DTOs.AdministrativeStaff;
using SchoolManagementSystem.Interfaces;

namespace SchoolManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] // admin only
    public class AdministrativeStaffController : ControllerBase
    {
        private readonly IAdministrativeStaffService _administrativeStaffService;

        public AdministrativeStaffController(IAdministrativeStaffService administrativeStaffService)
        {
            _administrativeStaffService = administrativeStaffService;
        }



        // GET api/administrativestaff
        // 1. Returns a list of all administrative staff members
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var staffList = await _administrativeStaffService.GetAllAsync();
            return Ok(staffList); // 200 OK with the list
        }



        // GET api/administrativestaff/{id}
        // 2. Returns one administrative staff member by their ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var staff = await _administrativeStaffService.GetByIdAsync(id);

            // 404 not founf
            if (staff == null)
                return NotFound(new { message = $"Administrative staff with ID {id} not found." });

            return Ok(staff); // 200 OK with staff data
        }



        // POST api/administrativestaff
        // 3. Creates a new administrative staff record
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAdministrativeStaffDto dto)
        {
            // Check that all required fields passed their validation rules
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400 Bad Request with validation errors

            try
            {
                var staff = await _administrativeStaffService.CreateAsync(dto);

                // 201 Created
                // Sets the Location header to api/administrativestaff/{id}
                return CreatedAtAction(nameof(GetById),
                    new { id = staff.EmployeeId },
                    staff);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }



        // PUT api/nonacademicstaff/{id}
        // 4. Updates all fields of an existing administrative staff record
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAdministrativeStaffDto dto)
        {
            // Validate the incoming data
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var staff = await _administrativeStaffService.UpdateAsync(id, dto);

            // Service returns null if the record was not found
            if (staff == null)
                return NotFound(new { message = $"Administrative staff with ID {id} not found." });

            return Ok(staff); // 200 OK with updated staff data
        }



        // DELETE api/administrativestaff/{id}
        // 5. Permanently deletes a administrative staff record
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _administrativeStaffService.DeleteAsync(id);

            if (!result)
                return NotFound(new { message = $"Administrative staff with ID {id} not found." });

            // 204 No Content
            return NoContent();
        }
    }
}