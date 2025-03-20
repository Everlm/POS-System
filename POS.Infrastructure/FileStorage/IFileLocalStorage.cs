using Microsoft.AspNetCore.Http;

namespace POS.Infrastructure.FileStorage
{
    public interface IFileLocalStorage
    {
        Task<string> SaveFileAsync(IFormFile file, string container, string webRootPath, string scheme, string host);
        Task<string> UpdateFileAsync(IFormFile file, string container, string route, string webRootPath, string scheme, string host);
        Task DeleteFileAsync(string container, string route, string webRootPath);
    }
}
