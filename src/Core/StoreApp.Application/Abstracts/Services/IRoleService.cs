using StoreApp.Application.DTOs.RoleDtos;
using StoreApp.Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Abstracts.Services
{
    public interface IRoleService
    {
        Task<BaseResponse<string?>> CreateRole(RoleCreateDto dto);
        Task<BaseResponse<string?>> DeleteRole(RoleDeleteDto dto);
    }

}
