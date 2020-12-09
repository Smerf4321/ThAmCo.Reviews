using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ThAmCo.Reviews.Models;
using ThAmCo.Reviews.Services;

namespace ThAmCo.Reviews.Controllers
{
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // GET: api/Review/
        [HttpGet("api/Review")]
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
        [HttpGet("api/Review/{reviewId}")]
        public async Task<IActionResult> GetReviewAsync(int reviewId)
        {
            var reviewDto = await _reviewService.GetReviewAsync(reviewId);

            if (reviewDto == null)
            {
                return NotFound();
            }

            return Ok(reviewDto);
        }

        // POST: api/Review/Create
        [HttpPost("api/Review/Create")]
        public async Task<IActionResult> Create([Bind("reviewId,userId,productId,userName,reviewContent,reviewRating")] ReviewDto reviewDto)
        {
            if (ModelState.IsValid)
            {
                await _reviewService.CreateReviewAsync(reviewDto);
                return Ok();
            }
            return BadRequest();
        }

        // POST: api/Review/Edit/5
        [HttpPost("api/Review/Edit/")]
        public async Task<IActionResult> Edit([Bind("reviewId,userId,productId,userName,reviewContent,reviewRating")] ReviewDto reviewDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _reviewService.EditReviewAsync(reviewDto);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await ReviewDtoExists(reviewDto.reviewId)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok();
            }
            return BadRequest();
        }

        // POST: ReviewDtoes/Delete/5
        [HttpPost("api/Review/Delete/{reviewId}")]
        public async Task<IActionResult> DeleteConfirmed(int reviewId)
        {
            if (await ReviewDtoExists(reviewId))
            {
                await _reviewService.DeleteReviewAsync(reviewId);
                return Ok();
            }
            return NotFound();
        }

        private Task<bool> ReviewDtoExists(int reviewId)
        {
            return _reviewService.DoesReviewDtoExists(reviewId);
        }

        // GET: api/Review/Rating/5
        [HttpGet("api/Review/Rating/{prodcutId}")]
        public async Task<IActionResult> GetMeanRating(int productId)
        {
            return Ok( await _reviewService.GetMeanRating(productId));
        }
    }
}
