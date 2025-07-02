using StoreApp.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Persistence.Repositories
{
    public class ProductRepository : Repository<Domain.Entities.Product>, Application.Abstracts.Repositories.IProductRepository
    {
        public ProductRepository(StoreAppDbContext context) : base(context)
        {
        }
    }
}
