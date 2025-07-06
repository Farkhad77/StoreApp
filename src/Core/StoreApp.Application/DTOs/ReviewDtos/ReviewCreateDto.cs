using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.DTOs.ReviewDtos
{
    public record class ReviewCreateDto
    {
        public Guid ProductId { get; set; }
        public string Content { get; set; } = null!;
      
    }
}
