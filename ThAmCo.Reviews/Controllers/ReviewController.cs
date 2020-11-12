using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ThAmCo.Reviews.Data;
using ThAmCo.Reviews.Models;
using ThAmCo.Reviews.Services;

namespace ThAmCo.Reviews.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // GET: api/Review/
        [HttpGet("api/Review/")]
        public async Task<IActionResult> GetReviewListAsync(int? productId, int? userId)
        {
            IEnumerable<ReviewDto> reviews = null;

            try
            {
                reviews = await _reviewService.GetReviewListAsync(productId, userId);
            }
            catch (HttpRequestException)
            {
                reviews = Array.Empty<ReviewDto>();
            }

            if (reviews == null)
            {
                return NotFound();
            }

            return Ok(reviews);
        }

        // GET: api/Review/5
        [HttpGet("api/Review/5")]
        public async Task<IActionResult> GetReviewAsync(int reviewId)
        {
            var reviewDto = await _reviewService.GetReviewAsync(reviewId);

            if (reviewDto == null)
            {
                return NotFound();
            }

            return View(reviewDto);
        }
    }
}
