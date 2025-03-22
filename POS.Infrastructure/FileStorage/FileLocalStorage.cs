using Microsoft.AspNetCore.Http;

namespace POS.Infrastructure.FileStorage
{
    public class FileLocalStorage : IFileLocalStorage
    {

        public async Task<string> SaveFileAsync(IFormFile file, string container, string webRootPath, string scheme, string host)
        {
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(webRootPath, container);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string path = Path.Combine(folder, fileName);

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                await File.WriteAllBytesAsync(path, content);
            }

            var currentUrl = $"{scheme}://{host}";
            var pathDb = Path.Combine(currentUrl, container, fileName).Replace("\\", "/");
            return pathDb;
        }

        public async Task<string> UpdateFileAsync(IFormFile file, string container, string route, string webRootPath, string scheme, string host)
        {
            await DeleteFileAsync(route, container, webRootPath);
            return await SaveFileAsync(file, container, webRootPath, scheme, host);
        }

        public Task DeleteFileAsync(string container, string route, string webRootPath)
        {
            if (string.IsNullOrEmpty(route))
            {
                return Task.CompletedTask;
            }

            var fileName = Path.GetFileName(route);
            var directoryFile = Path.Combine(webRootPath, container, fileName);

            if (File.Exists(directoryFile))
            {
                File.Delete(directoryFile);
            }

            return Task.CompletedTask;
        }

    }
}
