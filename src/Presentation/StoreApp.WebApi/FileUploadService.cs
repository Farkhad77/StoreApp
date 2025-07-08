using StoreApp.Application.Abstracts.Services;
using StoreApp.Application.DTOs.FileDtos;

namespace StoreApp.WebApi
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _env;

        public FileUploadService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadAsync(FileUploadDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                throw new ArgumentException("Fayl boşdur.");

            var uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var originalFileName = Path.GetFileNameWithoutExtension(dto.File.FileName);
            var extension = Path.GetExtension(dto.File.FileName);
            var fileName = $"{originalFileName}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            int count = 1;
            while (System.IO.File.Exists(filePath))
            {
                fileName = $"{originalFileName}({count}){extension}";
                filePath = Path.Combine(uploadsFolder, fileName);
                count++;
            }

            using var stream = new FileStream(filePath, FileMode.Create);
            await dto.File.CopyToAsync(stream);

            return $"/uploads/{fileName}";
        }

    }
}

