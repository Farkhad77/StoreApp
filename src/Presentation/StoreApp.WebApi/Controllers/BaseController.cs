using Microsoft.AspNetCore.Mvc;

namespace StoreApp.WebApi.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        [NonAction]
        public string? GetUserIdFromToken()
        {
            return User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
