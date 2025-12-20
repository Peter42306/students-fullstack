using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentsApi.Dtos;
using StudentsApi.Services;

namespace StudentsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }


        // GET: /api/students?search=john
        [HttpGet]
        public async Task<ActionResult<List<StudentReadDto>>> GetAll(
            [FromQuery] string? search, 
            CancellationToken ct = default)
        {
            var students = await _studentService.GetAllAsync(search, ct);
            return Ok(students);
        }

        // GET: /api/students/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<StudentReadDto>> GetById(
            int id,
            CancellationToken ct = default)
        {
            var student = await _studentService.GetByIdAsync(id, ct);

            if (student is null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        // POST: /api/students
        [HttpPost]
        public async Task<ActionResult<StudentReadDto>> Create(
            [FromBody] StudentCreateDto dto,
            CancellationToken ct)
        {
            var (ok, error, created) = await _studentService.CreateAsync(dto, ct);

            if (!ok)
            {
                return BadRequest(new { message = error});
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = created!.Id },
                created);
        }

        // PUT: /api/Students/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] StudentUpdateDto dto,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var existing = await _studentService.GetByIdAsync(id, ct);
            if (existing is null)
            {
                return NotFound();
            }

            var (ok, error) = await _studentService.UpdateAsync(id, dto, ct);
            if (!ok)
            {
                return BadRequest(new { message = error ?? "Update failed." });
            }

            return NoContent();
        }

        // DELETE: /api/students/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken ct)
        {
            var deleted = await _studentService.DeleteAsync(id, ct);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
