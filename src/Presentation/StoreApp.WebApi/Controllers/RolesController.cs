using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Shared.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
   
           
            [HttpGet("permissions")]

            public IActionResult GetAllPermissions()
            {
                var permissions = PermissionHelper.GetAllPermissions();
                return Ok(permissions);

            }

       
        }


    }

