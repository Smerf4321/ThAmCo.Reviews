using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Reviews.Models;

namespace ThAmCo.Reviews.Services
{
    public class FakeReviewService : IReviewService
    {
        List<Review> _reviews;
        private readonly List<Review> reviews = new List<Review>
        {
            new Review {reviewId = 1, productId = 1, userId = 1, userName = "Dimitri 'Not-Russian-Bot' Ivanov", reviewRating = 5, reviewContent = "Great Product. You can believe me, I'm not a bot.", hidden = false, deleted = false, dateCreated = DateTime.UtcNow, lastUpdated = DateTime.UtcNow, lastUpdatedStaffEmail = "fakestaff@fake.staff" },
            new Review {reviewId = 2, productId = 1, userId = 2, userName = "Joe Angry", reviewRating = 3, reviewContent = "It's an okay plunger. I expected more.", hidden = false, deleted = false, dateCreated = DateTime.UtcNow, lastUpdated = DateTime.UtcNow, lastUpdatedStaffEmail = "fakestaff@fake.staff" },
            new Review {reviewId = 3, productId = 4, userId = 1, userName = "Dimitri 'Not-Russian-Bot' Ivanov", reviewRating = 4, reviewContent = "Good hardbass, although lacking the newest song from Dj Put-in", hidden = false, deleted = false, dateCreated = DateTime.UtcNow, lastUpdated = DateTime.UtcNow, lastUpdatedStaffEmail = "fakestaff@fake.staff" }
        };

        public FakeReviewService()
        {
            _reviews = reviews;
        }

        public FakeReviewService(List<Review> data)
        {
            _reviews = data;
        }

        public Task<ReviewDto> GetReviewAsync(int reviewId)
        {
            var review = _reviews.Find(r => r.reviewId == reviewId && !r.hidden && !r.deleted);
            var reviewDto = new ReviewDto
                {
                    reviewId = review.reviewId,
                    productId = review.productId,
                    userId = review.userId,
                    userName = review.userName,
                    reviewRating = review.reviewRating,
                    reviewContent = review.reviewContent
                };
            return Task.FromResult(reviewDto);
        }

        public Task<IEnumerable<ReviewDto>> GetReviewListAsync(int? productId, int? userId)
        {
            var reviews = _reviews.AsEnumerable();
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

            return Task.FromResult(reviewsList);
        }

        public Task CreateReviewAsync(ReviewDto reviewDto)
        {
            var review = new Review
            {
                productId = reviewDto.productId,
                userId = reviewDto.userId,
                userName = reviewDto.userName,
                reviewRating = reviewDto.reviewRating,
                reviewContent = reviewDto.reviewContent,
                hidden = false,
                deleted = false,
                dateCreated = DateTime.UtcNow,
                lastUpdated = DateTime.UtcNow,
                lastUpdatedStaffEmail = null
            };

            return Task.Run(() =>
            {
                _reviews.Add(review);
            });
        }

        public Task DeleteReviewAsync(int reviewId)
        {
            return Task.Run(() =>
            {
                _reviews.RemoveAll(r => r.reviewId == reviewId);
            });
        }

        public Task<bool> DoesReviewDtoExists(int reviewId)
        {
            return Task.FromResult(_reviews.Exists(r => r.reviewId == reviewId));
        }

        public Task EditReviewAsync(ReviewDto reviewDto)
        {
            var review = _reviews.Find(r => r.reviewId == reviewDto.reviewId);

            return Task.Run(() =>
            {
                _reviews.Remove(review);
                _reviews.Add(review);
            });
        }

        public Task<double> GetMeanRating(int productId)
        {
            List<ReviewDto> ratings = _reviews.FindAll(r => r.productId == productId);
            double ratingTotal = 0;

            if (ratings.)
            {
                return Task.FromResult(ratingTotal);
            }

            foreach (ReviewDto review in ratings)
            {
                ratingTotal += review.reviewRating;
            }

            return Task.FromResult((double)ratingTotal / ratings.Count);
        }
    }
}
