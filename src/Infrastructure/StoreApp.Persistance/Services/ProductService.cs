using StoreApp.Application.Abstracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Persistence.Services
{
  
    using Microsoft.EntityFrameworkCore;
    using global::StoreApp.Application.DTOs.ProductDtos;
    using global::StoreApp.Application.Shared;
   
    using global::StoreApp.Persistence.Contexts;
    using global::StoreApp.Domain.Entities;
    using System.Net;

    namespace StoreApp.Persistence.Services
    {
        public class ProductService : IProductService
        {
            private readonly StoreAppDbContext _context;

            public ProductService(StoreAppDbContext context)
            {
                _context = context;
            }

            public async Task<BaseResponse<string?>> CreateProduct(ProductCreateDto dto, string userId)
            {
                var product = new Product
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    Stock = dto.Stock,
                    UserId = userId

                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return new BaseResponse<string?>("Product created successfully", HttpStatusCode.Created);
            }

            public async Task<BaseResponse<string?>> UpdateProductAsync(ProductUpdateDto dto, string userId)
            {
                var product = await _context.Products.FindAsync(dto.Id);

                if (product == null)
                {
                    return new BaseResponse<string>("Məhsul tapılmadı",  HttpStatusCode.NotFound);
                }

                if (product.UserId != userId)
                {
                    return new BaseResponse<string>("Bu məhsulu dəyişmək səlahiyyətiniz yoxdur", false, HttpStatusCode.Forbidden);
                }

                product.Name = dto.Name;
                product.Price = dto.Price;

                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                return new BaseResponse<string>("Məhsul yeniləndi",  HttpStatusCode.OK);
            }

            public async Task<BaseResponse<string>> DeleteProductAsync(ProductDeleteDto dto, string userId)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == dto.Id);

                if (product == null)
                    return new BaseResponse<string>("Məhsul tapılmadı.",HttpStatusCode.NotFound);

                if (product.UserId != userId)
                    return new BaseResponse<string>("Bu məhsulu silmək icazəniz yoxdur.",HttpStatusCode.BadRequest);

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return new BaseResponse<string>("Məhsul uğurla silindi.", HttpStatusCode.OK);
            }



            public async Task<BaseResponse<ProductGetDto>> GetProductByIdAsync(Guid id)
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return new BaseResponse<ProductGetDto>("Product not found", null, HttpStatusCode.NotFound);

                var dto = new ProductGetDto
                {
                    Id = product.Id,
                   
                    Description = product.Description,
                    Price = product.Price
                    
                };

                return new BaseResponse<ProductGetDto>("Product detail", dto, HttpStatusCode.OK);
            }

            public async Task<BaseResponse<List<ProductGetDto>>> GetAllProducts()
            {
                var products = await _context.Products
                    .Select(p => new ProductGetDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        Stock = p.Stock
                    })
                    .ToListAsync();

                return new BaseResponse<List<ProductGetDto>>("List", products, HttpStatusCode.OK);
            }
        }
    }

}
