using StoreApp.Application.DTOs.ReviewDtos;
using StoreApp.Application.Shared;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Abstracts.Services
{
    public interface IReviewService
    {
        Task<Review> CreateReviewAsync(ReviewCreateDto dto, string userId);
        Task<Review?> GetReviewByIdAsync(Guid id);
        Task<BaseResponse<string>> DeleteReviewAsync(Guid reviewId, string userId);
    }
}
