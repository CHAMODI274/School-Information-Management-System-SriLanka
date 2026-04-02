using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.DTOs.ExaminationClass;
using SchoolManagementSystem.Interfaces;

namespace SchoolManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExaminationClassController : ControllerBase
    {
        private readonly IExaminationClassService _examinationClassService;

        public ExaminationClassController(IExaminationClassService examinationClassService)
        {
            _examinationClassService = examinationClassService;
        }



        // GET api/examinationclass
        // 1. Returns all examination class records across all exams and classes
        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetAll()
        {
            var records = await _examinationClassService.GetAllAsync();
            return Ok(records); // 200 OK with the full list
        }



        // GET api/examinationclass/exam/{examId}
        // 2. Returns all classes scheduled for a specific examination
        [HttpGet("exam/{examId:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetByExam(int examId)
        {
            var records = await _examinationClassService.GetByExamAsync(examId);
            return Ok(records); // 200 OK, or returns empty list if no classes scheduled
        }



        // GET api/examinationclass/class/{classId}
        // 3. Returns all exams scheduled for a specific class
        [HttpGet("class/{classId:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetByClass(int classId)
        {
            var records = await _examinationClassService.GetByClassAsync(classId);
            return Ok(records); // 200 OK, or returns empty list if no exams scheduled
        }



        // GET api/examinationclass/{id}
        // 4. Returns one examination class record by its ID
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _examinationClassService.GetByIdAsync(id);

            // Service returns null if not found -> 404 not found
            if (record == null)
                return NotFound(new
                {
                    message = $"Examination class record with ID {id} not found."
                });

            return Ok(record); // 200 OK with examination class data
        }



        // POST api/examinationclass
        // 5. Create/Schedules an examination for a class
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateExaminationClassDto dto)
        {
            // Check that all required fields passed their validation rules
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400 Bad Request with validation errors

            try
            {
                var record = await _examinationClassService.CreateAsync(dto);

                // 201 Created
                // Sets the Location header to api/examinationclass/{id}
                return CreatedAtAction(nameof(GetById),
                    new { id = record.ExamClassId },
                    record);
            }
            catch (InvalidOperationException ex)
            {
                // same exam is already scheduled for this class
                return Conflict(new { message = ex.Message });
            }    
        }



        // PUT api/examinationclass/{id}
        // 6. Updates the scheduled date and time
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateExaminationClassDto dto)
        {
            // Validate the incoming data
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var record = await _examinationClassService.UpdateAsync(id, dto);

            // Service returns null if the record was not found
            if (record == null)
                return NotFound(new
                {
                    message = $"Examination class record with ID {id} not found."
                });

            return Ok(record); // 200 OK with updated examination class data
        }



        // DELETE api/examinationclass/{id}
        // 7. Permanently removes an examination class record
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _examinationClassService.DeleteAsync(id);

            if (!result)
                return NotFound(new
                {
                    message = $"Examination class record with ID {id} not found."
                });

            return NoContent();
        }
    }
}