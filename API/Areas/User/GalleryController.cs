using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Services.Interface;

namespace API.Areas.User
{
    [Area("User")]
    [Authorize(Policy = "User")]
    [ApiController]
    [Route("api/[area]/[controller]/[action]")]
    public class GalleryController : ControllerBase
    {
        private readonly IGalleryService _galleryService;
        private readonly IAzureStorageService _azureStorageService;

        public GalleryController(IGalleryService galleryService, IAzureStorageService azureStorageService)
        {
            _galleryService = galleryService;
            _azureStorageService = azureStorageService;
        }

        [HttpGet]
        public async Task<ActionResult> List()
        {
            var response = await _galleryService.Galleries();
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpGet("{eventId}")]
        public async Task<ActionResult> ById(int eventId)
        {
            var response = await _galleryService.Gallery(eventId);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }


        [HttpPost]
        public async Task<ActionResult> AddImageToGallery(AddGalleryImgDto addGalleryImgDto)
        {
            var response = await _galleryService.AddImageToGallery(addGalleryImgDto);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult> AddImageListToGallery(AddImageListToGalleryDto addImageListToGalleryDto)
        {
            var response = await _galleryService.AddImageListToGallery(addImageListToGalleryDto);

            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult> UploadImageToGallery(IFormCollection formCollection)

        {
            var response = await _azureStorageService.UploadImage(formCollection, "gallery");
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }
    }
}
