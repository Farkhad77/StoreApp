using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Abstracts.Services;
using StoreApp.Application.DTOs.OrderDtos;
using StoreApp.Application.Shared;
using StoreApp.Persistence.Contexts;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly StoreAppDbContext _context;



        public OrdersController(IOrderService orderService,StoreAppDbContext context)
        {
            _orderService = orderService;
            _context = context;
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
        [HttpPut("change-status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeOrderStatus([FromQuery] Guid orderId, [FromQuery] string newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return NotFound($"Order with ID {orderId} not found.");

            if (string.IsNullOrWhiteSpace(newStatus))
                return BadRequest("Yeni status boş ola bilməz.");
            order.OrderStatus = newStatus;

            await _context.SaveChangesAsync();

            return Ok($"Order status updated to '{newStatus}'.");
        }
    }
}
