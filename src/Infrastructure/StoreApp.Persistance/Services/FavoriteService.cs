using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Abstracts.Repositories;
using StoreApp.Application.Abstracts.Services;
using StoreApp.Application.DTOs.FavoriteDtos;
using StoreApp.Application.DTOs.ProductDtos;
using StoreApp.Application.Shared;
using StoreApp.Domain.Entities;
using StoreApp.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Persistence.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IRepository<Favorite> _favoriteRepository;
        private readonly IRepository<Product> _productRepository;

        public FavoriteService(IRepository<Favorite> favoriteRepository, IRepository<Product> productRepository)
        {
            _favoriteRepository = favoriteRepository;
            _productRepository = productRepository;
        }

        public async Task<BaseResponse<string>> AddFavoriteAsync(FavoriteCreateDto dto, string userId)
        {
            var exists = _favoriteRepository.GetByFiltered(f =>
                f.UserId == userId && f.ProductId == dto.ProductId).Any();

            if (exists)
                return new("Product already in favorites", HttpStatusCode.BadRequest);

            var favorite = new Favorite
            {
                UserId = userId,
                ProductId = dto.ProductId
            };

            await _favoriteRepository.AddAsync(favorite);
            await _favoriteRepository.SaveChangeAsync();

            return new("Product added to favorites", HttpStatusCode.Created);
        }

        public async Task<BaseResponse<List<ProductGetDto>>> GetUserFavoritesAsync(string userId)
        {
            var favoriteProducts = _favoriteRepository.GetByFiltered(
                f => f.UserId == userId,
                include: new Expression<Func<Favorite, object>>[] { f => f.Product }
            );

            var result = await favoriteProducts
                .Select(f => new ProductGetDto
                {
                    Id = f.Product.Id,
                    Name = f.Product.Name,
                    Price = f.Product.Price,
                    // Əgər əlavə sahə varsa buraya əlavə et
                })
                .ToListAsync();

            return new BaseResponse<List<ProductGetDto>>(result, HttpStatusCode.OK);
        }

        public async Task<BaseResponse<string>> RemoveFavoriteAsync(Guid productId, string userId)
        {
            var favorite = _favoriteRepository.GetByFiltered(f =>
                f.UserId == userId && f.ProductId == productId).FirstOrDefault();

            if (favorite is null)
                return new("Favorite not found", HttpStatusCode.NotFound);

            _favoriteRepository.Delete(favorite);
            await _favoriteRepository.SaveChangeAsync();

            return new("Favorite removed", HttpStatusCode.OK);
        }

    }
}       
