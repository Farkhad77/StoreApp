using StoreApp.Application.DTOs.ProductDtos;
using StoreApp.Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Abstracts.Services
{
    public interface IProductService
    {
        Task<BaseResponse<string?>> CreateProduct(ProductCreateDto dto, string userId);
        Task<BaseResponse<string?>> UpdateProductAsync(ProductUpdateDto dto, string userId);
        Task<BaseResponse<string?>> DeleteProductAsync(ProductDeleteDto dto, string userId);
        Task<BaseResponse<ProductGetDto>> GetProductByIdAsync(Guid id);
        Task<BaseResponse<List<ProductGetDto>>> GetAllProducts();
    }
}
