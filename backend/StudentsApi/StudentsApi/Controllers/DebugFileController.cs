using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentsApi.Services.Storage;

namespace StudentsApi.Controllers
{
    [Route("api/debug")]
    [ApiController]
    public class DebugFileController : ControllerBase
    {
        private readonly IFileStorage _fileStorage;

        public DebugFileController(IFileStorage fileStorage)
        {
            _fileStorage = fileStorage;
        }


        [HttpPost("upload")]
        public async Task<IActionResult> Upload(
            IFormFile file,
            CancellationToken ct)
        {
            var result = await _fileStorage.SaveAsync(
                file,
                subFolder: "debug",
                ct: ct);

            return Ok(result);
        }
    }
}
