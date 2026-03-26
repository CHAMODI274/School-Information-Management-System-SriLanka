using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.DTOs.User;
using SchoolManagementSystem.Interfaces;
using System.Security.Claims;

namespace SchoolManagementSystem.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        // Constructor injection
        public UserController(IUserService userService)
        {
            _userService = userService;
        }



        // GET api/user
        // Returns a list of all users 
        [HttpGet]
        [Authorize(Roles = "Admin")] // Admin only
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users); // 200 OK with the list
        }



        // GET api/user/{id}
        // Returns one user by ID
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")] // Admin Only
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            //  if user not found Service returns null - 404
            if (user == null)
                return NotFound(new { message = $"User with ID {id} not found." });

            return Ok(user); // 200 OK with the user data
        }



        // POST api/user/change-password
        // Allows any logged-in user to change their OWN password
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            // Check that the incoming data passes validation rules defined in the DTO
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400 Bad Request with validation errors

            // Read the logged-in user's ID from the JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                           ?? User.FindFirst("sub");

            // If we can't read the user ID from the token, something is wrong
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized(new { message = "Could not identify the current user." });

            // Ask the service to change the password
            // The service will verify the current password and hash the new one
            var success = await _userService.ChangePasswordAsync(userId, dto);

            // Service returns false if the current password was wrong
            if (!success)
                return BadRequest(new { message = "Current password is incorrect." });

            return Ok(new { message = "Password changed successfully." }); // 200 OK
        }




        // PATCH api/user/{id}/status
        // Activate or deactivate a user account
        [HttpPatch("{id:int}/status")] 
        [Authorize(Roles = "Admin")] // Admin only
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateUserStatusDto dto)
        {
            var user = await _userService.UpdateStatusAsync(id, dto);

            if (user == null)
                return NotFound(new { message = $"User with ID {id} not found." });

            return Ok(user); // 200 OK with the updated user
        }



        // PATCH api/user/{id}/role
        // Change a user's role
        [HttpPatch("{id:int}/role")]
        [Authorize(Roles = "Admin")] // Admin only
        public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateUserRoleDto dto)
        {
            var user = await _userService.UpdateRoleAsync(id, dto);

            if (user == null)
                return NotFound(new { message = $"User with ID {id} not found." });

            return Ok(user); // 200 OK with the updated user
        }



        // DELETE api/user/{id}
        // Permanently delete a user
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")] // Admin only
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteAsync(id);

            if (!result)
                return NotFound(new { message = $"User with ID {id} not found." });

            return NoContent(); // 204 No Content
        }

    }
}