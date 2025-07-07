using StoreApp.Application.DTOs.UserDtos;
using StoreApp.Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Abstracts.Services
{
    public interface IUserService
    {
        Task<BaseResponse<string>> RegisterAsync(UserRegisterDto dto);
        Task<BaseResponse<TokenResponse>> Login(UserLoginDto dto);
        Task<BaseResponse<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);
        Task<BaseResponse<string>> AddUserToRoleAsync(string userId, string roleName);
    }
}
