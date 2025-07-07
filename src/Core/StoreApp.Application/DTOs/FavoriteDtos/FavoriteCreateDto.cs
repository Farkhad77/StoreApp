using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.DTOs.FavoriteDtos
{
    public record class FavoriteCreateDto
    {
        public Guid ProductId { get; set; }
    }
}
