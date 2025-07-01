using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities;

public class Category : BaseEntity
{
    
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    // Navigation
    public ICollection<Product> Products { get; set; } = new List<Product>();

}
