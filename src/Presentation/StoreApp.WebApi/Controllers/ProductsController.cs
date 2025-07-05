using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Abstracts.Services;
using StoreApp.Application.DTOs.ProductDtos;
using System.Security.Claims;

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
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _productService.GetAllProducts();
        return StatusCode((int)result.StatusCode, result);
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
        var userId = GetUserIdFromToken();
        if (userId == null)
            return Unauthorized();

        var result = await _productService.CreateProduct(dto, userId);
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

        var result = await _productService.UpdateProductAsync(dto, userId);
        return StatusCode((int)result.StatusCode, result);
    }



    // DELETE /api/products/{id}

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] ProductDeleteDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var result = await _productService.DeleteProductAsync(dto, userId);

        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }


}
