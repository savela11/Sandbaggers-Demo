using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Utilities;

namespace Services.Interface
{
    public interface IAzureStorageService
    {
        Task<ServiceResponse<string>> UploadImage(IFormCollection formCollection, string containerName);
    }
}
