using StoreApp.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Persistence.Repositories
{
    public class OrderRepository : Repository<Domain.Entities.Order>, Application.Abstracts.Repositories.IOrderRepository
    {
        public OrderRepository(StoreAppDbContext context) : base(context)
        {
        }
    }
}
