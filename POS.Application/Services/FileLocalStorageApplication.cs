using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using POS.Application.Interfaces;
using POS.Infrastructure.FileStorage;

namespace POS.Application.Services
{
    public class FileLocalStorageApplication : IFileLocalStorageApplication
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IFileLocalStorage _fileLocalStorage;

        public FileLocalStorageApplication(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor contextAccessor,
                IFileLocalStorage fileLocalStorage)
        {
            _webHostEnvironment = webHostEnvironment;
            _contextAccessor = contextAccessor;
            _fileLocalStorage = fileLocalStorage;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string container)
        {
            var webRootPath = _webHostEnvironment.WebRootPath;
            var scheme = _contextAccessor.HttpContext!.Request.Scheme;
            var host = _contextAccessor.HttpContext!.Request.Host;

            return await _fileLocalStorage.SaveFileAsync(file, container, webRootPath, scheme, host.Value);
        }

        public async Task<string> UpdateFileAsync(IFormFile file, string container, string route)
        {
            var webRootPath = _webHostEnvironment.WebRootPath;
            var scheme = _contextAccessor.HttpContext!.Request.Scheme;
            var host = _contextAccessor.HttpContext!.Request.Host;

            return await _fileLocalStorage.UpdateFileAsync(file, container, route, webRootPath, scheme, host.Value);

        }

        public async Task DeleteFileAsync(string container, string route)
        {
            var webRootPath = _webHostEnvironment.WebRootPath;

            await _fileLocalStorage.DeleteFileAsync(container, route, webRootPath);
        }

    }
}
