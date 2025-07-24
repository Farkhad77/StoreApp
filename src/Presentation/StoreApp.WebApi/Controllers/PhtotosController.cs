using Microsoft.AspNetCore.Mvc;
using StoreApp.Infrastructure.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhotosController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public PhotosController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var url = await _photoService.UploadImageAsync(file);
            return Ok(new { Url = url });
        }
    }

}
