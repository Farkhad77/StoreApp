using Microsoft.EntityFrameworkCore;
using StoreApp.Domain.Entities;
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
        private readonly StoreAppDbContext _context;
        public async Task<List<Category>> GetByNameSearchAsync(string namePart)
        {
            return await _context.Categories
                .Where(c => c.Name.Contains(namePart))
                .ToListAsync();
        }
    }
}
