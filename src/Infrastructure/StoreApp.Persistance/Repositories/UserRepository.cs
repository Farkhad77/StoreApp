using StoreApp.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Persistence.Repositories
{
    public class UserRepository : Repository<Domain.Entities.User>, Application.Abstracts.Repositories.IUserRepository
    {
        public UserRepository(StoreAppDbContext context) : base(context)
        {
        }
    }
}
