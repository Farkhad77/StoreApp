using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Abstracts.Services;
using StoreApp.Application.DTOs.RoleDtos;
using StoreApp.Application.Shared.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private IRoleService _roleService { get; }
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpGet("permissions")]

        public IActionResult GetAllPermissions()
        {
            var permissions = PermissionHelper.GetAllPermissions();
            return Ok(permissions);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleCreateDto dto)
        {
            var result = await _roleService.CreateRole(dto);
            return StatusCode((int)result.StatusCode, result);
        }


    }


    }

