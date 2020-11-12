﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Reviews.Models;

namespace ThAmCo.Reviews.Services
{
    public class FakeReviewService : IReviewService
    {
        private readonly ReviewDto[] _reviews =
        {
            new ReviewDto {reviewId = 1, productId = 1, userId = 1, userName = "Dimitri 'Not-Russian-Bot' Ivanov", reviewRating = 5, reviewContent = "Great Product. You can believe me, I'm not a bot." },
            new ReviewDto {reviewId = 2, productId = 1, userId = 2, userName = "Joe Angry", reviewRating = 3, reviewContent = "It's an okay plunger. I expected more." },
            new ReviewDto {reviewId = 3, productId = 4, userId = 1, userName = "Dimitri 'Not-Russian-Bot' Ivanov", reviewRating = 4, reviewContent = "Good hardbass, although lacking the newest song from Dj Put-in" }
        };

        public Task<ReviewDto> GetReviewAsync(int reviewId)
        {
            var review = _reviews.FirstOrDefault(r => r.reviewId == reviewId);
            return Task.FromResult(review);
        }

        public Task<IEnumerable<ReviewDto>> GetReviewListAsync(int? productId, int? userId)
        {
            var reviews = _reviews.AsEnumerable();

            if (productId == null)
            {
                reviews = reviews.Where(r => r.userId == userId);
                return Task.FromResult(reviews);
            }
            if (userId == null)
            {
                reviews = reviews.Where(r => r.productId == productId);
                return Task.FromResult(reviews);
            }

            reviews = reviews.Where(r => r.productId == productId && r.userId == userId);
            return Task.FromResult(reviews);
        }
    }
}
