using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StoreApp.Application.Abstracts.Services;
using StoreApp.Application.DTOs.UserDtos;
using StoreApp.Application.Shared;
using StoreApp.Application.Shared.Settings;
using StoreApp.Domain.Entities;
using StoreApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JWTSettings _jwtSetting;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, 
            IOptions<JWTSettings> jwtSetting, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSetting = jwtSetting.Value;
            _roleManager = roleManager;
        }

        public async Task<BaseResponse<string>> RegisterAsync(UserRegisterDto dto)
        {
            var existEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (existEmail != null)
            {
                return new BaseResponse<string>("This account already exists", HttpStatusCode.BadRequest);
            }

            User newUser = new()
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName
            };

            IdentityResult identityResult = await _userManager.CreateAsync(newUser, dto.Password);
            if (!identityResult.Succeeded)
            {
                var errorsMessage = string.Join(";", identityResult.Errors.Select(e => e.Description));
                return new BaseResponse<string>(errorsMessage, HttpStatusCode.BadRequest);
            }

            var roleName = dto.Role.ToString();

            // ✅ Əgər rol mövcud deyilsə, qeydiyyat uğursuz olur
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                return new BaseResponse<string>($"Role '{roleName}' is not available", HttpStatusCode.BadRequest);
            }

            await _userManager.AddToRoleAsync(newUser, roleName);

            return new BaseResponse<string>("Email registered successfully", HttpStatusCode.Created);
        }

        public async Task<BaseResponse<TokenResponse>> Login(UserLoginDto dto)
        {
            var existedEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (existedEmail is null)
            {
                return new("Email or password is wrong.", null, HttpStatusCode.NotFound);
            }

            /*if (!existedEmail.EmailConfirmed)
            {
                return new("Email is not confirmed.", null, HttpStatusCode.BadRequest);
            }*/

            SignInResult signInResult = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, true, true);
            if (!signInResult.Succeeded)
            {
                return new("Email or password is wrong.", null, HttpStatusCode.NotFound);
            }

            var token = await GenerateTokensAsync(existedEmail);
            return new("Token generated", token, HttpStatusCode.OK);
        }

        private async Task<TokenResponse> GenerateTokensAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSetting.SecretKey);

            // İstifadəçinin rollarını al
            var userRoles = await _userManager.GetRolesAsync(user);

            // Əsas claim-lər
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Email, user.Email!),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            // Rolları JWT-yə əlavə et
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Token ayarları
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSetting.ExpiryMinutes),
                Issuer = _jwtSetting.Issuer,
                Audience = _jwtSetting.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // Token və refresh token yarat
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiryDate = DateTime.UtcNow.AddHours(2);

            user.RefreshToken = refreshToken;
            user.ExpiryDate = refreshTokenExpiryDate;
            await _userManager.UpdateAsync(user);

            return new TokenResponse
            {
                Token = jwt,
                RefreshToken = refreshToken,
                ExpireDate = tokenDescriptor.Expires!.Value
            };
        }

        public async Task<BaseResponse<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var principal = GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
                return new("Invalid access token", null, HttpStatusCode.Unauthorized);

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId!);

            if (user == null)
                return new("User not found", null, HttpStatusCode.NotFound);

            if (user.RefreshToken is null || user.RefreshToken != request.RefreshToken || user.ExpiryDate < DateTime.UtcNow)
                return new("Invalid refresh token", null, HttpStatusCode.BadRequest);

            var newAccessToken = await GenerateTokensAsync(user);
            return new("Token refreshed", newAccessToken, HttpStatusCode.OK);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false, // Token-in vaxtını yoxlama
                ValidIssuer = _jwtSetting.Issuer,
                ValidAudience = _jwtSetting.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.SecretKey))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    return null;

                return principal;
            }
            catch
            {
                return null;
            }
        }
      
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}





