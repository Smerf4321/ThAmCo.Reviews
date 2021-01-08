using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Reviews.Controllers;
using ThAmCo.Reviews.Models;
using ThAmCo.Reviews.Services;

namespace ReviewTests.Services
{
    [TestClass]
    public class FakeReviewServicesTest
    {
        private readonly List<Review> _reviews = new List<Review>
        {
            new Review {reviewId = 1, productId = 1, userId = 1, userName = "Dimitri 'Not-Russian-Bot' Ivanov", reviewRating = 5, reviewContent = "Great Product. You can believe me, I'm not a bot.", hidden = false, deleted = false, dateCreated = DateTime.UtcNow, lastUpdated = DateTime.UtcNow, lastUpdatedStaffEmail = "fakeuser@fake.user" },
            new Review {reviewId = 2, productId = 1, userId = 2, userName = "Joe Angry", reviewRating = 3, reviewContent = "It's an okay plunger. I expected more.", hidden = false, deleted = false, dateCreated = DateTime.UtcNow, lastUpdated = DateTime.UtcNow, lastUpdatedStaffEmail = "fakeuser@fake.user" },
            new Review {reviewId = 3, productId = 4, userId = 1, userName = "Dimitri 'Not-Russian-Bot' Ivanov", reviewRating = 4, reviewContent = "Good hardbass, although lacking the newest song from Dj Put-in", hidden = false, deleted = false, dateCreated = DateTime.UtcNow, lastUpdated = DateTime.UtcNow, lastUpdatedStaffEmail = "fakeuser@fake.user" },
            new Review {reviewId = 4, productId = 2, userId = 3, userName = "Bob", reviewRating = 5, reviewContent = "Great quality", hidden = false, deleted = false, dateCreated = DateTime.UtcNow, lastUpdated = DateTime.UtcNow, lastUpdatedStaffEmail = "fakeuser@fake.user" }
        };

        [TestMethod]
        public async Task GetAllReviews_ShouldTask()
        {
            var service = new FakeReviewService(_reviews);
            var result = await service.GetReviewListAsync(null, null, false, false);

            Assert.IsNotNull(result);
            var reviewsResult = result as IEnumerable<Review>;
            Assert.IsNotNull(reviewsResult);
            var resultList = reviewsResult.ToList();
            Assert.AreEqual(_reviews.Count, resultList.Count);

            for (int i = 0; i < _reviews.Count; ++i)
            {
                Assert.AreEqual(_reviews[i].reviewId, resultList[i].reviewId);
                Assert.AreEqual(_reviews[i].productId, resultList[i].productId);
                Assert.AreEqual(_reviews[i].userId, resultList[i].userId);
                Assert.AreEqual(_reviews[i].userName, resultList[i].userName);
                Assert.AreEqual(_reviews[i].reviewRating, resultList[i].reviewRating);
                Assert.AreEqual(_reviews[i].hidden, resultList[i].hidden);
                Assert.AreEqual(_reviews[i].deleted, resultList[i].deleted);
                Assert.AreEqual(_reviews[i].dateCreated, resultList[i].dateCreated);
                Assert.AreEqual(_reviews[i].lastUpdated, resultList[i].lastUpdated);
                Assert.AreEqual(_reviews[i].lastUpdatedStaffEmail, resultList[i].lastUpdatedStaffEmail);
            }
        }

