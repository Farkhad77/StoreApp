using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Abstracts.Services;
using StoreApp.Application.DTOs.ProductDtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreApp.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]

    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _productService.GetProductByIdAsync(id);
        return StatusCode((int)result.StatusCode, result);
    }

    // POST /api/products
    [HttpPost]
    [Authorize(Roles = "Seller")]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
    {
        var userId = GetUserIdFromToken(); // string olacaq
        if (userId == null)
            return Unauthorized();

        dto.UserId = userId; // birbaşa mənimsədilir
        var result = await _productService.CreateProduct(dto);
        return StatusCode((int)result.StatusCode, result);
    }

    // PUT /api/products/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "Seller")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProductUpdateDto dto)
    {
        var userId = GetUserIdFromToken();
        if (userId == null)
            return Unauthorized();

        dto.Id = id;
        dto.UserId = userId; // string tipində birbaşa mənimsədilir

        var result = await _productService.UpdateProductAsync(dto);
        return StatusCode((int)result.StatusCode, result);
    }


    // DELETE /api/products/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Seller")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = GetUserIdFromToken();
        if (userId == null)
            return Unauthorized();

        var dto = new ProductDeleteDto
        {
            Id = id,
            UserId = userId
        };

        var result = await _productService.DeleteProductAsync(dto);
        return StatusCode((int)result.StatusCode, result);
    }

}
