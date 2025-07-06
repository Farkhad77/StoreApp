using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StoreApp.Application.DTOs.OrderDtos
{
    public record class OrderCreateDto
    {
        [JsonPropertyName("productIds")]
        public List<Guid> ProductIds { get; set; } // Çoxlu məhsul
        public int OrderCount { get; set; } // Sifariş sayı
        public decimal Price { get; set; } // Sifariş qiyməti


    }
}
