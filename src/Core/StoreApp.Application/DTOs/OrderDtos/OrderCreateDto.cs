using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.DTOs.OrderDtos
{
    public record class OrderCreateDto
    {
       public List<Product> Products { get; set; } = new();

    }
}
