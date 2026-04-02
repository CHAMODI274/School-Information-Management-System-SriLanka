using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.DTOs.Promotion;
using SchoolManagementSystem.Interfaces;

namespace SchoolManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }



        // POST api/promotion/student
        // 1. Promotes a SINGLE student individually
        [HttpPost("student")]
        public async Task<IActionResult> PromoteStudent([FromBody] PromoteStudentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _promotionService.PromoteStudentAsync(dto);
                return Ok(result); // 200 OK with the promotion result
            }
            catch (KeyNotFoundException ex)
            {
                // Enrollment or next class was not found
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Student is not Active or already enrolled for the new year
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Unexpected error
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }



        // POST api/promotion/class
        // Promotes ALL students in ONE specific class at once
        [HttpPost("class")]
        public async Task<IActionResult> PromoteClass([FromBody] ClassPromoteDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _promotionService.ClassPromoteAsync(dto);
                return Ok(result); // 200 OK with the class promotion summary
            }
            catch (KeyNotFoundException ex)
            {
                // New class was not found
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Business rule violation
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Unexpected error
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }



        // POST api/promotion/school
        // Promotes ALL students in the WHOLE SCHOOL at once
        [HttpPost("school")]
        public async Task<IActionResult> PromoteWholeSchool([FromBody] SchoolPromotionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _promotionService.PromoteWholeSchoolAsync(dto);
                return Ok(result); // 200 OK with the school-wide promotion summary
            }
            catch (KeyNotFoundException ex)
            {
                // No classes found for the current year
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Business rule violation
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Unexpected error
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }
    }
}