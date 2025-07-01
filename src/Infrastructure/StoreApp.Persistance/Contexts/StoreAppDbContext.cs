using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Persistence.Contexts;

public class StoreAppDbContext : DbContext
{
    public StoreAppDbContext(DbContextOptions<StoreAppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreAppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);

    }

    // Define DbSets for your entities here, e.g.:
    // public DbSet<Product> Products { get; set; }
    // public DbSet<Category> Categories { get; set; }
}
