using StoreApp.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Persistence.Repositories
{
    public class CategoryRepository : Repository<Domain.Entities.Category>, Application.Abstracts.Repositories.ICategoryRepository
    {
        public CategoryRepository(StoreAppDbContext context) : base(context)
        {
        }
    }
}
