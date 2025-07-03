using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StoreApp.Application.Abstracts.Services;
using StoreApp.Application.DTOs.UserDtos;
using StoreApp.Application.Shared;
using StoreApp.Application.Shared.Settings;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Persistence.Services
{
    public class UserService : IUserService
    {
        public UserManager<User> _userManager { get; }
        private SignInManager<User> _signInManager { get; }
        private JWTSettings _jwtSetting { get; }

        public UserService(UserManager<User> userManager,SignInManager<User> signInManager, IOptions<JWTSettings> jwtSetting )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSetting = jwtSetting.Value;
        }
        public async Task<BaseResponse<string>> RegisterAsync(UserRegisterDto dto)
        {
          var existEmail=   await _userManager.FindByEmailAsync(dto.Email);
            if (existEmail != null)
            {
                return new BaseResponse<string>("This account already exists", System.Net.HttpStatusCode.BadRequest);

            }
            User newUser = new()
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName= dto.FullName

            };
           IdentityResult identityResult= await _userManager.CreateAsync(newUser, dto.Password);
            if (identityResult.Succeeded == false)
            {
                var errors = identityResult.Errors;
                StringBuilder errorsMessage = new StringBuilder();
                foreach (var error in errors)
                {
                    errorsMessage.Append(error.Description + ";");
                }
                return new(errorsMessage.ToString(), System.Net.HttpStatusCode.BadRequest);
            }
            return new BaseResponse<string>("Email registered successfully", System.Net.HttpStatusCode.Created);
        }

     

       
            public async Task<BaseResponse<TokenResponse>> Login(UserLoginDto dto)
            {
                var existedEmail = await _userManager.FindByEmailAsync(dto.Email);
                if (existedEmail is null)
                {
                    return new("Email or password is wrong.", null, System.Net.HttpStatusCode.NotFound);
                }

                if (!existedEmail.EmailConfirmed)
                {
                    return new("Email is not confirmed.", null, System.Net.HttpStatusCode.BadRequest);
                }

                SignInResult signInResult = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, true, true);

                if (!signInResult.Succeeded)
                {
                    return new("Email or password is wrong.", null, System.Net.HttpStatusCode.NotFound);
                }

                var token = await GenerateTokensAsync(existedEmail);
                return new("Token generated", token, System.Net.HttpStatusCode.OK);
            }
        private async Task<TokenResponse> GenerateTokensAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSetting.SecretKey);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
        };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));

                // Hər rol üçün permission-ları əlavə et
                var identityRole = await _roleManager.FindByNameAsync(role);
                if (identityRole != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(identityRole);
                    foreach (var claim in roleClaims.Where(c => c.Type == "Permission"))
                    {
                        claims.Add(claim);
                    }
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSetting.ExpiryMinutes),
                Issuer = _jwtSetting.Issuer,
                Audience = _jwtSetting.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiryDate = DateTime.UtcNow.AddHours(2); // Refresh token valid for 7 days
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
    }
 }

