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
        [TestMethod]
        public async Task GetAllReviews_ShouldOKObject()
        {

        List<ReviewDto> reviews = new List<ReviewDto>
        {
            new ReviewDto {reviewId = 1, productId = 1, userId = 1, userName = "Joe", reviewRating = 5, reviewContent = "Great Product." },
            new ReviewDto {reviewId = 2, productId = 2, userId = 2, userName = "Bob", reviewRating = 3, reviewContent = "It's okay." },
        };

            var repo = new FakeReviewService(reviews);
            var controller = new ReviewController(repo);
            var result = await controller.GetReviewListAsync(null, null);


            Assert.IsNotNull(result);
            var objResult = result as OkObjectResult;
            Assert.IsNotNull(objResult);
            var reviewsResult = objResult.Value as IEnumerable<ReviewDto>;
            Assert.IsNotNull(reviewsResult);
            var reviewsResultList = reviewsResult.ToList();
            Assert.AreEqual(reviews.Count, reviewsResultList.Count);
            for (int i = 0; i < reviews.Count; ++i)
            {
                Assert.AreEqual(reviews[i].reviewId, reviewsResultList[i].reviewId);
                Assert.AreEqual(reviews[i].productId, reviewsResultList[i].productId);
                Assert.AreEqual(reviews[i].userId, reviewsResultList[i].userId);
                Assert.AreEqual(reviews[i].userName, reviewsResultList[i].userName);
                Assert.AreEqual(reviews[i].reviewRating, reviewsResultList[i].reviewRating);
                Assert.AreEqual(reviews[i].reviewContent, reviewsResultList[i].reviewContent);
            }
        }
    }
}
