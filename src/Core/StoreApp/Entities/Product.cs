using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace StoreApp.Domain.Entities;

public class Product : BaseEntity
{

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public Guid CategoryId { get; set; }
    public string UserId { get; set; }

    public Category Category { get; set; }
    public User User { get; set; }

    public ICollection<Image> Images { get; set; }
    public ICollection<OrderProduct> OrderProducts { get; set; }
    public ICollection<Favorite> Favorites { get; set; }
}
