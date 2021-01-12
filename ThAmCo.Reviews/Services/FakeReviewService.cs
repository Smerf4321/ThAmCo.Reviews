using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Reviews.Models;

namespace ThAmCo.Reviews.Services
{
    public class FakeReviewService : IReviewService
    {
        public List<Review> _reviews;
        public readonly List<Review> reviews = new List<Review>
        {
            new Review {reviewId = 1, productId = 1, userId = 1, userName = "Dimitri 'Not-Russian-Bot' Ivanov", reviewRating = 5, reviewContent = "Great Product. You can believe me, I'm not a bot.", hidden = false, deleted = false, dateCreated = DateTime.UtcNow, lastUpdated = DateTime.UtcNow, lastUpdatedStaffEmail = "fakeuser@fake.user" },
            new Review {reviewId = 2, productId = 1, userId = 2, userName = "Joe Angry", reviewRating = 3, reviewContent = "It's an okay plunger. I expected more.", hidden = false, deleted = false, dateCreated = DateTime.UtcNow, lastUpdated = DateTime.UtcNow, lastUpdatedStaffEmail = "fakeuser@fake.user" },
            new Review {reviewId = 3, productId = 4, userId = 1, userName = "Dimitri 'Not-Russian-Bot' Ivanov", reviewRating = 4, reviewContent = "Good hardbass, although lacking the newest song from Dj Put-in", hidden = false, deleted = false, dateCreated = DateTime.UtcNow, lastUpdated = DateTime.UtcNow, lastUpdatedStaffEmail = "fakeuser@fake.user" },
            new Review {reviewId = 4, productId = 2, userId = 3, userName = "Bob", reviewRating = 5, reviewContent = "Great quality", hidden = false, deleted = false, dateCreated = DateTime.UtcNow, lastUpdated = DateTime.UtcNow, lastUpdatedStaffEmail = "fakeuser@fake.user" },
            new Review {reviewId = 5, productId = 5, userId = 4, userName = "Adam", reviewRating = 1, reviewContent = "Just no", hidden = true, deleted = true, dateCreated = DateTime.UtcNow, lastUpdated = DateTime.UtcNow, lastUpdatedStaffEmail = "fakeuser@fake.user" }
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
            var review = _reviews.Find(r => r.reviewId == reviewId && !r.deleted);

            if (!DoesReviewExists(reviewId).Result)
            {
                return Task.FromResult<ReviewDto>(null);
            }

            var reviewDto = new ReviewDto
                {
                    reviewId = review.reviewId,
                    productId = review.productId,
                    userId = review.userId,
                    userName = review.userName,
                    reviewRating = review.reviewRating,
                    reviewContent = review.reviewContent,
                    dateCreated = review.dateCreated,
                    hidden = review.hidden
            };
            return Task.FromResult(reviewDto);
        }

