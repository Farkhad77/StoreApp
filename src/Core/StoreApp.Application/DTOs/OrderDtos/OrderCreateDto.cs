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
        public List<Guid> ProductIds { get; set; } // Çoxlu məhsul
        public string UserId { get; set; } = null!;


    }
}
