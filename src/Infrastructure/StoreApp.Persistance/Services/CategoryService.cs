using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Abstracts.Repositories;
using StoreApp.Application.Abstracts.Services;
using StoreApp.Application.DTOs.CategoryDtos;
using StoreApp.Application.Shared;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Persistence.Services
{
    public class CategoryService:ICategoryService
    {
        private ICategoryRepository _categoryRepository { get; }
      

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            
        }

        public async Task<BaseResponse<string>> AddAsync(CategoryCreateDto dto)
        {
            var categoryDb = await _categoryRepository.GetByFiltered(c => c.Name.Trim().ToLower() == dto.Name.Trim().ToLower()).FirstOrDefaultAsync();
            if (categoryDb is not null)
            {
                return new BaseResponse<string>("This category already exists", System.Net.HttpStatusCode.BadRequest);
            }
            Category category = new()
            {
                Name = dto.Name.Trim(),

            };
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangeAsync();
            return new BaseResponse<string>(System.Net.HttpStatusCode.Created);
        }

        public async Task<BaseResponse<string>> DeleteAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null)
            {
                return new BaseResponse<string>("Category not found", HttpStatusCode.NotFound);
            }
            _categoryRepository.Delete(category);
            await _categoryRepository.SaveChangeAsync();
            return new BaseResponse<string>("Category deleted successfully", HttpStatusCode.OK);
        }

        public async Task<BaseResponse<List<CategoryGetDto>>> GetAllAsync()
        {
            var categories = _categoryRepository.GetAll();
            if (categories is null)
            {
                return new BaseResponse<List<CategoryGetDto>>(HttpStatusCode.NotFound);
            }
            var dtoList = new List<CategoryGetDto>();
            foreach (var category in categories)
            {
                dtoList.Add(new CategoryGetDto
                {
                    Id = category.Id,
                    Name = category.Name
                });
            }
            return new BaseResponse<List<CategoryGetDto>>("Data", dtoList, HttpStatusCode.OK);


        }

        public async Task<BaseResponse<CategoryGetDto>> GetByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null)
            {
                return new BaseResponse<CategoryGetDto>(HttpStatusCode.NotFound);
            }
            var dto = new CategoryGetDto
            {
                Id = category.Id,
                Name = category.Name
            };
            return new BaseResponse<CategoryGetDto>("Data", dto, HttpStatusCode.OK);
        }

        public Task<BaseResponse<CategoryGetDto>> GetByNameAsync(string search)
        {
            var categories = _categoryRepository.GetAll();
            var dtoCategory = new CategoryGetDto();
            foreach (var category in categories)
            {
                if (category.Name == search)
                {
                    dtoCategory.Name = category.Name;
                    dtoCategory.Id = category.Id;
                }

            }
            if (dtoCategory is null)
            {
                return Task.FromResult(new BaseResponse<CategoryGetDto>(HttpStatusCode.NotFound));
            }
            return Task.FromResult(new BaseResponse<CategoryGetDto>("Data", dtoCategory, HttpStatusCode.OK));

        }

        public async Task<BaseResponse<CategoryUpdateDto>> UpdateAsync(CategoryUpdateDto dto)
        {
            var categoryDb = await _categoryRepository.GetByIdAsync(dto.Id);
            if (categoryDb is not null)
            {
                return new BaseResponse<CategoryUpdateDto>(HttpStatusCode.NotFound);
            }

            var existedCategory = await _categoryRepository
                .GetByFiltered(c => c.Name.Trim().ToLower() == dto.Name.Trim().ToLower())
                .FirstOrDefaultAsync();
            if (existedCategory is not null)
            {
                return new BaseResponse<CategoryUpdateDto>("This category already exists", HttpStatusCode.BadRequest);
            }
            categoryDb.Name = dto.Name.Trim();



            await _categoryRepository.SaveChangeAsync();
            return new BaseResponse<CategoryUpdateDto>("Category updated successfully", dto, HttpStatusCode.OK);
        }

        public async Task<BaseResponse<List<CategoryGetDto>>> GetByNameSearchAsync(string namePart)
        {
            var categories = await _categoryRepository.GetByNameSearchAsync(namePart);

            if (categories == null || !categories.Any())
            {
                return new BaseResponse<List<CategoryGetDto>>(
                    "No categories found with the given name part",
                    HttpStatusCode.NotFound
                );
            }

            // Manual mapping
            var categoryDtos = categories.Select(c => new CategoryGetDto
            {
                Id = c.Id,
                Name = c.Name,
               
                // Lazım olan bütün property-ləri burada doldur
            }).ToList();

            return new BaseResponse<List<CategoryGetDto>>("Data", categoryDtos, HttpStatusCode.OK);
        }

    }
}
