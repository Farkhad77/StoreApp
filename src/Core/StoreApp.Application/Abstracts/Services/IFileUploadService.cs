using Microsoft.AspNetCore.Http;
using StoreApp.Application.DTOs.FileDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Abstracts.Services
{
    public interface IFileUploadService
    {
        Task<string> UploadAsync(FileUploadDto dto);
    }
}
