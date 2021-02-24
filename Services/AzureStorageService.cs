using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Services.Interface;
using Utilities;

namespace Services
{
    public class AdditionalData
    {
        public string Name { get; set; }
        public string Container { get; set; }
        public IFormFile Image { get; set; }
        public string Link { get; set; }
    }

    public class AzureStorageService : IAzureStorageService
    {
        private readonly AppSettingsExtension _appSettings;

        public AzureStorageService(IOptions<AppSettingsExtension> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public string GetContentType(string contentType)
        {
            var cType = "";
            switch (contentType)
            {
                case "image/svg+xml":
                    cType = "image/svg+xml";
                    break;
            }

            return cType;
        }

        public async Task<ServiceResponse<string>> UploadImage(IFormCollection formCollection, string containerName)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                var file = formCollection.Files[0];


                var blobServiceClient = new BlobServiceClient(_appSettings.AzureStorageCredentials);

                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                var blob = containerClient.GetBlobClient(file.FileName);

                var blobHttpHeader = new BlobHttpHeaders()
                {
                    ContentType = file.ContentType
                };


                var uploadedBlob = await blob.UploadAsync(file.OpenReadStream(), blobHttpHeader);
                var res = uploadedBlob.GetRawResponse();
                serviceResponse.Data = $"{blobServiceClient.Uri}{containerClient.Name}/{file.FileName}";
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
