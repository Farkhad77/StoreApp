using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Abstracts.Repositories;
public interface ICategoryRepository : IRepository<Domain.Entities.Category>
{
    Task<List<Category>> GetByNameSearchAsync(string namePart);
}


