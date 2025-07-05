using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Abstracts.Repositories;
using StoreApp.Application.Abstracts.Services;
using StoreApp.Application.DTOs.OrderDtos;
using StoreApp.Application.Shared;
using StoreApp.Domain.Entities;
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
        IOrderRepository _orderRepository;
        public OrderService(StoreAppDbContext context,IOrderRepository orderRepository)
        {
            _context = context;
            _orderRepository = orderRepository;
        }

        public async Task<BaseResponse<string>> CreateOrderAsync( OrderCreateDto dto, string userId)
        {
            if (dto.ProductIds == null || !dto.ProductIds.Any())
                return new BaseResponse<string>("Məhsul siyahısı boşdur.", false, HttpStatusCode.BadRequest);

            var products = await _context.Products
                .Where(p => dto.ProductIds.Contains(p.Id))
                .ToListAsync();

            if (products.Count != dto.ProductIds.Count)
                return new BaseResponse<string>("Bəzi məhsullar tapılmadı.", false, HttpStatusCode.NotFound);

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                OrderProducts = products.Select(p => new OrderProduct
                {
                    ProductId = p.Id
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

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
