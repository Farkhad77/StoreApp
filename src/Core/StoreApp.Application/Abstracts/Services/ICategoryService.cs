using StoreApp.Application.DTOs.CategoryDtos;
using StoreApp.Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Abstracts.Services
{
    public interface  ICategoryService
    {
        Task<BaseResponse<string>> AddAsync(CategoryCreateDto dto);
        Task<BaseResponse<string>> DeleteAsync(Guid id);
        Task<BaseResponse<CategoryUpdateDto>> UpdateAsync(CategoryUpdateDto dto);
        Task<BaseResponse<CategoryGetDto>> GetByIdAsync(Guid id);
        Task<BaseResponse<CategoryGetDto>> GetByNameAsync(string search);
        Task<BaseResponse<List<CategoryGetDto>>> GetAllAsync();
        Task<BaseResponse<List<CategoryGetDto>>> GetByNameSearchAsync(string namePart);
    }
}
