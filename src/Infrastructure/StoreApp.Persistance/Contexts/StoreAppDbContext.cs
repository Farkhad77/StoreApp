using Microsoft.EntityFrameworkCore;
using StoreApp.Domain.Entities;
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
    public DbSet<Category> Categories { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
}
