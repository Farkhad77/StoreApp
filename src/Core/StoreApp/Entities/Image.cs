using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities
{
    public class Image : BaseEntity
    {
       
        public string Image_Url { get; set; } // Əgər bu URL-dirsə, string tip olmalıdır!
        public Guid ProductId { get; set; }

        public Product Product { get; set; }
    }
}
