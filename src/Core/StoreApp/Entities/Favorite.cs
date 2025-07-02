using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities
{
    public class Favorite : BaseEntity
    {
        public int Name { get; set; } // Bu field-ın məqsədi qeyri-müəyyəndir. Əgər Name deyilsə, silmək olar.
        public Guid ProductId { get; set; }
        public string UserId { get; set; }

        public Product Product { get; set; }
        public User User { get; set; }
    }
}