        public Task<IEnumerable<ReviewDto>> GetReviewListAsync(int? productId, int? userId, Boolean hidden, Boolean deleted)
        {
            var reviews = _reviews.AsEnumerable().Where(r => r.hidden == hidden && r.deleted == deleted);

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
                    reviewContent = r.reviewContent,
                    dateCreated = r.dateCreated,
                    hidden = r.hidden
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
                    reviewContent = r.reviewContent,
                        dateCreated = r.dateCreated,
                        hidden = r.hidden
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
                    reviewContent = r.reviewContent,
                        dateCreated = r.dateCreated,
                        hidden = r.hidden
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
                        reviewContent = r.reviewContent,
                        dateCreated = r.dateCreated,
                        hidden = r.hidden
                    });
            }

            return Task.FromResult(reviewsList);
        }

        
        public Task CreateReviewAsync(int userId, int productId, string userName, string reviewContent, int reviewRating)
        {
            var review = new Review
            {
                reviewId = reviews.Count + 1,
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

            return Task.Run(() =>
            {
                _reviews.Add(review);
            });
        }

        public Task DeleteReviewAsync(int reviewId, string staffEmail)
        {
            Review review = _reviews.Find(r => r.reviewId == reviewId);

            if (!DoesReviewExists(reviewId).Result)
            {
                return Task.FromResult<ReviewDto>(null);
            }

            return Task.Run(() =>
            {
                review.deleted = true;
                review.lastUpdated = DateTime.UtcNow;
                review.lastUpdatedStaffEmail = staffEmail;
            });
        }

        public Task DeleteReviewPIIAsync(int userId, string staffEmail)
        { 
            return Task.Run(() => {
                for (int i = 0; i <= _reviews.Count-1; i++)
                {
                    if (_reviews[i].userId == userId)
                    {
                        _reviews[i].userName = "Account Deleted";
                        _reviews[i].deleted = true;
                        _reviews[i].lastUpdated = DateTime.UtcNow;
                        _reviews[i].lastUpdatedStaffEmail = staffEmail;
                    }
                }
            });
        }

        public Task DeleteReviewByProductAsync(int productId, string staffEmail)
        {
            return Task.Run(() => {
                for (int i = 0; i <= _reviews.Count - 1; i++)
                {
                    if (_reviews[i].productId == productId)
                    {
                        _reviews[i].deleted = true;
                        _reviews[i].lastUpdated = DateTime.UtcNow;
                        _reviews[i].lastUpdatedStaffEmail = staffEmail;
                    }
                }
            });
        }

        public Task HideReviewAsync(int reviewId, string staffEmail)
        {
            Review review = _reviews.Find(r => r.reviewId == reviewId);

            if (!DoesReviewExists(reviewId).Result)
            {
                return Task.Run(() => { });
            }

            return Task.Run(() =>
            {
                review.hidden = true;
                review.lastUpdated = DateTime.UtcNow;
                review.lastUpdatedStaffEmail = staffEmail;
            });
        }

        public Task EditReviewAsync(int reviewId, string reviewContent, int reviewRating)
        {
            var review = _reviews.Find(r => r.reviewId == reviewId);

            if (!DoesReviewExists(reviewId).Result)
            {
                return Task.Run(() => { });
            }

            return Task.Run(() =>
            {
                _reviews.Remove(review);
                review.reviewRating = reviewRating;
                review.reviewContent = reviewContent;
                review.lastUpdated = DateTime.UtcNow;
                _reviews.Add(review);
            });
        }

        public Task RecoverDeletedReviewAsync(int reviewId, string staffEmail)
        {
            if (!DoesReviewExists(reviewId).Result)
            {
                return Task.Run(() => { });
            }

            return Task.Run(() =>
            {
                for (int i = 0; i <= _reviews.Count - 1; i++)
                {
                    if (_reviews[i].reviewId == reviewId)
                    {
                        _reviews[i].deleted = false;
                        _reviews[i].lastUpdated = DateTime.UtcNow;
                        _reviews[i].lastUpdatedStaffEmail = staffEmail;
                    }
                }  
            });
        }

        public Task RecoverHiddenReviewAsync(int reviewId, string staffEmail)
        {
            if (!DoesReviewExists(reviewId).Result)
            {
                return Task.Run(() => { });
            }

            return Task.Run(() =>
            {
                for (int i = 0; i <= _reviews.Count - 1; i++)
                {
                    if (_reviews[i].reviewId == reviewId)
                    {
                        _reviews[i].hidden = false;
                        _reviews[i].lastUpdated = DateTime.UtcNow;
                        _reviews[i].lastUpdatedStaffEmail = staffEmail;
                    }
                }
            });
        }

        public Task<double> GetMeanRating(int productId)
        {
            List<Review> ratings = _reviews.FindAll(r => r.productId == productId);
            double ratingTotal = 0;

            if (ratings.Count == 0)
            {
                return Task.FromResult(ratingTotal);
            }

            foreach (Review review in ratings)
            {
                ratingTotal += review.reviewRating;
            }

            return Task.FromResult((double)ratingTotal / ratings.Count);
        }

        public Task<bool> DoesReviewExists(int reviewId)
        {
            return Task.FromResult(_reviews.Exists(r => r.reviewId == reviewId));
        }
    }
}