        [TestMethod]
        public async Task GetAllReviewsForProductId_ShouldTask()
        {
            var service = new FakeReviewService(_reviews);
            var result = await service.GetReviewListAsync(1, null, false, false);

            Assert.IsNotNull(result);
            var reviewsResult = result as IEnumerable<Review>;
            Assert.IsNotNull(reviewsResult);
            Assert.IsTrue(reviewsResult.All(r => r.productId == 1));
            var reviewsResultList = reviewsResult.ToList();
            var limitedReviews = _reviews.FindAll(r => r.productId == 1);
            Assert.AreEqual(limitedReviews.Count, reviewsResultList.Count);

            for (int i = 0; i < limitedReviews.Count; ++i)
            {
                Assert.AreEqual(limitedReviews[i].reviewId, reviewsResultList[i].reviewId);
                Assert.AreEqual(limitedReviews[i].productId, reviewsResultList[i].productId);
                Assert.AreEqual(limitedReviews[i].userId, reviewsResultList[i].userId);
                Assert.AreEqual(limitedReviews[i].userName, reviewsResultList[i].userName);
                Assert.AreEqual(limitedReviews[i].reviewRating, reviewsResultList[i].reviewRating);
                Assert.AreEqual(limitedReviews[i].hidden, reviewsResultList[i].hidden);
                Assert.AreEqual(limitedReviews[i].deleted, reviewsResultList[i].deleted);
                Assert.AreEqual(limitedReviews[i].dateCreated, reviewsResultList[i].dateCreated);
                Assert.AreEqual(limitedReviews[i].lastUpdated, reviewsResultList[i].lastUpdated);
                Assert.AreEqual(limitedReviews[i].lastUpdatedStaffEmail, reviewsResultList[i].lastUpdatedStaffEmail);
            }
        }

        [TestMethod]
        public async Task GetAllReviewsForUserId_ShouldTask()
        {
            var service = new FakeReviewService(_reviews);
            var result = await service.GetReviewListAsync(null, 1, false, false);

            Assert.IsNotNull(result);
            var reviewsResult = result as IEnumerable<Review>;
            Assert.IsNotNull(reviewsResult);
            Assert.IsTrue(reviewsResult.All(r => r.userId == 1));
            var reviewsResultList = reviewsResult.ToList();
            var limitedReviews = _reviews.FindAll(r => r.userId == 1);
            Assert.AreEqual(limitedReviews.Count, reviewsResultList.Count);

            for (int i = 0; i < limitedReviews.Count; ++i)
            {
                Assert.AreEqual(limitedReviews[i].reviewId, reviewsResultList[i].reviewId);
                Assert.AreEqual(limitedReviews[i].productId, reviewsResultList[i].productId);
                Assert.AreEqual(limitedReviews[i].userId, reviewsResultList[i].userId);
                Assert.AreEqual(limitedReviews[i].userName, reviewsResultList[i].userName);
                Assert.AreEqual(limitedReviews[i].reviewRating, reviewsResultList[i].reviewRating);
                Assert.AreEqual(limitedReviews[i].hidden, reviewsResultList[i].hidden);
                Assert.AreEqual(limitedReviews[i].deleted, reviewsResultList[i].deleted);
                Assert.AreEqual(limitedReviews[i].dateCreated, reviewsResultList[i].dateCreated);
                Assert.AreEqual(limitedReviews[i].lastUpdated, reviewsResultList[i].lastUpdated);
                Assert.AreEqual(limitedReviews[i].lastUpdatedStaffEmail, reviewsResultList[i].lastUpdatedStaffEmail);
            }
        }

        [TestMethod]
        public async Task GetAllReviewsForProductIdandUserId_ShouldTask()
        {
            var service = new FakeReviewService(_reviews);
            var result = await service.GetReviewListAsync(1, 1, false, false);

            Assert.IsNotNull(result);
            var reviewsResult = result as IEnumerable<Review>;
            Assert.IsNotNull(reviewsResult);
            Assert.IsTrue(reviewsResult.All(r => r.userId == 1 && r.productId == 1));
            var reviewsResultList = reviewsResult.ToList();
            var limitedReviews = _reviews.FindAll(r => r.userId == 1 && r.productId == 1);
            Assert.AreEqual(limitedReviews.Count, reviewsResultList.Count);

            for (int i = 0; i < limitedReviews.Count; ++i)
            {
                Assert.AreEqual(limitedReviews[i].reviewId, reviewsResultList[i].reviewId);
                Assert.AreEqual(limitedReviews[i].productId, reviewsResultList[i].productId);
                Assert.AreEqual(limitedReviews[i].userId, reviewsResultList[i].userId);
                Assert.AreEqual(limitedReviews[i].userName, reviewsResultList[i].userName);
                Assert.AreEqual(limitedReviews[i].reviewRating, reviewsResultList[i].reviewRating);
                Assert.AreEqual(limitedReviews[i].hidden, reviewsResultList[i].hidden);
                Assert.AreEqual(limitedReviews[i].deleted, reviewsResultList[i].deleted);
                Assert.AreEqual(limitedReviews[i].dateCreated, reviewsResultList[i].dateCreated);
                Assert.AreEqual(limitedReviews[i].lastUpdated, reviewsResultList[i].lastUpdated);
                Assert.AreEqual(limitedReviews[i].lastUpdatedStaffEmail, reviewsResultList[i].lastUpdatedStaffEmail);
            }
        }

