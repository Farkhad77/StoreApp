using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Abstracts.Services;
using StoreApp.Application.DTOs.ReviewDtos;
using StoreApp.Application.Shared;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : BaseController
    {
        // GET: api/<ReviewsController>
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReview([FromBody] ReviewCreateDto dto)
        {
            var userId = GetUserIdFromToken();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            try
            {
                var review = await _reviewService.CreateReviewAsync(dto, userId);
                return CreatedAtAction(nameof(GetReviewById), new { id = review.Id }, review);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // Örnek GetReviewById metodu (varsa)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById(Guid id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);

            if (review == null)
                return NotFound(new { success = false, message = "Review not found" });

            return Ok(new { success = true, data = review });
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
            var userId = GetUserIdFromToken();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new BaseResponse<string>("İstifadəçi identifikatoru tapılmadı", HttpStatusCode.Unauthorized));

            var response = await _reviewService.DeleteReviewAsync(id, userId);

            return StatusCode((int)response.StatusCode, response);
        }

    }
}
