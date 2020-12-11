using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Reviews.Models;

namespace ThAmCo.Reviews.Services
{
    public class FakeReviewService : IReviewService
    {
        List<ReviewDto> _reviews;
        private readonly List<ReviewDto> reviews = new List<ReviewDto>
        {
            new ReviewDto {reviewId = 1, productId = 1, userId = 1, userName = "Dimitri 'Not-Russian-Bot' Ivanov", reviewRating = 5, reviewContent = "Great Product. You can believe me, I'm not a bot." },
            new ReviewDto {reviewId = 2, productId = 1, userId = 2, userName = "Joe Angry", reviewRating = 3, reviewContent = "It's an okay plunger. I expected more." },
            new ReviewDto {reviewId = 3, productId = 4, userId = 1, userName = "Dimitri 'Not-Russian-Bot' Ivanov", reviewRating = 4, reviewContent = "Good hardbass, although lacking the newest song from Dj Put-in" }
        };

        public FakeReviewService()
        {
            _reviews = reviews;
        }

        public FakeReviewService(List<ReviewDto> data)
        {
            _reviews = data;
        }

        

        public Task CreateReviewAsync(ReviewDto review)
        {
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

        public Task EditReviewAsync(ReviewDto review)
        {
            return Task.Run(() =>
            {
                _reviews.Remove(review);
                _reviews.Add(review);
            });
        }

        public Task<double> GetMeanRating(int productId)
        {
            List<ReviewDto> ratings = _reviews.FindAll(r => r.productId == productId);
            int ratingTotal = 0;

            foreach (ReviewDto review in ratings)
            {
                ratingTotal += review.reviewRating;
            }

            return Task.FromResult((double)ratingTotal / ratings.Count);
        }

        public Task<ReviewDto> GetReviewAsync(int reviewId)
        {
            var review = _reviews.FirstOrDefault(r => r.reviewId == reviewId);
            return Task.FromResult(review);
        }

        public Task<IEnumerable<ReviewDto>> GetReviewListAsync(int? productId, int? userId)
        {
            var reviews = _reviews.AsEnumerable();

            if (productId != null && userId != null)
            {
                reviews = reviews.Where(r => r.productId == productId && r.userId == userId);
            }
            if (productId != null)
            {
                reviews = reviews.Where(r => r.productId == productId);
            }
            if (userId != null)
            {
                reviews = reviews.Where(r => r.userId == userId);
            }
            return Task.FromResult(reviews);
        }
    }
}
