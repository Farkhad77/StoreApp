using Microsoft.AspNetCore.Identity;
using StoreApp.Application.Abstracts.Services;
using StoreApp.Application.DTOs.UserDtos;
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
        public Task RegisterAsync(UserRegisterDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
