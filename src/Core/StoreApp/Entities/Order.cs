using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities;

public class Order : BaseEntity
{
  
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }

    // Foreign Keys
    public int UserId { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
