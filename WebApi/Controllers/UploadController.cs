using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public UploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        //[HttpPost("image")]
        //public async Task<IActionResult> UploadImage(IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //        return BadRequest("No file uploaded.");

        //    var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
        //    if (!Directory.Exists(uploadsPath))
        //        Directory.CreateDirectory(uploadsPath);

        //    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        //    var filePath = Path.Combine(uploadsPath, fileName);

        //    using var stream = new FileStream(filePath, FileMode.Create);
        //    await file.CopyToAsync(stream);

        //    return Ok(new { imageUrl = fileName });
        //}

        [HttpPost("image")]
        public async Task<IActionResult> UploadImage(IFormFile file, [FromQuery] string existingFileName = null)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            string fileName;
            if (!string.IsNullOrEmpty(existingFileName))
            {
                // Delete old image
                var oldFilePath = Path.Combine(uploadsPath, existingFileName);
                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);

                fileName = existingFileName;
            }
            else
            {
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            }

            var filePath = Path.Combine(uploadsPath, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return Ok(new { imageUrl = fileName });
        }

    }
}
