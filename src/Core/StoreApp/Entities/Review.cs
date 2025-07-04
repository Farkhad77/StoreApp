using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities
{
    public class Review : BaseEntity
    {
        public string Content { get; set; } = null!;
      
       
        public Guid ProductId { get; set; }
        public string UserId { get; set; }

   
        public Product Product { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
