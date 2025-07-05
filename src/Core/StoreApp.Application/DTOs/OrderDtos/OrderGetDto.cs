using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.DTOs.OrderDtos
{
    public record class OrderGetDto
    {
        public Guid Id { get; set; }
        public string UserID { get; set; } = null!;
    }
}
