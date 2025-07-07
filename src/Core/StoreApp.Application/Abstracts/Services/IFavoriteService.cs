using StoreApp.Application.DTOs.FavoriteDtos;
using StoreApp.Application.DTOs.ProductDtos;
using StoreApp.Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Abstracts.Services
{
    public interface IFavoriteService
    {
        Task<BaseResponse<string>> AddFavoriteAsync(FavoriteCreateDto dto, string userId);
        Task<BaseResponse<List<ProductGetDto>>> GetUserFavoritesAsync(string userId);
        Task<BaseResponse<string>> RemoveFavoriteAsync(Guid productId, string userId);
    }
}
