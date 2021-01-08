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

        // GET: api/ReviewList/
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

        // GET: api/HiddenReviewList/
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

        // GET: api/DeletedReviewList/
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

        // POST: Review/Delete/5
        [HttpPost("api/Review/Delete/{reviewId}")]
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

        // POST: Review/DeletePII/5
        [HttpPost("api/Review/DeletePII/{reviewId}")]
        public async Task<IActionResult> DeletePII(int userId)
        {
            //FIX ME
            //DO NOT LEAVE THIS IN THE CODE 
            //FIX WHEN WEBAPP IS FIXED
            var staffEmail = "";

            await _reviewService.DeleteReviewAsync(userId, staffEmail);
            return Ok();
        }

        // POST: Review/Hide/5
        [HttpPost("api/Review/Hide/{reviewId}")]
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
                    if (!(await ReviewExists(reviewDto.reviewId)))
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

        // POST: Review/RecoverHidden/5
        [HttpPost("api/Review/RecoverHidden/{reviewId}")]
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

        // POST: Review/RecoverDeleted/5
        [HttpPost("api/Review/RecoverDeleted/{reviewId}")]
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

        // GET: api/Review/Rating/5
        [HttpGet("api/Review/Rating/{prodcutId}")]
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