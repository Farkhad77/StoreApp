/*using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using StoreApp.Application.Abstracts.Services;
using System.IO;
using System.Threading.Tasks;


namespace StoreApp.Infrastructure.Services
{
   

    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _env;

        public FileUploadService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
            var extension = Path.GetExtension(file.FileName);
            var fileName = originalFileName + extension;
            var filePath = Path.Combine(uploadsFolder, fileName);

            int count = 1;
            while (System.IO.File.Exists(filePath))
            {
                var tempFileName = $"{originalFileName}({count}){extension}";
                filePath = Path.Combine(uploadsFolder, tempFileName);
                fileName = tempFileName;
                count++;
            }

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/{fileName}";
        }
    }

}
*/