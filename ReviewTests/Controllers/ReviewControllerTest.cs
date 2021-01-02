using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Reviews.Controllers;
using ThAmCo.Reviews.Models;
using ThAmCo.Reviews.Services;

namespace ReviewTests
{
    [TestClass]
    public class ReviewControllerTest
    {
        private readonly List<ReviewDto> _reviews = new List<ReviewDto>
        {
            new ReviewDto {reviewId = 1, productId = 1, userId = 1, userName = "Joe", reviewRating = 5, reviewContent = "Great Product." },
            new ReviewDto {reviewId = 2, productId = 2, userId = 2, userName = "Bob", reviewRating = 3, reviewContent = "It's okay." },
            new ReviewDto {reviewId = 3, productId = 1, userId = 2, userName = "Bob", reviewRating = 1, reviewContent = "Really bad." },
            new ReviewDto {reviewId = 4, productId = 3, userId = 1, userName = "Joe", reviewRating = 3, reviewContent = "It's okay." },
        };

        [TestMethod]
        public async Task GetAllReviews_ShouldOKObject()
        {
            var service = new FakeReviewService(_reviews);
            var controller = new ReviewController(service);
            var result = await controller.GetReviewListAsync(null, null);


            Assert.IsNotNull(result);
            var objResult = result as OkObjectResult;
            Assert.IsNotNull(objResult);
            var reviewsResult = objResult.Value as IEnumerable<ReviewDto>;
            Assert.IsNotNull(reviewsResult);
            var reviewsResultList = reviewsResult.ToList();
            Assert.AreEqual(_reviews.Count, reviewsResultList.Count);
            for (int i = 0; i < _reviews.Count; ++i)
            {
                Assert.AreEqual(_reviews[i].reviewId, reviewsResultList[i].reviewId);
                Assert.AreEqual(_reviews[i].productId, reviewsResultList[i].productId);
                Assert.AreEqual(_reviews[i].userId, reviewsResultList[i].userId);
                Assert.AreEqual(_reviews[i].userName, reviewsResultList[i].userName);
                Assert.AreEqual(_reviews[i].reviewRating, reviewsResultList[i].reviewRating);
                Assert.AreEqual(_reviews[i].reviewContent, reviewsResultList[i].reviewContent);
            }
        }

        [TestMethod]
        public async Task GetAllReviewsForProductId_ShouldOKObject()
        {
            var service = new FakeReviewService(_reviews);
            var controller = new ReviewController(service);
            var result = await controller.GetReviewListAsync(1, null);


            Assert.IsNotNull(result);
            var objResult = result as OkObjectResult;
            Assert.IsNotNull(objResult);
            var reviewsResult = objResult.Value as IEnumerable<ReviewDto>;
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
                Assert.AreEqual(limitedReviews[i].reviewContent, reviewsResultList[i].reviewContent);
            }
        }

        [TestMethod]
        public async Task GetAllReviewsForUserId_ShouldOKObject()
        {
            var service = new FakeReviewService(_reviews);
            var controller = new ReviewController(service);
            var result = await controller.GetReviewListAsync(null, 1);


            Assert.IsNotNull(result);
            var objResult = result as OkObjectResult;
            Assert.IsNotNull(objResult);
            var reviewsResult = objResult.Value as IEnumerable<ReviewDto>;
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
                Assert.AreEqual(limitedReviews[i].reviewContent, reviewsResultList[i].reviewContent);
            }
        }

        [TestMethod]
        public async Task GetReviewWithReviewId_ShouldOKObject()
        {
            var service = new FakeReviewService(_reviews);
            var controller = new ReviewController(service);
            var result = await controller.GetReviewAsync(1);


            Assert.IsNotNull(result);
            var objResult = result as OkObjectResult;
            Assert.IsNotNull(objResult);
            var reviewsResult = objResult.Value as ReviewDto;
            Assert.IsNotNull(reviewsResult);

            var targetReview = _reviews.Find(r => r.reviewId == 1);

            Assert.AreEqual(targetReview.productId, reviewsResult.productId);
            Assert.AreEqual(targetReview.userId, reviewsResult.userId);
            Assert.AreEqual(targetReview.userName, reviewsResult.userName);
            Assert.AreEqual(targetReview.reviewRating, reviewsResult.reviewRating);
            Assert.AreEqual(targetReview.reviewContent, reviewsResult.reviewContent);
        }

        public async Task Create_ShouldOKObject()
        { 
            var service = new FakeReviewService(_reviews);
            var controller = new ReviewController(service);
            var result = await controller.GetReviewAsync(1);


            Assert.IsNotNull(result);
            var objResult = result as OkObjectResult;
            Assert.IsNotNull(objResult);
            var reviewsResult = objResult.Value as ReviewDto;
            Assert.IsNotNull(reviewsResult);

            var targetReview = _reviews.Find(r => r.reviewId == 1);

            Assert.AreEqual(targetReview.productId, reviewsResult.productId);
            Assert.AreEqual(targetReview.userId, reviewsResult.userId);
            Assert.AreEqual(targetReview.userName, reviewsResult.userName);
            Assert.AreEqual(targetReview.reviewRating, reviewsResult.reviewRating);
            Assert.AreEqual(targetReview.reviewContent, reviewsResult.reviewContent);
        }
    }
}
