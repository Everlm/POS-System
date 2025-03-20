using Microsoft.AspNetCore.Http;

namespace POS.Application.Interfaces
{
    public interface IFileLocalStorageApplication
    {
        Task<string> SaveFileAsync(IFormFile file, string container);
        Task<string> UpdateFileAsync(IFormFile file, string container, string route);
        Task DeleteFileAsync(string container, string route);
    }
}
