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
        private readonly IAzureStorageService _azureStorageService;
        private readonly IService _service;

        public GalleryController(IAzureStorageService azureStorageService, IService service)
        {
            _azureStorageService = azureStorageService;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> List()
        {
            var response = await _service.Gallery.Galleries();
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpGet("{eventId}")]
        public async Task<ActionResult> ById(int eventId)
        {
            var response = await _service.Gallery.Gallery(eventId);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }


        [HttpPost]
        public async Task<ActionResult> AddImageToGallery(AddGalleryImgDto addGalleryImgDto)
        {
            var response = await _service.Gallery.AddImageToGallery(addGalleryImgDto);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult> AddImageListToGallery(AddImageListToGalleryDto addImageListToGalleryDto)
        {
            var response = await _service.Gallery.AddImageListToGallery(addImageListToGalleryDto);

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
