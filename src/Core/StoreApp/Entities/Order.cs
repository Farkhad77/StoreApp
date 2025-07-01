using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities;

public class Order : BaseEntity
{
    public string OrderStatus { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public int UserId { get; set; }
    public User user { get; set; }
    public ICollection<OrderProduct> OrderProducts { get; set; }

}
