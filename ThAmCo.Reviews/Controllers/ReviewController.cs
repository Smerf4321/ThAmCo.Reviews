using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ThAmCo.Reviews.Models;
using ThAmCo.Reviews.Services;

namespace ThAmCo.Reviews.Controllers
{
    [ApiController]
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
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

        // GET: api/ReviewList
        [HttpGet("api/ReviewList")]
        public async Task<IActionResult> GetReviewListAsync(int? productId, int? userId)
        {
            IEnumerable<ReviewDto> reviews;
            try
            {
                reviews = await _reviewService.GetReviewListAsync(productId, userId, false, false);
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

        // GET: api/HiddenReviewList
        [HttpGet("api/HiddenReviewList")]
        public async Task<IActionResult> GetHiddenReviewListAsync(int? productId, int? userId)
        {
            IEnumerable<ReviewDto> reviews;
            try
            {
                reviews = await _reviewService.GetReviewListAsync(productId, userId, true, false);
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

        // GET: api/DeletedReviewList
        [HttpGet("api/DeletedReviewList")]
        public async Task<IActionResult> GetDeletedReviewListAsync(int? productId, int? userId)
        {
            IEnumerable<ReviewDto> reviews;
            try
            {
                reviews = await _reviewService.GetReviewListAsync(productId, userId, false, true);
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

        // POST: api/CreateReview
        [HttpPost("api/CreateReview")]
        public async Task<IActionResult> Create(
            int reviewId,
            [FromForm(Name = "Review.userId")] int userId,
            [FromForm(Name = "Review.productId")] int productId,
            [FromForm(Name = "Review.userName")] string userName,
            [FromForm(Name = "Review.reviewContent")] string reviewContent,
            [FromForm(Name = "Review.reviewRating")] int reviewRating)

        {
            if (ModelState.IsValid)
            {
                await _reviewService.CreateReviewAsync(userId, productId, userName, reviewContent, reviewRating);
                return Ok();
            }
            return BadRequest();
        }

        // POST: api/DeleteReview/
        [HttpPost("api/DeleteReview/{reviewId}")]
        public async Task<IActionResult> Delete(int reviewId)
        {
            //FIX ME
            //DO NOT LEAVE THIS IN THE CODE 
            //FIX WHEN WEBAPP IS FIXED
            var staffEmail = "";

            if (await ReviewExists(reviewId))
            {
                await _reviewService.DeleteReviewAsync(reviewId, staffEmail);
                return Ok();
            }
            return NotFound();
        }

        // POST: api/DeleteReviewPII/5
        [HttpPost("api/DeleteReviewPII/{userId}")]
        public async Task<IActionResult> DeletePII(int userId)
        {
            //FIX ME
            //DO NOT LEAVE THIS IN THE CODE 
            //FIX WHEN WEBAPP IS FIXED
            var staffEmail = "";

            await _reviewService.DeleteReviewPIIAsync(userId, staffEmail);
            return Ok();
        }

        // POST: api/DeleteReviewByProduct/5
        [HttpPost("api/DeleteReviewByProduct/{productId}")]
        public async Task<IActionResult> DeleteByProduct(int productId)
        {
            //FIX ME
            //DO NOT LEAVE THIS IN THE CODE 
            //FIX WHEN WEBAPP IS FIXED
            var staffEmail = "";

            await _reviewService.DeleteReviewByProductAsync(productId, staffEmail);
            return Ok();
        }

        // POST: api/HideReview/5
        [HttpPost("api/HideReview/{reviewId}")]
        public async Task<IActionResult> Hide(int reviewId)
        {
            //FIX ME
            //DO NOT LEAVE THIS IN THE CODE 
            //FIX WHEN WEBAPP IS FIXED
            var staffEmail = "";

            if (await ReviewExists(reviewId))
            {
                await _reviewService.HideReviewAsync(reviewId, staffEmail);
                return Ok();
            }
            return NotFound();
        }

        // POST: api/EditReview/
        [HttpPost("api/EditReview")]
        public async Task<IActionResult> Edit(
            [FromForm(Name = "Review.reviewId")]int reviewId,
            [FromForm(Name = "Review.reviewContent")] string reviewContent,
            [FromForm(Name = "Review.reviewRating")] int reviewRating)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _reviewService.EditReviewAsync(reviewId, reviewContent, reviewRating);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await ReviewExists(reviewId)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                return Ok();
            }
            return BadRequest();
        }

        // POST: api/RecoverHiddenReview/5
        [HttpPost("api/RecoverHiddenReview/{reviewId}")]
        public async Task<IActionResult> RecoverHidden(int reviewId)
        {
            //FIX ME
            //DO NOT LEAVE THIS IN THE CODE 
            //FIX WHEN WEBAPP IS FIXED
            var staffEmail = "";

            if (await ReviewExists(reviewId))
            {
                await _reviewService.RecoverHiddenReviewAsync(reviewId, staffEmail);
                return Ok();
            }
            return NotFound();
        }

        // POST: api/RecoverDeletedReview/5
        [HttpPost("api/RecoverDeletedReview/{reviewId}")]
        public async Task<IActionResult> RecoverDeleted(int reviewId)
        {
            //FIX ME
            //DO NOT LEAVE THIS IN THE CODE 
            //FIX WHEN WEBAPP IS FIXED
            var staffEmail = "";

            if (await ReviewExists(reviewId))
            {
                await _reviewService.RecoverDeletedReviewAsync(reviewId, staffEmail);
                return Ok();
            }
            return NotFound();
        }

        // GET: api/ReviewRating/5
        [HttpGet("api/ReviewRating/{prodcutId}")]
        public async Task<IActionResult> GetMeanRating(int productId)
        {
            return Ok( await _reviewService.GetMeanRating(productId));
        }

        private Task<bool> ReviewExists(int reviewId)
        {
            return _reviewService.DoesReviewExists(reviewId);
        }
    }
}