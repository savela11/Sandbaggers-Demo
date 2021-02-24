using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Services.Interface;

namespace API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class GalleryController : Controller
    {
        private readonly IGalleryService _galleryService;
        private readonly IAzureStorageService _azureStorageService;

        public GalleryController(IGalleryService galleryService, IAzureStorageService azureStorageService)
        {
            _galleryService = galleryService;
            _azureStorageService = azureStorageService;
        }


        [HttpGet]
        public async Task<ActionResult> Galleries()
        {
            var response = await _galleryService.Galleries();
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpGet("{eventId}")]
        public async Task<ActionResult> Gallery(int eventId)
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
        public async Task<ActionResult> AddImgListToGallery(AddImageListToGalleryDto addImageListToGalleryDto)
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
