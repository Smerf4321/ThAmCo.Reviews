using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Reviews.Data;
using ThAmCo.Reviews.Models;

namespace ThAmCo.Reviews.Services
{
    public class ReviewService : IReviewService
    {
        private ThAmCoReviewsContext _reviews;

        public ReviewService (ThAmCoReviewsContext context)
        {
            _reviews = context;
        }

        public async Task<ReviewDto> GetReviewAsync(int reviewId)
        {
            var review = await _reviews.Review.FirstOrDefaultAsync(r => r.reviewId == reviewId && !r.hidden && !r.deleted);
            var reviewDto = new ReviewDto
            {
                reviewId = review.reviewId,
                productId = review.productId,
                userId = review.userId,
                userName = review.userName,
                reviewRating = review.reviewRating,
                reviewContent = review.reviewContent
            };
            return reviewDto;
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewListAsync(int? productId, int? userId, Boolean hidden, Boolean deleted)
        {
            var reviews = await _reviews.Review.Where(r => r.hidden == false && r.deleted == false).ToListAsync();

            if (hidden)
            {
                reviews = await _reviews.Review.Where(r => r.hidden == true).ToListAsync();
            }
            else if (deleted)
            {
                reviews = await _reviews.Review.Where(r => r.deleted == true).ToListAsync();
            }

            IEnumerable<ReviewDto> reviewsList;

            if (productId != null && userId != null)
            {
                reviewsList = reviews
                    .Where(r => r.productId == productId && r.userId == userId)
                    .Select(r => new ReviewDto
                    {
                        reviewId = r.reviewId,
                        productId = r.productId,
                        userId = r.userId,
                        userName = r.userName,
                        reviewRating = r.reviewRating,
                        reviewContent = r.reviewContent
                    });
            }
            else if (productId != null)
            {
                reviewsList = reviews
                    .Where(r => r.productId == productId)
                    .Select(r => new ReviewDto
                    {
                        reviewId = r.reviewId,
                        productId = r.productId,
                        userId = r.userId,
                        userName = r.userName,
                        reviewRating = r.reviewRating,
                        reviewContent = r.reviewContent
                    });
            }
            else if (userId != null)
            {
                reviewsList = reviews
                    .Where(r => r.userId == userId)
                    .Select(r => new ReviewDto
                    {
                        reviewId = r.reviewId,
                        productId = r.productId,
                        userId = r.userId,
                        userName = r.userName,
                        reviewRating = r.reviewRating,
                        reviewContent = r.reviewContent
                    });
            }
            else
            {
                reviewsList = reviews
                    .Select(r => new ReviewDto
                    {
                        reviewId = r.reviewId,
                        productId = r.productId,
                        userId = r.userId,
                        userName = r.userName,
                        reviewRating = r.reviewRating,
                        reviewContent = r.reviewContent
                    });
            }

            return reviewsList;
        }

        public async Task CreateReviewAsync(int userId, int productId, string userName, string reviewContent, int reviewRating)
        {
            var review = new Review
            {
                productId = productId,
                userId = userId,
                userName = userName,
                reviewRating = reviewRating,
                reviewContent = reviewContent,
                hidden = false,
                deleted = false,
                dateCreated = DateTime.UtcNow,
                lastUpdated = DateTime.UtcNow,
                lastUpdatedStaffEmail = null
            };

            _reviews.Add(review);
            await _reviews.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(int reviewId, string staffEmail)
        {
            Review review = await _reviews.Review.FirstOrDefaultAsync(r => r.reviewId == reviewId);

            review.deleted = true;
            review.lastUpdated = DateTime.UtcNow;
            review.lastUpdatedStaffEmail = staffEmail;

            _reviews.Update(review);
            await _reviews.SaveChangesAsync();
        }

        public async Task DeleteReviewPIIAsync(int userId, string staffEmail)
        {
            List<Review> reviews = await _reviews.Review.Where(r => r.userId == userId).ToListAsync();

            foreach (Review review in reviews)
            {
                review.userName = "Account Deleted";
                review.deleted = true;
                review.lastUpdated = DateTime.UtcNow;
                review.lastUpdatedStaffEmail = staffEmail;

                _reviews.Update(review);
            }

            await _reviews.SaveChangesAsync();
        }

        public async Task DeleteReviewByProductAsync(int productId,  string staffEmail)
        {
            List<Review> reviews = await _reviews.Review.Where(r => r.productId == productId).ToListAsync();

            foreach (Review review in reviews)
            {
                review.deleted = true;
                review.lastUpdated = DateTime.UtcNow;
                review.lastUpdatedStaffEmail = staffEmail;

                _reviews.Update(review);
            }

            await _reviews.SaveChangesAsync();
        }

        public async Task HideReviewAsync(int reviewId, string staffEmail)
        {
            var review = await _reviews.Review.FirstOrDefaultAsync(r => r.reviewId == reviewId);

            review.hidden = true;
            review.lastUpdated = DateTime.UtcNow;
            review.lastUpdatedStaffEmail = staffEmail;

            _reviews.Update(review);
            await _reviews.SaveChangesAsync();
        }

        public async Task EditReviewAsync(int reviewId, string reviewContent, int reviewRating)
        {
            var review = await _reviews.Review.FirstOrDefaultAsync(r => r.reviewId == reviewId);

            review.reviewRating = reviewRating;
            review.reviewContent = reviewContent;
            review.lastUpdated = DateTime.UtcNow;

            _reviews.Update(review);
            await _reviews.SaveChangesAsync();
        }

        public async Task RecoverDeletedReviewAsync(int reviewId, string staffEmail)
        {
            Review review = await _reviews.Review.FirstOrDefaultAsync(r => r.reviewId == reviewId);

            review.deleted = false;
            review.lastUpdated = DateTime.UtcNow;
            review.lastUpdatedStaffEmail = staffEmail;

            _reviews.Update(review);
            await _reviews.SaveChangesAsync();
        }

        public async Task RecoverHiddenReviewAsync(int reviewId, string staffEmail)
        {
            Review review = await _reviews.Review.FirstOrDefaultAsync(r => r.reviewId == reviewId);

            review.hidden = false;
            review.lastUpdated = DateTime.UtcNow;
            review.lastUpdatedStaffEmail = staffEmail;

            _reviews.Update(review);
            await _reviews.SaveChangesAsync();
        }

        public async Task<double> GetMeanRating(int productId)
        {
            List<Review> ratings = await _reviews.Review.Where(r => r.productId == productId).ToListAsync();
            double ratingTotal = 0;

            foreach (Review review in ratings)
            {
                ratingTotal += review.reviewRating;
            }

            return ((double)ratingTotal / ratings.Count);
        }

        public async Task<bool> DoesReviewExists(int reviewId)
        {
            return await _reviews.Review.AnyAsync(r => r.reviewId == reviewId);
        }
    }
}
