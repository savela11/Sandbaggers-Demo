using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Data.Repository.Interface;
using Models.DTO;
using Models.ViewModels;
using Services.Interface;
using Utilities;

namespace Services
{
    public class GalleryService : IGalleryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GalleryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<ServiceResponse<List<GalleryWithNoImagesVm>>> Galleries()
        {
            var serviceResponse = new ServiceResponse<List<GalleryWithNoImagesVm>>();

            try
            {
                var galleryList = await _unitOfWork.Gallery.GetAll(orderBy: gallery => gallery.OrderBy(g => g.Year));
                List<GalleryWithNoImagesVm> galleryWithNoImagesVmList = new List<GalleryWithNoImagesVm>();
                foreach (var gallery in galleryList)
                {
                    var galleryWithNoImagesVm = new GalleryWithNoImagesVm
                    {
                        Name = gallery.Name,
                        Year = gallery.Year,
                        EventId = gallery.EventId,
                        MainImg = gallery.MainImg,
                        NumOfImages = gallery.Images.Count
                    };

                    galleryWithNoImagesVmList.Add(galleryWithNoImagesVm);
                }

                serviceResponse.Data = galleryWithNoImagesVmList;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<EventGalleryVm>> Gallery(int eventId)
        {
            var serviceResponse = new ServiceResponse<EventGalleryVm>();

            try
            {
                var foundGallery = await _unitOfWork.Gallery.GetFirstOrDefault(g => g.EventId == eventId);

                if (foundGallery == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"No Gallery Found By Id";
                    return serviceResponse;
                }

                var galleryVm = new EventGalleryVm
                {
                    EventId = foundGallery.EventId,
                    Name = foundGallery.Name,
                    Year = foundGallery.Year,
                    MainImg = foundGallery.MainImg,
                    Images = new List<GalleryImgVm>()
                };
                foreach (var galleryImg in foundGallery.Images)
                {
                    var foundUser = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == galleryImg.CreatedByUserId);
                    var galleryImageVm = new GalleryImgVm
                    {
                        ImageId = galleryImg.ImageId,
                        Image = galleryImg.Image,
                        CreatedBy = new ImageCreatedByUserVm
                        {
                            FullName = foundUser.UserProfile.FirstName + " " + foundUser.UserProfile.LastName,
                            Image = foundUser.UserProfile.Image,
                            UserId = foundUser.Id
                        },
                        CreatedOn = galleryImg.CreatedOn,
                        Likes = new List<LikeImageVm>(),
                        Comments = new List<CommentImgVm>()
                    };
                    foreach (var likeUserId in galleryImg.LikedByUserIds)
                    {
                        var foundLikedByUser = await _unitOfWork.User.GetFirstOrDefault(user => user.Id == likeUserId);
                        var likeImageVm = new LikeImageVm {FullName = foundLikedByUser.UserProfile.FirstName + " " + foundLikedByUser.UserProfile.LastName};
                        galleryImageVm.Likes.Add(likeImageVm);
                    }

                    foreach (var comment in galleryImg.Comments)
                    {
                        var foundCommenterUser = await _unitOfWork.User.GetFirstOrDefault(user => user.Id == comment.CreatedByUserId);
                        var commentImgVm = new CommentImgVm
                        {
                            CommentId = comment.CommentId,
                            Comment = comment.Comment,
                            CreatedByUser = new CommentCreatedByUserVm
                            {
                                UserId = foundCommenterUser.Id,
                                Image = foundCommenterUser.UserProfile.Image,
                                FullName = foundCommenterUser.UserProfile.FirstName + " " + foundCommenterUser.UserProfile.LastName
                            },
                            CreatedOn = comment.CreatedOn
                        };
                        galleryImageVm.Comments.Add(commentImgVm);
                    }

                    galleryVm.Images.Add(galleryImageVm);
                }


                serviceResponse.Data = galleryVm;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> AddImageToGallery(AddGalleryImgDto addGalleryImgDto)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                var foundGallery = await _unitOfWork.Gallery.GetFirstOrDefault(gallery => gallery.EventId == addGalleryImgDto.EventId);
                if (foundGallery == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Gallery found by Event Id";
                    return serviceResponse;
                }

                var foundCreatedByUser = await _unitOfWork.User.GetFirstOrDefault(user => user.Id == addGalleryImgDto.CreatedByUserId, "UserProfile");

                if (foundCreatedByUser == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No User found by Id";
                    return serviceResponse;
                }


                var galleryImage = new GalleryImage
                {
                    Image = addGalleryImgDto.Image,
                    Comments = new List<GalleryImageComment>(),
                    CreatedOn = DateTime.Now,
                    CreatedByUserId = foundCreatedByUser.Id,
                    LikedByUserIds = new List<string>()
                };
                foundGallery.Images.Add(galleryImage);

                await _unitOfWork.Save();
                serviceResponse.Data = "Image added to Gallery";
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }


        public async Task<ServiceResponse<List<string>>> AddImageListToGallery(AddImageListToGalleryDto addImageListToGalleryDto)
        {
            var serviceResponse = new ServiceResponse<List<string>>();

            try
            {
                List<string> imagesAdded = new List<string>();

                foreach (var image in addImageListToGalleryDto.Images)
                {
                    var response = await AddImageToGallery(new AddGalleryImgDto
                        {Image = image, EventId = addImageListToGalleryDto.EventId, CreatedByUserId = addImageListToGalleryDto.CreatedByUserId});
                    imagesAdded.Add(response.Data);
                }

                serviceResponse.Data = imagesAdded;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }
    }
}
