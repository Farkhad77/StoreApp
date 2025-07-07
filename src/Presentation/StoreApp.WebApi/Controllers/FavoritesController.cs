using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Abstracts.Services;
using StoreApp.Application.DTOs.FavoriteDtos;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // İstifadəçi login olmalıdır
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoritesController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

       
        [HttpGet]
        public async Task<IActionResult> GetFavorites()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized();

            var result = await _favoriteService.GetUserFavoritesAsync(userId);
            return StatusCode((int)result.StatusCode, result);
        }

        
        [HttpPost]
        public async Task<IActionResult> AddFavorite([FromBody] FavoriteCreateDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized();

            var result = await _favoriteService.AddFavoriteAsync(dto, userId);
            return StatusCode((int)result.StatusCode, result);
        }

        
        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFavorite(Guid productId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized();

            var result = await _favoriteService.RemoveFavoriteAsync(productId, userId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
