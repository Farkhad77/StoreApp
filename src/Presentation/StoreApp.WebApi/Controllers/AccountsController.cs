using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Abstracts.Services;
using StoreApp.Application.DTOs.UserDtos;
using StoreApp.Application.Shared;
using StoreApp.Persistence.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreApp.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IUserService _userService { get; }
        private IRedisCacheService _redisCacheService { get; }
        public AccountsController(IUserService userService, IRedisCacheService redisCacheService)
        {
            _userService = userService;
            _redisCacheService = redisCacheService;

        }
        [HttpPost("create")]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            var result = await _userService.RegisterAsync(dto);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<TokenResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var result = await _userService.Login(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("refresh token")]
        [Authorize]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest dto)
        {
            var result = await _userService.RefreshTokenAsync(dto);
            return StatusCode((int)result.StatusCode, result);
        }
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            string accessToken = Request.Headers["Authorization"]
                .ToString().Replace("Bearer ", "");

            string? refreshToken = GetRefreshTokenFromHeader(); // Aşağıdakı metodu əlavə et

            // Token bitmə vaxtını al
            TimeSpan expiry = GetTokenExpiry(accessToken); // Aşağıda bu da var

            // Access token Redis-ə blackliste at
            await _redisCacheService.SetAsync($"blacklist:access:{accessToken}", "true", expiry);

            // Refresh token də varsa onu da blackliste at
            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _redisCacheService.SetAsync($"blacklist:refresh:{refreshToken}", "true", TimeSpan.FromDays(7));
            }

            return Ok(new { message = "Logout successful" });
        }
        private static TimeSpan GetTokenExpiry(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var expiry = jwtToken.ValidTo;
            return expiry - DateTime.UtcNow;
        }

        private string? GetRefreshTokenFromHeader()
        {
            return Request.Headers.TryGetValue("Refresh-Token", out var value) ? value.ToString() : null;
        }

    }
}
