using StoreApp.Application.DTOs.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Abstracts.Services
{
    public interface IUserService
    {
        Task RegisterAsync(UserRegisterDto dto);
    }
}
