using Microsoft.Extensions.DependencyInjection;
using StoreApp.Application.Abstracts.Repositories;
using StoreApp.Application.Abstracts.Services;
using StoreApp.Persistence.Repositories;
using StoreApp.Persistence.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Persistence
{
    public static class ServiceRegistration
    {
        public static void RegisterService(this IServiceCollection services)
        {
            #region Repositories
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUserRepository,UserRepository>();
            services.AddScoped<IOrderRepository,OrderRepository>();
            services.AddScoped<IProductRepository,ProductRepository>();
            services.AddScoped<IFavoriteRepository,FavoriteRepository>();
           
            #endregion

            #region Services
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IFavoriteService, FavoriteService>();
            services.AddScoped<IOrderService, OrderService>();
           
            #endregion

        }
    }
}
