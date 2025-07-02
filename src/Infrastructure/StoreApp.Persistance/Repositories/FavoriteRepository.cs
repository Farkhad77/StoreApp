using StoreApp.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Persistence.Repositories
{
    public class FavoriteRepository : Repository<Domain.Entities.Favorite>, Application.Abstracts.Repositories.IFavoriteRepository
    {
        public FavoriteRepository(StoreAppDbContext context) : base(context)
        {
        }
    }
}
