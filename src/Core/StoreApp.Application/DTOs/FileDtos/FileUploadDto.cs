using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.DTOs.FileDtos
{
    public class FileUploadDto
    {
        [Required]
        public IFormFile File { get; set; }

        public string? Description { get; set; } // Əlavə sahə varsa
    }
}
