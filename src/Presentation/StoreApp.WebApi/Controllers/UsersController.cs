using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Abstracts.Services;
using StoreApp.Application.DTOs.UserDtos;
using StoreApp.Domain.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;

        public UsersController(UserManager<User> userManager, IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        // GET /api/users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users.Select(u => new
            {
                u.Id,
                u.UserName,
                u.Email,
                u.FullName
            }).ToList();

            return Ok(users);
        }

        // GET /api/users/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound($"User with id '{id}' not found.");

            var userDto = new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.FullName
            };

            return Ok(userDto);
        }
        [HttpPost("assign-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole([FromBody] UserAssignRoleDto dto)
        {
            var result = await _userService.AddUserToRoleAsync(dto.UserId, dto.RoleName);
            return StatusCode((int)result.StatusCode, result);
        }

    }
}
