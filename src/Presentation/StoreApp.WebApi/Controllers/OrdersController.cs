using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Abstracts.Services;
using StoreApp.Application.DTOs.OrderDtos;
using StoreApp.Application.Shared;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;

       
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto dto)
        {
            var userId = GetUserIdFromToken();
            if (userId is null)
            {
                return Unauthorized(new BaseResponse<string>(
                    "İstifadəçi tapılmadı", false, HttpStatusCode.Unauthorized));
            }

            var result = await _orderService.CreateOrderAsync(dto,userId);
            return StatusCode((int)result.StatusCode, result);
        }
        
        [HttpGet("my-orders")]
        [Authorize(Policy = Permissions.Order.GetMyOrders)]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = GetUserIdFromToken(); // BaseController-dən gəlir
            var orders = await _orderService.GetMyOrdersAsync(userId);
            return Ok(orders);
        }
        
        [HttpGet("my-sales")]
        [Authorize(Policy = Permissions.Order.GetMySales)]
        public async Task<IActionResult> GetMySales()
        {
            var sellerId = GetUserIdFromToken();
            var sales = await _orderService.GetMySalesAsync(sellerId);
            return Ok(sales);
        }
    }
}
