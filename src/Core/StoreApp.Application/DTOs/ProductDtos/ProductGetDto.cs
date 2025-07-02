using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.DTOs.ProductDtos
{
    public record class ProductGetDto
    {
        public Guid Id { get; set; }
    }
}
