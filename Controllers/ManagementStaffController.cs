using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.DTOs.ManagementStaff;
using SchoolManagementSystem.Interfaces;

namespace SchoolManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ManagementStaffController : ControllerBase
    {
        private readonly IManagementStaffService _managementStaffService;

        public ManagementStaffController(IManagementStaffService managementStaffService)
        {
            _managementStaffService = managementStaffService;
        }



        // GET api/managementstaff
        // 1. Returns a list of all management staff members
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var staffList = await _managementStaffService.GetAllAsync();
            return Ok(staffList); // 200 OK with the list
        }



        // GET api/managementstaff/{id}
        // 2. Returns one management staff member by their ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var staff = await _managementStaffService.GetByIdAsync(id);

            // Service returns null if not found -> 404 not found
            if (staff == null)
                return NotFound(new { message = $"Management staff with ID {id} not found." });

            return Ok(staff); // 200 OK with staff data
        }



        // POST api/managementstaff
        // 3. Creates a new management staff record
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateManagementStaffDto dto)
        {
            // Check that all required fields passed their validation rules
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400 Bad Request with validation errors

            try
            {
                var staff = await _managementStaffService.CreateAsync(dto);

                // 201 Created
                // Sets the Location header to api/managementstaff/{id}
                return CreatedAtAction(nameof(GetById),
                    new { id = staff.EmployeeId },
                    staff);
            }
            catch (InvalidOperationException ex)
            {
                // Service threw this because NIC or email is already taken
                return Conflict(new { message = ex.Message }); // return 409 Conflict with the specific error message
            }
        }



        // PUT api/managementstaff/{id}
        // 4. Updates all fields of an existing management staff record
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateManagementStaffDto dto)
        {
            // Validate the incoming data
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var staff = await _managementStaffService.UpdateAsync(id, dto);

            // Service returns null if the record was not found
            if (staff == null)
                return NotFound(new { message = $"Management staff with ID {id} not found." });

            return Ok(staff); // 200 OK with updated staff data
        }



        // DELETE api/managementstaff/{id}
        // 5. Permanently deletes a management staff record
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _managementStaffService.DeleteAsync(id);

            if (!result)
                return NotFound(new { message = $"Management staff with ID {id} not found." });

            return NoContent(); // 204 No Content (successful delete, nothing to return)
        }

    }
}