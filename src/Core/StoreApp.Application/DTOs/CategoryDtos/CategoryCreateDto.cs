using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.DTOs.CategoryDtos
{
    public record class CategoryCreateDto
    {
        public string Name { get; set; } 
        public string? Description { get; set; }
      
        
      
    }
}
