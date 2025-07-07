using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Abstracts.Repositories;
using StoreApp.Application.Abstracts.Services;
using StoreApp.Application.DTOs.OrderDtos;
using StoreApp.Application.Shared;
using StoreApp.Domain.Entities;
using StoreApp.Domain.Enums;
using StoreApp.Infrastructure.Services;
using StoreApp.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Persistence.Services
{
    public class OrderService : IOrderService
    {
        private readonly StoreAppDbContext _context;
        private readonly IEmailService _emailService;
        IOrderRepository _orderRepository;
       
        public OrderService(StoreAppDbContext context,IOrderRepository orderRepository, IEmailService emailService)
        {
            _context = context;
            _orderRepository = orderRepository;
            _emailService = emailService;
        }

        public async Task<BaseResponse<string>> CreateOrderAsync(OrderCreateDto dto, string userId)
        {
            if (dto.ProductIds == null || !dto.ProductIds.Any())
                return new BaseResponse<string>("Məhsul siyahısı boşdur.", false, HttpStatusCode.BadRequest);

            var products = await _context.Products
                .Where(p => dto.ProductIds.Contains(p.Id))
                .ToListAsync();

            if (products.Count != dto.ProductIds.Count)
                return new BaseResponse<string>("Bəzi məhsullar tapılmadı.", false, HttpStatusCode.NotFound);

            // Stok yoxlaması
            foreach (var product in products)
            {
                if (product.Stock < dto.OrderCount)
                {
                    return new BaseResponse<string>($"'{product.Name}' üçün maksimum {product.Stock} ədəd sifariş edə bilərsiniz.", false, HttpStatusCode.BadRequest);
                }
            }

            var orderId = Guid.NewGuid();
            var totalPrice = products.Sum(p => p.Price * dto.OrderCount);
            var order = new Order
            {
                Id = orderId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                OrderStatus = OrderStatus.Pending.ToString(),
                TotalPrice = totalPrice,

                OrderProducts = products.Select(p => new OrderProduct
                {
                    ProductId = p.Id,
                    OrderId = orderId,
                 
                    OrderCount = dto.OrderCount,
                    Price = p.Price, // Məhsulun öz qiyməti istifadə olunur
                    CreatedAt = DateTime.UtcNow
                }).ToList()
            };

            _context.Orders.Add(order);

            // Stokdan çıx
            foreach (var product in products)
            {
                product.Stock-= dto.OrderCount;
            }

            await _context.SaveChangesAsync();

            // Email göndər
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null && !string.IsNullOrWhiteSpace(user.Email))
            {
                var productNames = string.Join(", ", products.Select(p => p.Name));
                var emailBody = "<h2>Sifarişiniz uğurla yaradıldı</h2>" +
                                $"<p><strong>Sifariş ID:</strong> {orderId}</p>" +
                                $"<p><strong>Məhsullar:</strong> {productNames}</p>" +
                                $"<p><strong>Status:</strong> {order.OrderStatus}</p>" +
                                $"<p><strong>Umumi mebleg:</strong> {order.TotalPrice}</p>" +
                                $"<p><strong>Tarix:</strong> {DateTime.UtcNow:yyyy-MM-dd HH:mm}</p>";

                await _emailService.SendEmailAsync(user.Email, "Sifarişiniz qəbul edildi", emailBody);
            }

            return new BaseResponse<string>("Sifariş uğurla yaradıldı.", true, HttpStatusCode.Created);
        }





        public async Task<List<Order>> GetMyOrdersAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User ID not found in token");

            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .ToListAsync();

            return orders;
        }

        public async Task<List<Order>> GetMySalesAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("Seller ID not found in token");

            // Satıcının məhsulları
            var productIds = await _context.Products
                .Where(p => p.UserId == userId)
                .Select(p => p.Id)
                .ToListAsync();

            // Həmin məhsullar olan OrderProduct-ları tap
            var orderIds = await _context.OrderProducts
                .Where(op => productIds.Contains(op.ProductId))
                .Select(op => op.OrderId)
                .Distinct()
                .ToListAsync();

            // Həmin sifarişləri gətir
            var orders = await _context.Orders
                .Where(o => orderIds.Contains(o.Id))
                .ToListAsync();

            return orders;
        }
        public async Task<BaseResponse<OrderGetDto>> GetOrderByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order is null)
            {
                return new BaseResponse<OrderGetDto>(HttpStatusCode.NotFound);
            }
            var dto = new OrderGetDto
            {
                Id = order.Id,
                
            };
            return new BaseResponse<OrderGetDto>("Data", dto, HttpStatusCode.OK);
        }
    }
}
