using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentsApi.Dtos.Photos;
using StudentsApi.Services.Photos;

namespace StudentsApi.Controllers
{
    [Route("api/students/{studentId:int}")]
    [ApiController]
    public class StudentPhotosController : ControllerBase
    {
        private readonly IStudentPhotoService _photos;

        public StudentPhotosController(IStudentPhotoService photos)
        {
            _photos = photos;
        }


        [HttpPost("avatar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadAvatar(
            int studentId,
            [FromForm] UploadAvatarRequest request,
            CancellationToken ct = default)
        {
            var result = await _photos.UploadAvatarAsync(studentId, request.File, ct);

            return Ok(result);
        }


        [HttpGet("avatar")]
        public async Task<IActionResult> GetAvatar(
            int studentId,
            CancellationToken ct = default)
        {
            var result = await _photos.GetAvatarFileAsync(studentId, ct);
            return File(result.Stream, result.ContentType);
        }

        [HttpDelete("avatar")]
        public async Task<IActionResult> DeleteAvatar(
            int studentId,
            CancellationToken ct = default)
        {
            var deleted = await _photos.DeleteAvatarAsync(studentId, ct);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }



        [HttpPost("enclosures")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadEnclosures(
            int studentId,
            [FromForm] UploadEnclosuresRequest request,
            CancellationToken ct = default)
        {
            var result = await _photos.UploadEnclosuresAsync(studentId, request.Files, ct);
            return Ok(result);
        }

        [HttpGet("enclosures")]
        public async Task<IActionResult> GetEnclosures(
            int studentId,
            CancellationToken ct = default)
        {
            var result = await _photos.GetEnclosuresAsync(studentId, ct);
            return Ok(result);
        }

        [HttpGet("enclosures/{enclosureId:int}")]
        public async Task<IActionResult> GetEnclosure(
            int studentId,
            int enclosureId, 
            CancellationToken ct = default)
        {
            var result = await _photos.GetEnclosureAsync(studentId, enclosureId, ct);
            return File(result.Stream, result.ContentType);
        }        

        [HttpDelete("enclosures/{enclosureId:int}")]
        public async Task<IActionResult> DeleteEnclosure(
            int studentId,
            int enclosureId,
            CancellationToken ct = default)
        {
            var deleted = await _photos.DeleteEnclosureAsync(studentId, enclosureId, ct);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
