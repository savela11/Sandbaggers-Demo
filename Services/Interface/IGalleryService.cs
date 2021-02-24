using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTO;
using Models.ViewModels;
using Utilities;

namespace Services.Interface
{
    public interface IGalleryService
    {
        Task<ServiceResponse<List<GalleryWithNoImagesVm>>> Galleries();
        Task<ServiceResponse<EventGalleryVm>> Gallery(int eventId);
        Task<ServiceResponse<string>> AddImageToGallery(AddGalleryImgDto addGalleryImgDto);

        Task<ServiceResponse<List<string>>> AddImageListToGallery(AddImageListToGalleryDto addImageListToGalleryDto);
    }
}
