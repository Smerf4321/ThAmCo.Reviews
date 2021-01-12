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
        private readonly List<Review> testReviews = new List<Review>
        {
            new Review {reviewId = 1, productId = 1, userId = 1, userName = "Dimitri 'Not-Russian-Bot' Ivanov", reviewRating = 5, reviewContent = "Great Product. You can believe me, I'm not a bot.", hidden = false, deleted = false, dateCreated = DateTime.UtcNow, lastUpdated = DateTime.UtcNow, lastUpdatedStaffEmail = "fakeuser@fake.user" },
            new Review {reviewId = 2, productId = 1, userId = 2, userName = "Joe Angry", reviewRating = 3, reviewContent = "It's an okay plunger. I expected more.", hidden = false, deleted = false, dateCreated = DateTime.UtcNow, lastUpdated = DateTime.UtcNow, lastUpdatedStaffEmail = "fakeuser@fake.user" },
            new Review {reviewId = 3, productId = 4, userId = 1, userName = "Dimitri 'Not-Russian-Bot' Ivanov", reviewRating = 4, reviewContent = "Good hardbass, although lacking the newest song from Dj Put-in", hidden = false, deleted = false, dateCreated = DateTime.UtcNow, lastUpdated = DateTime.UtcNow, lastUpdatedStaffEmail = "fakeuser@fake.user" },
            new Review {reviewId = 4, productId = 2, userId = 3, userName = "Bob", reviewRating = 5, reviewContent = "Great quality", hidden = false, deleted = false, dateCreated = DateTime.UtcNow, lastUpdated = DateTime.UtcNow, lastUpdatedStaffEmail = "fakeuser@fake.user" },
            new Review {reviewId = 5, productId = 5, userId = 4, userName = "Adam", reviewRating = 1, reviewContent = "Just no", hidden = true, deleted = true, dateCreated = DateTime.UtcNow, lastUpdated = DateTime.UtcNow, lastUpdatedStaffEmail = "fakeuser@fake.user" }
        };

        [TestMethod]
        public async Task GetAllReviews_Positive()
        {
            var service = new FakeReviewService(testReviews);
            var result = await service.GetReviewListAsync(null, null, false, false);
            Assert.IsNotNull(result);
            
            var reviewsResult = result as IEnumerable<ReviewDto>;
            Assert.IsNotNull(reviewsResult);

            var resultList = reviewsResult.ToList();
            var reviewCount = service._reviews.Where(r => r.hidden == false && r.deleted == false).Count();
            Assert.AreEqual(reviewCount, resultList.Count());

            for (int i = 0; i < resultList.Count(); ++i)
            {
                Assert.AreEqual(service._reviews[i].reviewId, resultList[i].reviewId);
                Assert.AreEqual(service._reviews[i].productId, resultList[i].productId);
                Assert.AreEqual(service._reviews[i].userId, resultList[i].userId);
                Assert.AreEqual(service._reviews[i].userName, resultList[i].userName);
                Assert.AreEqual(service._reviews[i].reviewRating, resultList[i].reviewRating);
            }
        }

        [TestMethod]
        public async Task GetAllReviewsForProductId_ShouldTask()
        {
            var service = new FakeReviewService(testReviews);
            var result = await service.GetReviewListAsync(1, null, false, false);
            Assert.IsNotNull(result);

            var reviewsResult = result as IEnumerable<ReviewDto>;
            Assert.IsNotNull(reviewsResult);
            Assert.IsTrue(reviewsResult.All(r => r.productId == 1));

            var reviewsList = reviewsResult.ToList();
            var limitedReviews = service._reviews.FindAll(r => r.hidden == false && r.deleted == false && r.productId == 1);
            Assert.AreEqual(limitedReviews.Count, reviewsList.Count);

            for (int i = 0; i < limitedReviews.Count; ++i)
            {
                Assert.AreEqual(limitedReviews[i].reviewId, reviewsList[i].reviewId);
                Assert.AreEqual(limitedReviews[i].productId, reviewsList[i].productId);
                Assert.AreEqual(limitedReviews[i].userId, reviewsList[i].userId);
                Assert.AreEqual(limitedReviews[i].userName, reviewsList[i].userName);
                Assert.AreEqual(limitedReviews[i].reviewRating, reviewsList[i].reviewRating);
            }
        }

        [TestMethod]
        public async Task GetAllReviewsForProductId_NonExistentProduct()
        {
            var service = new FakeReviewService(testReviews);
            var result = await service.GetReviewListAsync(13, null, false, false);
            Assert.IsNotNull(result);

            var reviewsResult = result as IEnumerable<ReviewDto>;
            Assert.IsNotNull(reviewsResult);
            Assert.IsTrue(reviewsResult.All(r => r.productId == 13));

            var reviewsList = reviewsResult.ToList();
            var limitedReviews = service._reviews.FindAll(r => r.hidden == false && r.deleted == false && r.productId == 13);
            Assert.AreEqual(limitedReviews.Count, reviewsList.Count);

            for (int i = 0; i < limitedReviews.Count; ++i)
            {
                Assert.AreEqual(limitedReviews[i].reviewId, reviewsList[i].reviewId);
                Assert.AreEqual(limitedReviews[i].productId, reviewsList[i].productId);
                Assert.AreEqual(limitedReviews[i].userId, reviewsList[i].userId);
                Assert.AreEqual(limitedReviews[i].userName, reviewsList[i].userName);
                Assert.AreEqual(limitedReviews[i].reviewRating, reviewsList[i].reviewRating);
            }
        }

        [TestMethod]
        public async Task GetAllReviewsForUserId_ShouldTask()
        {
            var service = new FakeReviewService(testReviews);
            var result = await service.GetReviewListAsync(null, 1, false, false);
            Assert.IsNotNull(result);

            var reviewsResult = result as IEnumerable<ReviewDto>;
            Assert.IsNotNull(reviewsResult);
            Assert.IsTrue(reviewsResult.All(r => r.userId == 1));

            var reviewsResultList = reviewsResult.ToList();
            var limitedReviews = service._reviews.FindAll(r => r.hidden == false && r.deleted == false && r.userId == 1);
            Assert.AreEqual(limitedReviews.Count, reviewsResultList.Count);

            for (int i = 0; i < limitedReviews.Count; ++i)
            {
                Assert.AreEqual(limitedReviews[i].reviewId, reviewsResultList[i].reviewId);
                Assert.AreEqual(limitedReviews[i].productId, reviewsResultList[i].productId);
                Assert.AreEqual(limitedReviews[i].userId, reviewsResultList[i].userId);
                Assert.AreEqual(limitedReviews[i].userName, reviewsResultList[i].userName);
                Assert.AreEqual(limitedReviews[i].reviewRating, reviewsResultList[i].reviewRating);
            }
        }

        [TestMethod]
        public async Task GetAllReviewsForUserId_NonExixtentUser()
        {
            var service = new FakeReviewService(testReviews);
            var result = await service.GetReviewListAsync(null, 13, false, false);
            Assert.IsNotNull(result);

            var reviewsResult = result as IEnumerable<ReviewDto>;
            Assert.IsNotNull(reviewsResult);
            Assert.IsTrue(reviewsResult.All(r => r.userId == 13));

            var reviewsResultList = reviewsResult.ToList();
            var limitedReviews = service._reviews.FindAll(r => r.hidden == false && r.deleted == false && r.userId == 13);
            Assert.AreEqual(limitedReviews.Count, reviewsResultList.Count);

            for (int i = 0; i < limitedReviews.Count; ++i)
            {
                Assert.AreEqual(limitedReviews[i].reviewId, reviewsResultList[i].reviewId);
                Assert.AreEqual(limitedReviews[i].productId, reviewsResultList[i].productId);
                Assert.AreEqual(limitedReviews[i].userId, reviewsResultList[i].userId);
                Assert.AreEqual(limitedReviews[i].userName, reviewsResultList[i].userName);
                Assert.AreEqual(limitedReviews[i].reviewRating, reviewsResultList[i].reviewRating);
            }
        }

        [TestMethod]
        public async Task GetAllReviewsForProductIdandUserId_ShouldTask()
        {
            var service = new FakeReviewService(testReviews);
            var result = await service.GetReviewListAsync(1, 1, false, false);
            Assert.IsNotNull(result);

            var reviewsResult = result as IEnumerable<ReviewDto>;
            Assert.IsNotNull(reviewsResult);
            Assert.IsTrue(reviewsResult.All(r => r.userId == 1 && r.productId == 1));

            var reviewsResultList = reviewsResult.ToList();
            var limitedReviews = service._reviews.FindAll(r => r.hidden == false && r.deleted == false &&  r.userId == 1 && r.productId == 1);
            Assert.AreEqual(limitedReviews.Count, reviewsResultList.Count);

            for (int i = 0; i < limitedReviews.Count; ++i)
            {
                Assert.AreEqual(limitedReviews[i].reviewId, reviewsResultList[i].reviewId);
                Assert.AreEqual(limitedReviews[i].productId, reviewsResultList[i].productId);
                Assert.AreEqual(limitedReviews[i].userId, reviewsResultList[i].userId);
                Assert.AreEqual(limitedReviews[i].userName, reviewsResultList[i].userName);
                Assert.AreEqual(limitedReviews[i].reviewRating, reviewsResultList[i].reviewRating);
            }
        }

        [TestMethod]
        public async Task GetAllReviewsForProductIdandUserId_NonExistentIds()
        {
            var service = new FakeReviewService(testReviews);
            var result = await service.GetReviewListAsync(13, 13, false, false);
            Assert.IsNotNull(result);

            var reviewsResult = result as IEnumerable<ReviewDto>;
            Assert.IsNotNull(reviewsResult);
            Assert.IsTrue(reviewsResult.All(r => r.userId == 13 && r.productId == 13));

            var reviewsResultList = reviewsResult.ToList();
            var limitedReviews = service._reviews.FindAll(r => r.hidden == false && r.deleted == false && r.userId == 13 && r.productId == 13);
            Assert.AreEqual(limitedReviews.Count, reviewsResultList.Count);

            for (int i = 0; i < limitedReviews.Count; ++i)
            {
                Assert.AreEqual(limitedReviews[i].reviewId, reviewsResultList[i].reviewId);
                Assert.AreEqual(limitedReviews[i].productId, reviewsResultList[i].productId);
                Assert.AreEqual(limitedReviews[i].userId, reviewsResultList[i].userId);
                Assert.AreEqual(limitedReviews[i].userName, reviewsResultList[i].userName);
                Assert.AreEqual(limitedReviews[i].reviewRating, reviewsResultList[i].reviewRating);
            }
        }

        [TestMethod]
        public async Task GetReviewWithReviewId_ShouldTask()
        {
            var service = new FakeReviewService(testReviews);
            var result = await service.GetReviewAsync(1);

            Assert.IsNotNull(result);
            var reviewsResult = result as ReviewDto;
            Assert.IsNotNull(reviewsResult);
            var targetReview = service._reviews.Find(r => r.reviewId == 1);

            Assert.AreEqual(targetReview.productId, reviewsResult.productId);
            Assert.AreEqual(targetReview.userId, reviewsResult.userId);
            Assert.AreEqual(targetReview.userName, reviewsResult.userName);
            Assert.AreEqual(targetReview.reviewRating, reviewsResult.reviewRating);
            Assert.AreEqual(targetReview.reviewContent, reviewsResult.reviewContent);
        }

        [TestMethod]
        public async Task GetReviewWithReviewId_NonExistentReview()
        {
            var service = new FakeReviewService(testReviews);
            var result = await service.GetReviewAsync(13);

            Assert.IsNull(result);
            var reviewsResult = result as ReviewDto;
            Assert.IsNull(reviewsResult);
        }

        [TestMethod]
        public async Task Create_ShouldTask()
        {
            var newReview = new ReviewDto {userId = 4, productId = 6, userName = "Mick", reviewContent = "Seen better.", reviewRating = 3 };
            var service = new FakeReviewService(testReviews);
            await service.CreateReviewAsync(4, 6, "Mick", "Seen better.", 3);

            Assert.IsNotNull(service._reviews.Find(r => r.reviewId == service._reviews.Count));
            var targetReview = service._reviews.Find(r => r.reviewId == service._reviews.Count);

            Assert.AreEqual(newReview.productId, targetReview.productId);
            Assert.AreEqual(newReview.userId, targetReview.userId);
            Assert.AreEqual(newReview.userName, targetReview.userName);
            Assert.AreEqual(newReview.reviewRating, targetReview.reviewRating);
            Assert.AreEqual(newReview.reviewContent, targetReview.reviewContent);
        }


        [TestMethod]
        public async Task Delete_ShouldTask()
        {
            var service = new FakeReviewService(testReviews);
            await service.DeleteReviewAsync(1, "");

            var targetReview = service._reviews.Find(r => r.reviewId == 1);

            Assert.IsTrue(targetReview.deleted);
        }

        [TestMethod]
        public async Task Delete_NonExistantReview()
        {
            var service = new FakeReviewService(testReviews);
            var reviewListBefore = service._reviews.Where(r => r.hidden == false && r.deleted == true).ToList();
            var reviewCountBefore = reviewListBefore.Count();

            await service.DeleteReviewAsync(13, "");

            var reviewCountAfter = service._reviews.Where(r => r.hidden == false && r.deleted == true).Count();

            Assert.AreEqual(reviewCountBefore, reviewCountAfter);

            for (int i = 0; i < reviewCountAfter; ++i)
            {
                Assert.AreEqual(service._reviews[i].reviewId, reviewListBefore[i].reviewId);
                Assert.AreEqual(service._reviews[i].productId, reviewListBefore[i].productId);
                Assert.AreEqual(service._reviews[i].userId, reviewListBefore[i].userId);
                Assert.AreEqual(service._reviews[i].userName, reviewListBefore[i].userName);
                Assert.AreEqual(service._reviews[i].reviewRating, reviewListBefore[i].reviewRating);
            }
        }


        [TestMethod]
        public async Task DeletePII_ShouldTask()
        {
            var service = new FakeReviewService(testReviews);
            await service.DeleteReviewPIIAsync(1, "");

            var targetReview = service._reviews.First(r => r.reviewId == 1);

            for (int i = 0; i < service._reviews.Count - 1; ++i)
            {
                if (service._reviews[i].userId == 1)
                {
                    Assert.AreEqual("Account Deleted", targetReview.userName);
                    Assert.IsTrue(targetReview.deleted);
                }
            }
        }

        [TestMethod]
        public async Task DeletePII_NonExistantUser()
        {
            var service = new FakeReviewService(testReviews);
            var reviewListBefore = service._reviews.Where(r => r.hidden == false && r.deleted == true).ToList();
            var reviewCountBefore = reviewListBefore.Count();

            await service.DeleteReviewPIIAsync(13, "");

            var reviewCountAfter = service._reviews.Where(r => r.hidden == false && r.deleted == true).Count();

            Assert.AreEqual(reviewCountBefore, reviewCountAfter);

            for (int i = 0; i < reviewCountAfter; ++i)
            {
                Assert.AreEqual(service._reviews[i].reviewId, reviewListBefore[i].reviewId);
                Assert.AreEqual(service._reviews[i].productId, reviewListBefore[i].productId);
                Assert.AreEqual(service._reviews[i].userId, reviewListBefore[i].userId);
                Assert.AreEqual(service._reviews[i].userName, reviewListBefore[i].userName);
                Assert.AreEqual(service._reviews[i].reviewRating, reviewListBefore[i].reviewRating);
            }
        }

        [TestMethod]
        public async Task HideReview_ShouldTask()
        {
            var service = new FakeReviewService(testReviews);
            await service.HideReviewAsync(1, "");

            var targetReview = service._reviews.Find(r => r.reviewId == 1);

            Assert.IsTrue(targetReview.hidden);
        }

        [TestMethod]
        public async Task HideReview_NonExistantReview()
        {
            var service = new FakeReviewService(testReviews);
            var reviewListBefore = service._reviews.Where(r => r.hidden == true && r.deleted == false).ToList();
            var reviewCountBefore = reviewListBefore.Count();

            await service.HideReviewAsync(13, "");

            var reviewCountAfter = service._reviews.Where(r => r.hidden == true && r.deleted == false).Count();
            Assert.AreEqual(reviewCountBefore, reviewCountAfter);

            for (int i = 0; i < reviewCountAfter; ++i)
            {
                Assert.AreEqual(service._reviews[i].reviewId, reviewListBefore[i].reviewId);
                Assert.AreEqual(service._reviews[i].productId, reviewListBefore[i].productId);
                Assert.AreEqual(service._reviews[i].userId, reviewListBefore[i].userId);
                Assert.AreEqual(service._reviews[i].userName, reviewListBefore[i].userName);
                Assert.AreEqual(service._reviews[i].reviewRating, reviewListBefore[i].reviewRating);
            }
        }

        [TestMethod]
        public async Task Edit_ShouldTask()
        {
            var service = new FakeReviewService(testReviews);

            await service.EditReviewAsync(1, "Seen better.", 3);
            
            var targetReview = service._reviews.Find(r => r.reviewId == 1);

            Assert.AreEqual("Seen better.", targetReview.reviewContent);
            Assert.AreEqual(3, targetReview.reviewRating);
        }

        [TestMethod]
        public async Task Edit_NonExistantReview()
        {
            var service = new FakeReviewService(testReviews);
            var reviewListBefore = service._reviews.Where(r => r.hidden == false && r.deleted == false).ToList();
            var reviewCountBefore = reviewListBefore.Count();

            await service.EditReviewAsync(13, "Seen better.", 3);

            var reviewCountAfter = service._reviews.Where(r => r.hidden == false && r.deleted == false).Count();

            Assert.AreEqual(reviewCountBefore, reviewCountAfter);
            for (int i = 0; i < reviewCountAfter; ++i)
            {
                Assert.AreEqual(service._reviews[i].reviewId, reviewListBefore[i].reviewId);
                Assert.AreEqual(service._reviews[i].productId, reviewListBefore[i].productId);
                Assert.AreEqual(service._reviews[i].userId, reviewListBefore[i].userId);
                Assert.AreEqual(service._reviews[i].userName, reviewListBefore[i].userName);
                Assert.AreEqual(service._reviews[i].reviewRating, reviewListBefore[i].reviewRating);
            }
        }

        [TestMethod]
        public async Task RecoverDeletedReview_ShouldTask()
        {
            var service = new FakeReviewService(testReviews);
            await service.RecoverDeletedReviewAsync(5, "");

            var targetReview = service._reviews.Find(r => r.reviewId == 5);

            Assert.IsFalse(targetReview.deleted);
            Assert.AreEqual(targetReview.lastUpdatedStaffEmail, "");
        }

        [TestMethod]
        public async Task RecoverDeletedReview_NonExistentReview()
        {
            var service = new FakeReviewService(testReviews);
            var reviewListBefore = service._reviews.Where(r => r.hidden == false && r.deleted == true).ToList();
            var reviewCountBefore = reviewListBefore.Count();

            await service.RecoverDeletedReviewAsync(51, "");

            var reviewCountAfter = service._reviews.Where(r => r.hidden == false && r.deleted == true).Count();
            Assert.AreEqual(reviewCountBefore, reviewCountAfter);

            for (int i = 0; i < reviewCountAfter; ++i)
            {
                Assert.AreEqual(service._reviews[i].reviewId, reviewListBefore[i].reviewId);
                Assert.AreEqual(service._reviews[i].productId, reviewListBefore[i].productId);
                Assert.AreEqual(service._reviews[i].userId, reviewListBefore[i].userId);
                Assert.AreEqual(service._reviews[i].userName, reviewListBefore[i].userName);
                Assert.AreEqual(service._reviews[i].reviewRating, reviewListBefore[i].reviewRating);
            }
        }

        [TestMethod]
        public async Task RecoverHiddenReview_ShouldTask()
        {
            var service = new FakeReviewService(testReviews);
            await service.RecoverHiddenReviewAsync(5, "");

            var targetReview = service._reviews.Find(r => r.reviewId == 5);

            Assert.IsFalse(targetReview.hidden);
            Assert.AreEqual(targetReview.lastUpdatedStaffEmail, "");
        }

        [TestMethod]
        public async Task RecoverHiddenReview_NonExistentReview()
        {
            var service = new FakeReviewService(testReviews);
            var reviewListBefore = service._reviews.Where(r => r.hidden == true && r.deleted == false).ToList();
            var reviewCountBefore = reviewListBefore.Count();

            await service.RecoverDeletedReviewAsync(51, "");

            var reviewCountAfter = service._reviews.Where(r => r.hidden == true && r.deleted == false).Count();
            Assert.AreEqual(reviewCountBefore, reviewCountAfter);

            for (int i = 0; i < reviewCountAfter; ++i)
            {
                Assert.AreEqual(service._reviews[i].reviewId, reviewListBefore[i].reviewId);
                Assert.AreEqual(service._reviews[i].productId, reviewListBefore[i].productId);
                Assert.AreEqual(service._reviews[i].userId, reviewListBefore[i].userId);
                Assert.AreEqual(service._reviews[i].userName, reviewListBefore[i].userName);
                Assert.AreEqual(service._reviews[i].reviewRating, reviewListBefore[i].reviewRating);
            }
        }

        [TestMethod]
        public async Task DoesReviewDtoExist_ShouldReturnTrue()
        {
            var service = new FakeReviewService(testReviews);
            var result = await service.DoesReviewExists(1);

            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task DoesReviewDtoExist_ShouldReturnFalse()
        {
            var service = new FakeReviewService(testReviews);
            var result = await service.DoesReviewExists(15);

            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task GetMeanRatingValidReviews_ShouldReturnDouble()
        {
            var service = new FakeReviewService(testReviews);
            var result = await service.GetMeanRating(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result); ;
        }

        [TestMethod]
        public async Task GetMeanRatingNonExistingReviews_ShouldReturnDouble()
        {
            var service = new FakeReviewService(testReviews);
            var result = await service.GetMeanRating(18);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result); ;
        }
    }
}
