using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TestBlobStorage.Services;

namespace TestBlobStorage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IStorageManager _storageManager;

        public FileController(IStorageManager storageManager)
        {
            _storageManager = storageManager;
        }

        [HttpGet("getUrl")]
        public IActionResult GetUrl(string fileName)
        {
            try
            {
                var result = _storageManager.GetSignedUrl(fileName);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
        }

        [HttpGet("deleteUrl")]
        public IActionResult DeleteUrl(string fileName)
        {
            try
            {
                var result = _storageManager.DeleteFile(fileName);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
        }
        [HttpPost("uploadUrl")]
        public IActionResult UploadUrl(IFormFile formFile)
        {
            try
            {

                return Ok(_storageManager.UploadFile(formFile));
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
        }
    }
}