        [TestMethod]
        public async Task GetReviewWithReviewId_ShouldTask()
        {
            var service = new FakeReviewService(_reviews);
            var result = await service.GetReviewAsync(1);

            Assert.IsNotNull(result);
            var reviewsResult = result as ReviewDto;
            Assert.IsNotNull(reviewsResult);
            var targetReview = _reviews.Find(r => r.reviewId == 1);

            Assert.AreEqual(targetReview.productId, reviewsResult.productId);
            Assert.AreEqual(targetReview.userId, reviewsResult.userId);
            Assert.AreEqual(targetReview.userName, reviewsResult.userName);
            Assert.AreEqual(targetReview.reviewRating, reviewsResult.reviewRating);
            Assert.AreEqual(targetReview.reviewContent, reviewsResult.reviewContent);
        }

        [TestMethod]
        public async Task Create_ShouldTask()
        {
            var newReview = new ReviewDto { reviewId = 5, productId = 6, userId = 4, userName = "Mick", reviewRating = 3, reviewContent = "Seen better." };
            var service = new FakeReviewService(_reviews);
            await service.CreateReviewAsync(newReview);

            Assert.IsNotNull(_reviews.Find(r => r.reviewId == 5));
            var targetReview = _reviews.Find(r => r.reviewId == 5);

            Assert.AreEqual(newReview.productId, targetReview.productId);
            Assert.AreEqual(newReview.userId, targetReview.userId);
            Assert.AreEqual(newReview.userName, targetReview.userName);
            Assert.AreEqual(newReview.reviewRating, targetReview.reviewRating);
            Assert.AreEqual(newReview.reviewContent, targetReview.reviewContent);
        }

        [TestMethod]
        public async Task Delete_ShouldTask()
        {
            var newReview = new ReviewDto { reviewId = 5, productId = 6, userId = 4, userName = "Mick", reviewRating = 3, reviewContent = "Seen better." };
            var service = new FakeReviewService(_reviews);
            await service.CreateReviewAsync(newReview);

            Assert.IsNotNull(_reviews.Find(r => r.reviewId == 5));
            var targetReview = _reviews.Find(r => r.reviewId == 5);

            Assert.AreEqual(newReview.productId, targetReview.productId);
            Assert.AreEqual(newReview.userId, targetReview.userId);
            Assert.AreEqual(newReview.userName, targetReview.userName);
            Assert.AreEqual(newReview.reviewRating, targetReview.reviewRating);
            Assert.AreEqual(newReview.reviewContent, targetReview.reviewContent);
        }

        [TestMethod]
        public async Task DoesReviewDtoExist_ShouldReturnTrue()
        {
            var service = new FakeReviewService(_reviews);
            var result = await service.DoesReviewExists(1);

            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task DoesReviewDtoExist_ShouldReturnFalse()
        {
            var service = new FakeReviewService(_reviews);
            var result = await service.DoesReviewExists(15);

            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task GetMeanRatingValidReviews_ShouldReturnDouble()
        {
            var service = new FakeReviewService(_reviews);
            var result = await service.GetMeanRating(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result); ;
        }

        [TestMethod]
        public async Task GetMeanRatingNonExistingReviews_ShouldReturnDouble()
        {
            var service = new FakeReviewService(_reviews);
            var result = await service.GetMeanRating(4);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result); ;
        }


    }
}
