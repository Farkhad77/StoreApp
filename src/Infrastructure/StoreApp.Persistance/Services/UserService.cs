using Microsoft.AspNetCore.Identity;
using StoreApp.Application.Abstracts.Services;
using StoreApp.Application.DTOs.UserDtos;
using StoreApp.Application.Shared;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Persistence.Services
{
    public class UserService : IUserService
    {
        public UserManager<User> _userManager { get; }

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
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
    }
}
