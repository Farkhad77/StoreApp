using StoreApp.Application.Abstracts.Services;
using StoreApp.Application.DTOs.ReviewDtos;
using StoreApp.Application.Shared;
using StoreApp.Domain.Entities;
using StoreApp.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Persistence.Services
{
    public class ReviewService : IReviewService
    {
        private readonly StoreAppDbContext _context;

        public ReviewService(StoreAppDbContext context)
        {
            _context = context;
        }

        public async Task<Review> CreateReviewAsync(ReviewCreateDto dto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("UserId cannot be null or empty.");

            var review = new Review
            {
                ProductId = dto.ProductId,
                Content = dto.Content,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return review;
        }
        public async Task<Review?> GetReviewByIdAsync(Guid id)
        {
            return await _context.Reviews.FindAsync(id);
        }
        public async Task<BaseResponse<string>> DeleteReviewAsync(Guid reviewId, string userId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null)
                return new BaseResponse<string>("Review tapılmadı", HttpStatusCode.NotFound);

            if (review.UserId != userId)
                return new BaseResponse<string>("Başqasının review-nu silmək olmaz", HttpStatusCode.Forbidden);

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return new BaseResponse<string>("Review uğurla silindi", true, HttpStatusCode.NoContent);
        }

    }

}
