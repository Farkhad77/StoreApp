using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Abstracts.Services;
using StoreApp.Application.DTOs.CategoryDtos;
using StoreApp.Application.Shared;
using StoreApp.Persistence.Services;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class CategoriesController : ControllerBase
    {
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        private ICategoryService _categoryService { get; }
      
       
       /* [HttpPost("custom-route")]
        [Authorize]
        [ProducesResponseType(typeof(BaseResponse<CategoryUpdateDto>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Post([FromBody] CategoryCreateDto dto)
        {
            var result = await _categoryService.AddAsync(dto);
            return StatusCode((int)result.StatusCode, result);
        }*/

        // GET api/<CategoriesController>/5
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllAsync();
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto)
        {
            var result = await _categoryService.AddAsync(dto);
            return StatusCode((int)result.StatusCode, result);
        }

       
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _categoryService.DeleteAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
