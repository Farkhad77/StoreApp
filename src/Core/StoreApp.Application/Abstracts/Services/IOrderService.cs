using StoreApp.Application.DTOs.OrderDtos;
using StoreApp.Application.Shared;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Abstracts.Services
{
    public interface IOrderService
    {
      
        Task<List<Order>> GetMyOrdersAsync(string userId);
        Task<List<Order>> GetMySalesAsync(string userId);
        Task<BaseResponse<OrderGetDto>> GetOrderByIdAsync(OrderGetDto dto);
        Task<BaseResponse<string>> CreateOrderAsync(OrderCreateDto dto,string userId);
    }
}
