using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Reviews.Controllers;
using ThAmCo.Reviews.Models;
using ThAmCo.Reviews.Services;
using Moq;

namespace ReviewTests
{
    [TestClass]
    public class ReviewControllerMock
    {
        private readonly List<Review> allTestReviews = new List<Review>
        {
            new Review {reviewId = 1, productId = 1, userId = 1, userName = "Dimitri 'Not-Russian-Bot' Ivanov", reviewRating = 5, reviewContent = "Great Product. You can believe me, I'm not a bot.", hidden = false, deleted = false, dateCreated = DateTime.UtcNow, lastUpdated = DateTime.UtcNow, lastUpdatedStaffEmail = "fakeuser@fake.user" },
            new Review {reviewId = 2, productId = 1, userId = 2, userName = "Joe Angry", reviewRating = 3, reviewContent = "It's an okay plunger. I expected more.", hidden = false, deleted = false, dateCreated = DateTime.UtcNow, lastUpdated = DateTime.UtcNow, lastUpdatedStaffEmail = "fakeuser@fake.user" },
            new Review {reviewId = 3, productId = 4, userId = 1, userName = "Dimitri 'Not-Russian-Bot' Ivanov", reviewRating = 4, reviewContent = "Good hardbass, although lacking the newest song from Dj Put-in", hidden = false, deleted = false, dateCreated = DateTime.UtcNow, lastUpdated = DateTime.UtcNow, lastUpdatedStaffEmail = "fakeuser@fake.user" },
            new Review {reviewId = 4, productId = 2, userId = 3, userName = "Bob", reviewRating = 5, reviewContent = "Great quality", hidden = false, deleted = false, dateCreated = DateTime.UtcNow, lastUpdated = DateTime.UtcNow, lastUpdatedStaffEmail = "fakeuser@fake.user" },
            new Review {reviewId = 5, productId = 5, userId = 4, userName = "Adam", reviewRating = 1, reviewContent = "Just no", hidden = true, deleted = true, dateCreated = DateTime.UtcNow, lastUpdated = DateTime.UtcNow, lastUpdatedStaffEmail = "fakeuser@fake.user" }
        };

        List<ReviewDto> allTestReviewDtos = new List<ReviewDto>
        {
            new ReviewDto {reviewId = 1, productId = 1, userId = 1, userName = "Dimitri 'Not-Russian-Bot' Ivanov", reviewRating = 5, reviewContent = "Great Product. You can believe me, I'm not a bot.", hidden = false},
            new ReviewDto {reviewId = 2, productId = 1, userId = 2, userName = "Joe Angry", reviewRating = 3, reviewContent = "It's an okay plunger. I expected more.", hidden = false},
            new ReviewDto {reviewId = 3, productId = 4, userId = 1, userName = "Dimitri 'Not-Russian-Bot' Ivanov", reviewRating = 4, reviewContent = "Good hardbass, although lacking the newest song from Dj Put-in", hidden = false},
            new ReviewDto {reviewId = 4, productId = 2, userId = 3, userName = "Bob", reviewRating = 5, reviewContent = "Great quality", hidden = false},
            new ReviewDto {reviewId = 5, productId = 5, userId = 4, userName = "Adam", reviewRating = 1, reviewContent = "Just no", hidden = true}
        };

        [TestMethod]
        public async Task GetAllReviews_ShouldOk()
        {
            List<ReviewDto> testReviewDtos = new List<ReviewDto>
            {
                new ReviewDto {reviewId = 1, productId = 1, userId = 1, userName = "Dimitri 'Not-Russian-Bot' Ivanov", reviewRating = 5, reviewContent = "Great Product. You can believe me, I'm not a bot.", hidden = false},
                new ReviewDto {reviewId = 2, productId = 1, userId = 2, userName = "Joe Angry", reviewRating = 3, reviewContent = "It's an okay plunger. I expected more.", hidden = false},
                new ReviewDto {reviewId = 3, productId = 4, userId = 1, userName = "Dimitri 'Not-Russian-Bot' Ivanov", reviewRating = 4, reviewContent = "Good hardbass, although lacking the newest song from Dj Put-in", hidden = false},
                new ReviewDto {reviewId = 4, productId = 2, userId = 3, userName = "Bob", reviewRating = 5, reviewContent = "Great quality", hidden = false}
            };

            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.GetReviewListAsync(null, null, false, false)).ReturnsAsync(testReviewDtos).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.GetReviewListAsync(null, null);
            Assert.IsNotNull(result);

            var controllerResult = result as OkObjectResult;
            Assert.IsNotNull(controllerResult);

            var reviewsResult = controllerResult.Value as IEnumerable<ReviewDto>;
            Assert.IsNotNull(reviewsResult);

            var resultList = reviewsResult.ToList();
            var reviewCount = testReviewDtos.Count();
            Assert.AreEqual(reviewCount, resultList.Count());

            for (int i = 0; i < resultList.Count(); ++i)
            {
                Assert.AreEqual(testReviewDtos[i].reviewId, resultList[i].reviewId);
                Assert.AreEqual(testReviewDtos[i].productId, resultList[i].productId);
                Assert.AreEqual(testReviewDtos[i].userId, resultList[i].userId);
                Assert.AreEqual(testReviewDtos[i].userName, resultList[i].userName);
                Assert.AreEqual(testReviewDtos[i].reviewRating, resultList[i].reviewRating);
            }

            mock.Verify();
            mock.Verify(repo => repo.GetReviewListAsync(null, null, false, false), Times.Once);
        }

        [TestMethod]
        public async Task GetAllReviewsForProductId_ShouldOk()
        {
            List<ReviewDto> testReviewDtos = new List<ReviewDto>
            {
                new ReviewDto {reviewId = 1, productId = 1, userId = 1, userName = "Dimitri 'Not-Russian-Bot' Ivanov", reviewRating = 5, reviewContent = "Great Product. You can believe me, I'm not a bot.", hidden = false},
                new ReviewDto {reviewId = 2, productId = 1, userId = 2, userName = "Joe Angry", reviewRating = 3, reviewContent = "It's an okay plunger. I expected more.", hidden = false}
            };

            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.GetReviewListAsync(1, null, false, false)).ReturnsAsync(testReviewDtos).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.GetReviewListAsync(1, null);
            Assert.IsNotNull(result);

            var controllerResult = result as OkObjectResult;
            Assert.IsNotNull(controllerResult);

            var reviewsResult = controllerResult.Value as IEnumerable<ReviewDto>;
            Assert.IsNotNull(reviewsResult);
            Assert.IsTrue(reviewsResult.All(r => r.productId == 1));

            var reviewsList = reviewsResult.ToList();
            Assert.AreEqual(testReviewDtos.Count, reviewsList.Count);

            for (int i = 0; i < testReviewDtos.Count; ++i)
            {
                Assert.AreEqual(testReviewDtos[i].reviewId, reviewsList[i].reviewId);
                Assert.AreEqual(testReviewDtos[i].productId, reviewsList[i].productId);
                Assert.AreEqual(testReviewDtos[i].userId, reviewsList[i].userId);
                Assert.AreEqual(testReviewDtos[i].userName, reviewsList[i].userName);
                Assert.AreEqual(testReviewDtos[i].reviewRating, reviewsList[i].reviewRating);
            }

            mock.Verify();
            mock.Verify(repo => repo.GetReviewListAsync(1, null, false, false), Times.Once);
        }

        [TestMethod]
        public async Task GetAllReviewsForProductId_ShouldNotFound()
        {
            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.GetReviewListAsync(10, null, false, false)).ReturnsAsync(null as List<ReviewDto>).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.GetReviewListAsync(10, null);
            Assert.IsNotNull(result);

            var controllerResult = result as NotFoundResult;
            Assert.IsNotNull(controllerResult);

            mock.Verify();
            mock.Verify(repo => repo.GetReviewListAsync(10, null, false, false), Times.Once);
        }

        [TestMethod]
        public async Task GetAllReviewsForUserId_ShouldOk()
        {
            List<ReviewDto> testReviewDtos = new List<ReviewDto>
            {
                new ReviewDto {reviewId = 1, productId = 1, userId = 1, userName = "Dimitri 'Not-Russian-Bot' Ivanov", reviewRating = 5, reviewContent = "Great Product. You can believe me, I'm not a bot.", hidden = false},
                new ReviewDto {reviewId = 3, productId = 4, userId = 1, userName = "Dimitri 'Not-Russian-Bot' Ivanov", reviewRating = 4, reviewContent = "Good hardbass, although lacking the newest song from Dj Put-in", hidden = false}
            };

            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.GetReviewListAsync(null, 1, false, false)).ReturnsAsync(testReviewDtos).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.GetReviewListAsync(null, 1);
            Assert.IsNotNull(result);

            var controllerResult = result as OkObjectResult;
            Assert.IsNotNull(controllerResult);

            var reviewsResult = controllerResult.Value as IEnumerable<ReviewDto>;
            Assert.IsNotNull(reviewsResult);
            Assert.IsTrue(reviewsResult.All(r => r.userId == 1));

            var reviewsList = reviewsResult.ToList();
            Assert.AreEqual(testReviewDtos.Count, reviewsList.Count);

            for (int i = 0; i < testReviewDtos.Count; ++i)
            {
                Assert.AreEqual(testReviewDtos[i].reviewId, reviewsList[i].reviewId);
                Assert.AreEqual(testReviewDtos[i].productId, reviewsList[i].productId);
                Assert.AreEqual(testReviewDtos[i].userId, reviewsList[i].userId);
                Assert.AreEqual(testReviewDtos[i].userName, reviewsList[i].userName);
                Assert.AreEqual(testReviewDtos[i].reviewRating, reviewsList[i].reviewRating);
            }

            mock.Verify();
            mock.Verify(repo => repo.GetReviewListAsync(null, 1, false, false), Times.Once);
        }

        [TestMethod]
        public async Task GetAllReviewsForUserId_ShouldNotFound()
        {
            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.GetReviewListAsync(null, 10, false, false)).ReturnsAsync(null as List<ReviewDto>).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.GetReviewListAsync(null, 10);
            Assert.IsNotNull(result);

            var controllerResult = result as NotFoundResult;
            Assert.IsNotNull(controllerResult);

            mock.Verify();
            mock.Verify(repo => repo.GetReviewListAsync(null, 10, false, false), Times.Once);
        }

        [TestMethod]
        public async Task GetAllReviewsForProductIdandUserId_ShouldOk()
        {
            List<ReviewDto> testReviewDtos = new List<ReviewDto>
            {
                new ReviewDto {reviewId = 1, productId = 1, userId = 1, userName = "Dimitri 'Not-Russian-Bot' Ivanov", reviewRating = 5, reviewContent = "Great Product. You can believe me, I'm not a bot.", hidden = false}
            };

            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.GetReviewListAsync(1, 1, false, false)).ReturnsAsync(testReviewDtos).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.GetReviewListAsync(1, 1);
            Assert.IsNotNull(result);

            var controllerResult = result as OkObjectResult;
            Assert.IsNotNull(controllerResult);

            var reviewsResult = controllerResult.Value as IEnumerable<ReviewDto>;
            Assert.IsNotNull(reviewsResult);
            Assert.IsTrue(reviewsResult.All(r => r.userId == 1 && r.productId == 1));

            var reviewsList = reviewsResult.ToList();
            Assert.AreEqual(testReviewDtos.Count, reviewsList.Count);

            for (int i = 0; i < testReviewDtos.Count; ++i)
            {
                Assert.AreEqual(testReviewDtos[i].reviewId, reviewsList[i].reviewId);
                Assert.AreEqual(testReviewDtos[i].productId, reviewsList[i].productId);
                Assert.AreEqual(testReviewDtos[i].userId, reviewsList[i].userId);
                Assert.AreEqual(testReviewDtos[i].userName, reviewsList[i].userName);
                Assert.AreEqual(testReviewDtos[i].reviewRating, reviewsList[i].reviewRating);
            }

            mock.Verify();
            mock.Verify(repo => repo.GetReviewListAsync(1, 1, false, false), Times.Once);
        }

        [TestMethod]
        public async Task GetAllReviewsForProductIdandUserId_ShouldNotFound()
        {

            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.GetReviewListAsync(10, 10, false, false)).ReturnsAsync(null as List<ReviewDto>).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.GetReviewListAsync(10, 10);
            Assert.IsNotNull(result);

            var controllerResult = result as NotFoundResult;
            Assert.IsNotNull(controllerResult);

            mock.Verify();
            mock.Verify(repo => repo.GetReviewListAsync(10, 10, false, false), Times.Once);
        }

        [TestMethod]
        public async Task GetHiddenReviews_ShouldOk()
        {
            List<ReviewDto> testReviewDtos = new List<ReviewDto>
            {
                new ReviewDto {reviewId = 5, productId = 5, userId = 4, userName = "Adam", reviewRating = 1, reviewContent = "Just no", hidden = true}
            };

            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.GetReviewListAsync(null, null, true, false)).ReturnsAsync(testReviewDtos).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.GetHiddenReviewListAsync(null, null);
            Assert.IsNotNull(result);

            var controllerResult = result as OkObjectResult;
            Assert.IsNotNull(controllerResult);

            var reviewsResult = controllerResult.Value as IEnumerable<ReviewDto>;
            Assert.IsNotNull(reviewsResult);

            var resultList = reviewsResult.ToList();
            var reviewCount = testReviewDtos.Count();
            Assert.AreEqual(reviewCount, resultList.Count());

            for (int i = 0; i < resultList.Count(); ++i)
            {
                Assert.AreEqual(testReviewDtos[i].reviewId, resultList[i].reviewId);
                Assert.AreEqual(testReviewDtos[i].productId, resultList[i].productId);
                Assert.AreEqual(testReviewDtos[i].userId, resultList[i].userId);
                Assert.AreEqual(testReviewDtos[i].userName, resultList[i].userName);
                Assert.AreEqual(testReviewDtos[i].reviewRating, resultList[i].reviewRating);
            }

            mock.Verify();
            mock.Verify(repo => repo.GetReviewListAsync(null, null, true, false), Times.Once);
        }

        [TestMethod]
        public async Task GetHiddenReviewsForProductId_ShouldOk()
        {
            List<ReviewDto> testReviewDtos = new List<ReviewDto>
            {
                new ReviewDto {reviewId = 5, productId = 5, userId = 4, userName = "Adam", reviewRating = 1, reviewContent = "Just no", hidden = true}
            };

            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.GetReviewListAsync(5, null, true, false)).ReturnsAsync(testReviewDtos).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.GetHiddenReviewListAsync(5, null);
            Assert.IsNotNull(result);

            var controllerResult = result as OkObjectResult;
            Assert.IsNotNull(controllerResult);

            var reviewsResult = controllerResult.Value as IEnumerable<ReviewDto>;
            Assert.IsNotNull(reviewsResult);
            Assert.IsTrue(reviewsResult.All(r => r.productId == 5));

            var reviewsList = reviewsResult.ToList();
            Assert.AreEqual(testReviewDtos.Count, reviewsList.Count);

            for (int i = 0; i < testReviewDtos.Count; ++i)
            {
                Assert.AreEqual(testReviewDtos[i].reviewId, reviewsList[i].reviewId);
                Assert.AreEqual(testReviewDtos[i].productId, reviewsList[i].productId);
                Assert.AreEqual(testReviewDtos[i].userId, reviewsList[i].userId);
                Assert.AreEqual(testReviewDtos[i].userName, reviewsList[i].userName);
                Assert.AreEqual(testReviewDtos[i].reviewRating, reviewsList[i].reviewRating);
            }

            mock.Verify();
            mock.Verify(repo => repo.GetReviewListAsync(5, null, true, false), Times.Once);
        }

        [TestMethod]
        public async Task GetHiddenReviewsForProductId_ShouldNotFound()
        {
            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.GetReviewListAsync(10, null, true, false)).ReturnsAsync(null as List<ReviewDto>).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.GetHiddenReviewListAsync(10, null);
            Assert.IsNotNull(result);

            var controllerResult = result as NotFoundResult;
            Assert.IsNotNull(controllerResult);

            mock.Verify();
            mock.Verify(repo => repo.GetReviewListAsync(10, null, true, false), Times.Once);
        }

        [TestMethod]
        public async Task GetHiddenReviewsForUserId_ShouldOk()
        {
            List<ReviewDto> testReviewDtos = new List<ReviewDto>
            {
                new ReviewDto {reviewId = 5, productId = 5, userId = 4, userName = "Adam", reviewRating = 1, reviewContent = "Just no", hidden = true}
            };

            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.GetReviewListAsync(null, 4, true, false)).ReturnsAsync(testReviewDtos).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.GetHiddenReviewListAsync(null, 4);
            Assert.IsNotNull(result);

            var controllerResult = result as OkObjectResult;
            Assert.IsNotNull(controllerResult);

            var reviewsResult = controllerResult.Value as IEnumerable<ReviewDto>;
            Assert.IsNotNull(reviewsResult);
            Assert.IsTrue(reviewsResult.All(r => r.userId == 4));

            var reviewsList = reviewsResult.ToList();
            Assert.AreEqual(testReviewDtos.Count, reviewsList.Count);

            for (int i = 0; i < testReviewDtos.Count; ++i)
            {
                Assert.AreEqual(testReviewDtos[i].reviewId, reviewsList[i].reviewId);
                Assert.AreEqual(testReviewDtos[i].productId, reviewsList[i].productId);
                Assert.AreEqual(testReviewDtos[i].userId, reviewsList[i].userId);
                Assert.AreEqual(testReviewDtos[i].userName, reviewsList[i].userName);
                Assert.AreEqual(testReviewDtos[i].reviewRating, reviewsList[i].reviewRating);
            }

            mock.Verify();
            mock.Verify(repo => repo.GetReviewListAsync(null, 4, true, false), Times.Once);
        }

        [TestMethod]
        public async Task GetHiddenReviewsForUserId_ShouldNotFound()
        {
            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.GetReviewListAsync(null, 10, true, false)).ReturnsAsync(null as List<ReviewDto>).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.GetHiddenReviewListAsync(null, 10);
            Assert.IsNotNull(result);

            var controllerResult = result as NotFoundResult;
            Assert.IsNotNull(controllerResult);

            mock.Verify();
            mock.Verify(repo => repo.GetReviewListAsync(null, 10, true, false), Times.Once);
        }

        [TestMethod]
        public async Task GetHiddenReviewsForProductIdandUserId_ShouldOk()
        {
            List<ReviewDto> testReviewDtos = new List<ReviewDto>
            {
                new ReviewDto {reviewId = 5, productId = 5, userId = 4, userName = "Adam", reviewRating = 1, reviewContent = "Just no", hidden = true}
            };

            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.GetReviewListAsync(5, 4, true, false)).ReturnsAsync(testReviewDtos).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.GetHiddenReviewListAsync(5, 4);
            Assert.IsNotNull(result);

            var controllerResult = result as OkObjectResult;
            Assert.IsNotNull(controllerResult);

            var reviewsResult = controllerResult.Value as IEnumerable<ReviewDto>;
            Assert.IsNotNull(reviewsResult);
            Assert.IsTrue(reviewsResult.All(r => r.userId == 4 && r.productId == 5));

            var reviewsList = reviewsResult.ToList();
            Assert.AreEqual(testReviewDtos.Count, reviewsList.Count);

            for (int i = 0; i < testReviewDtos.Count; ++i)
            {
                Assert.AreEqual(testReviewDtos[i].reviewId, reviewsList[i].reviewId);
                Assert.AreEqual(testReviewDtos[i].productId, reviewsList[i].productId);
                Assert.AreEqual(testReviewDtos[i].userId, reviewsList[i].userId);
                Assert.AreEqual(testReviewDtos[i].userName, reviewsList[i].userName);
                Assert.AreEqual(testReviewDtos[i].reviewRating, reviewsList[i].reviewRating);
            }

            mock.Verify();
            mock.Verify(repo => repo.GetReviewListAsync(5, 4, true, false), Times.Once);
        }

        [TestMethod]
        public async Task GetHiddenReviewsForProductIdandUserId_ShouldNotFound()
        {

            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.GetReviewListAsync(10, 10, true, false)).ReturnsAsync(null as List<ReviewDto>).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.GetHiddenReviewListAsync(10, 10);
            Assert.IsNotNull(result);

            var controllerResult = result as NotFoundResult;
            Assert.IsNotNull(controllerResult);

            mock.Verify();
            mock.Verify(repo => repo.GetReviewListAsync(10, 10, true, false), Times.Once);
        }

        [TestMethod]
        public async Task GetDeletedReviews_ShouldOk()
        {
            List<ReviewDto> testReviewDtos = new List<ReviewDto>
            {
                new ReviewDto {reviewId = 5, productId = 5, userId = 4, userName = "Adam", reviewRating = 1, reviewContent = "Just no", hidden = true}
            };

            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.GetReviewListAsync(null, null, false, true)).ReturnsAsync(testReviewDtos).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.GetDeletedReviewListAsync(null, null);
            Assert.IsNotNull(result);

            var controllerResult = result as OkObjectResult;
            Assert.IsNotNull(controllerResult);

            var reviewsResult = controllerResult.Value as IEnumerable<ReviewDto>;
            Assert.IsNotNull(reviewsResult);

            var resultList = reviewsResult.ToList();
            var reviewCount = testReviewDtos.Count();
            Assert.AreEqual(reviewCount, resultList.Count());

            for (int i = 0; i < resultList.Count(); ++i)
            {
                Assert.AreEqual(testReviewDtos[i].reviewId, resultList[i].reviewId);
                Assert.AreEqual(testReviewDtos[i].productId, resultList[i].productId);
                Assert.AreEqual(testReviewDtos[i].userId, resultList[i].userId);
                Assert.AreEqual(testReviewDtos[i].userName, resultList[i].userName);
                Assert.AreEqual(testReviewDtos[i].reviewRating, resultList[i].reviewRating);
            }

            mock.Verify();
            mock.Verify(repo => repo.GetReviewListAsync(null, null, false, true), Times.Once);
        }

        [TestMethod]
        public async Task GetDeletedReviewsForProductId_ShouldOk()
        {
            List<ReviewDto> testReviewDtos = new List<ReviewDto>
            {
                new ReviewDto {reviewId = 5, productId = 5, userId = 4, userName = "Adam", reviewRating = 1, reviewContent = "Just no", hidden = true}
            };

            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.GetReviewListAsync(5, null, false, true)).ReturnsAsync(testReviewDtos).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.GetDeletedReviewListAsync(5, null);
            Assert.IsNotNull(result);

            var controllerResult = result as OkObjectResult;
            Assert.IsNotNull(controllerResult);

            var reviewsResult = controllerResult.Value as IEnumerable<ReviewDto>;
            Assert.IsNotNull(reviewsResult);
            Assert.IsTrue(reviewsResult.All(r => r.productId == 5));

            var reviewsList = reviewsResult.ToList();
            Assert.AreEqual(testReviewDtos.Count, reviewsList.Count);

            for (int i = 0; i < testReviewDtos.Count; ++i)
            {
                Assert.AreEqual(testReviewDtos[i].reviewId, reviewsList[i].reviewId);
                Assert.AreEqual(testReviewDtos[i].productId, reviewsList[i].productId);
                Assert.AreEqual(testReviewDtos[i].userId, reviewsList[i].userId);
                Assert.AreEqual(testReviewDtos[i].userName, reviewsList[i].userName);
                Assert.AreEqual(testReviewDtos[i].reviewRating, reviewsList[i].reviewRating);
            }

            mock.Verify();
            mock.Verify(repo => repo.GetReviewListAsync(5, null, false, true), Times.Once);
        }

        [TestMethod]
        public async Task GetDeletedReviewsForProductId_ShouldNotFound()
        {
            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.GetReviewListAsync(10, null, false, true)).ReturnsAsync(null as List<ReviewDto>).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.GetDeletedReviewListAsync(10, null);
            Assert.IsNotNull(result);

            var controllerResult = result as NotFoundResult;
            Assert.IsNotNull(controllerResult);

            mock.Verify();
            mock.Verify(repo => repo.GetReviewListAsync(10, null, false, true), Times.Once);
        }

        [TestMethod]
        public async Task GetDeletedReviewsForUserId_ShouldOk()
        {
            List<ReviewDto> testReviewDtos = new List<ReviewDto>
            {
                new ReviewDto {reviewId = 5, productId = 5, userId = 4, userName = "Adam", reviewRating = 1, reviewContent = "Just no", hidden = true}
            };

            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.GetReviewListAsync(null, 4, false, true)).ReturnsAsync(testReviewDtos).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.GetDeletedReviewListAsync(null, 4);
            Assert.IsNotNull(result);

            var controllerResult = result as OkObjectResult;
            Assert.IsNotNull(controllerResult);

            var reviewsResult = controllerResult.Value as IEnumerable<ReviewDto>;
            Assert.IsNotNull(reviewsResult);
            Assert.IsTrue(reviewsResult.All(r => r.userId == 4));

            var reviewsList = reviewsResult.ToList();
            Assert.AreEqual(testReviewDtos.Count, reviewsList.Count);

            for (int i = 0; i < testReviewDtos.Count; ++i)
            {
                Assert.AreEqual(testReviewDtos[i].reviewId, reviewsList[i].reviewId);
                Assert.AreEqual(testReviewDtos[i].productId, reviewsList[i].productId);
                Assert.AreEqual(testReviewDtos[i].userId, reviewsList[i].userId);
                Assert.AreEqual(testReviewDtos[i].userName, reviewsList[i].userName);
                Assert.AreEqual(testReviewDtos[i].reviewRating, reviewsList[i].reviewRating);
            }

            mock.Verify();
            mock.Verify(repo => repo.GetReviewListAsync(null, 4, false, true), Times.Once);
        }

        [TestMethod]
        public async Task GetDeletedReviewsForUserId_ShouldNotFound()
        {
            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.GetReviewListAsync(null, 10, false, true)).ReturnsAsync(null as List<ReviewDto>).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.GetDeletedReviewListAsync(null, 10);
            Assert.IsNotNull(result);

            var controllerResult = result as NotFoundResult;
            Assert.IsNotNull(controllerResult);

            mock.Verify();
            mock.Verify(repo => repo.GetReviewListAsync(null, 10, false, true), Times.Once);
        }

        [TestMethod]
        public async Task GetDeletedReviewsForProductIdandUserId_ShouldOk()
        {
            List<ReviewDto> testReviewDtos = new List<ReviewDto>
            {
                new ReviewDto {reviewId = 5, productId = 5, userId = 4, userName = "Adam", reviewRating = 1, reviewContent = "Just no", hidden = true}
            };

            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.GetReviewListAsync(5, 4, false, true)).ReturnsAsync(testReviewDtos).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.GetDeletedReviewListAsync(5, 4);
            Assert.IsNotNull(result);

            var controllerResult = result as OkObjectResult;
            Assert.IsNotNull(controllerResult);

            var reviewsResult = controllerResult.Value as IEnumerable<ReviewDto>;
            Assert.IsNotNull(reviewsResult);
            Assert.IsTrue(reviewsResult.All(r => r.userId == 4 && r.productId == 5));

            var reviewsList = reviewsResult.ToList();
            Assert.AreEqual(testReviewDtos.Count, reviewsList.Count);

            for (int i = 0; i < testReviewDtos.Count; ++i)
            {
                Assert.AreEqual(testReviewDtos[i].reviewId, reviewsList[i].reviewId);
                Assert.AreEqual(testReviewDtos[i].productId, reviewsList[i].productId);
                Assert.AreEqual(testReviewDtos[i].userId, reviewsList[i].userId);
                Assert.AreEqual(testReviewDtos[i].userName, reviewsList[i].userName);
                Assert.AreEqual(testReviewDtos[i].reviewRating, reviewsList[i].reviewRating);
            }

            mock.Verify();
            mock.Verify(repo => repo.GetReviewListAsync(5, 4, false, true), Times.Once);
        }

        [TestMethod]
        public async Task GetDeletedReviewsForProductIdandUserId_ShouldNotFound()
        {

            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.GetReviewListAsync(10, 10, false, true)).ReturnsAsync(null as List<ReviewDto>).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.GetDeletedReviewListAsync(10, 10);
            Assert.IsNotNull(result);

            var controllerResult = result as NotFoundResult;
            Assert.IsNotNull(controllerResult);

            mock.Verify();
            mock.Verify(repo => repo.GetReviewListAsync(10, 10, false, true), Times.Once);
        }

        [TestMethod]
        public async Task GetReviewWithReviewId_ShouldOk()
        {
            var testReviewDto = new ReviewDto { reviewId = 1, productId = 1, userId = 1, userName = "Dimitri 'Not-Russian-Bot' Ivanov", reviewRating = 5, reviewContent = "Great Product. You can believe me, I'm not a bot.", hidden = false };

            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.GetReviewAsync(1)).ReturnsAsync(testReviewDto).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.GetReviewAsync(1);

            var controllerResult = result as OkObjectResult;
            Assert.IsNotNull(controllerResult);

            Assert.IsNotNull(controllerResult);
            var reviewsResult = controllerResult.Value as ReviewDto;
            Assert.IsNotNull(reviewsResult);

            Assert.AreEqual(testReviewDto.productId, reviewsResult.productId);
            Assert.AreEqual(testReviewDto.userId, reviewsResult.userId);
            Assert.AreEqual(testReviewDto.userName, reviewsResult.userName);
            Assert.AreEqual(testReviewDto.reviewRating, reviewsResult.reviewRating);
            Assert.AreEqual(testReviewDto.reviewContent, reviewsResult.reviewContent);

            mock.Verify();
            mock.Verify(repo => repo.GetReviewAsync(1), Times.Once);
        }

        //NOT FINISHED
        [TestMethod]
        public async Task GetReviewWithReviewId_ShouldNotFound()
        {
            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.GetReviewAsync(10)).ReturnsAsync(null as ReviewDto).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.GetReviewAsync(10);

            var controllerResult = result as NotFoundResult;
            Assert.IsNotNull(controllerResult);

            mock.Verify();
            mock.Verify(repo => repo.GetReviewAsync(10), Times.Once);
        }

        [TestMethod]
        public async Task Create_ShouldOk()
        {
            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.CreateReviewAsync(4, 6, "Mick", "Seen better.", 3)).Returns(Task.Run(() => { })).Verifiable();
            var controller = new ReviewController(mock.Object);
            
            var result = await controller.CreateReview(4, 6, "Mick", "Seen better.", 3);

            var controllerResult = result as OkResult;
            Assert.IsNotNull(controllerResult);

            mock.Verify();
            mock.Verify(repo => repo.CreateReviewAsync(4, 6, "Mick", "Seen better.", 3), Times.Once);
        }

        [TestMethod]
        public async Task Delete_ShouldOk()
        {
            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.DoesReviewExists(1)).Returns(Task.FromResult(true)).Verifiable();
            mock.Setup(repo => repo.DeleteReviewAsync(1, "")).Returns(Task.Run(() => { })).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.DeleteReview(1);

            var controllerResult = result as OkResult;
            Assert.IsNotNull(controllerResult);

            mock.Verify();
            mock.Verify(repo => repo.DoesReviewExists(1), Times.Once);
            mock.Verify(repo => repo.DeleteReviewAsync(1, ""), Times.Once);
        }

        [TestMethod]
        public async Task Delete_ShouldNotFound()
        {
            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.DoesReviewExists(10)).Returns(Task.FromResult(false)).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.DeleteReview(10);

            var controllerResult = result as NotFoundResult;
            Assert.IsNotNull(controllerResult);

            mock.Verify();
            mock.Verify(repo => repo.DoesReviewExists(10), Times.Once);
        }

        [TestMethod]
        public async Task DeletePII_ShouldOk()
        {
            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.DeleteReviewPIIAsync(1, "")).Returns(Task.Run(() => { })).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.DeletePII(1);

            var controllerResult = result as OkResult;
            Assert.IsNotNull(controllerResult);

            mock.Verify();
            mock.Verify(repo => repo.DeleteReviewPIIAsync(1, ""), Times.Once);
        }

        [TestMethod]
        public async Task DeleteByProduct_ShouldOk()
        {
            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.DeleteReviewByProductAsync(1, "")).Returns(Task.Run(() => { })).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.DeleteByProduct(1);

            var controllerResult = result as OkResult;
            Assert.IsNotNull(controllerResult);

            mock.Verify();
            mock.Verify(repo => repo.DeleteReviewByProductAsync(1, ""), Times.Once);
        }

        [TestMethod]
        public async Task HideReview_ShouldOk()
        {
            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.DoesReviewExists(1)).Returns(Task.FromResult(true)).Verifiable();
            mock.Setup(repo => repo.HideReviewAsync(1, "")).Returns(Task.Run(() => { })).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.HideReview(1);

            var controllerResult = result as OkResult;
            Assert.IsNotNull(controllerResult);

            mock.Verify();
            mock.Verify(repo => repo.DoesReviewExists(1), Times.Once);
            mock.Verify(repo => repo.HideReviewAsync(1, ""), Times.Once);
        }

        [TestMethod]
        public async Task HideReview_ShouldNotFound()
        {
            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.DoesReviewExists(10)).Returns(Task.FromResult(false)).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.HideReview(10);

            var controllerResult = result as NotFoundResult;
            Assert.IsNotNull(controllerResult);

            mock.Verify();
            mock.Verify(repo => repo.DoesReviewExists(10), Times.Once);
        }

        [TestMethod]
        public async Task Edit_ShouldOk()
        {
            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.DoesReviewExists(1)).Returns(Task.FromResult(true)).Verifiable();
            mock.Setup(repo => repo.EditReviewAsync(1, "Seen better.", 3)).Returns(Task.Run(() => { })).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result =  await controller.EditReview(1, "Seen better.", 3);

            var controllerResult = result as OkResult;
            Assert.IsNotNull(controllerResult);

            mock.Verify();
            mock.Verify(repo => repo.DoesReviewExists(1), Times.Once);
            mock.Verify(repo => repo.EditReviewAsync(1, "Seen better.", 3), Times.Once);
        }

        [TestMethod]
        public async Task Edit_ShouldNotFound()
        {
            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.DoesReviewExists(10)).Returns(Task.FromResult(false)).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.EditReview(10, "Seen better.", 3);

            var controllerResult = result as NotFoundResult;
            Assert.IsNotNull(controllerResult);

            mock.Verify();
            mock.Verify(repo => repo.DoesReviewExists(10), Times.Once);
        }

        [TestMethod]
        public async Task RecoverHiddenReview_ShouldTask()
        {
            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.DoesReviewExists(1)).Returns(Task.FromResult(true)).Verifiable();
            mock.Setup(repo => repo.RecoverHiddenReviewAsync(1, "")).Returns(Task.Run(() => { })).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.RecoverHidden(1);

            var controllerResult = result as OkResult;
            Assert.IsNotNull(controllerResult);

            mock.Verify();
            mock.Verify(repo => repo.DoesReviewExists(1), Times.Once);
            mock.Verify(repo => repo.RecoverHiddenReviewAsync(1, ""), Times.Once);
        }

        [TestMethod]
        public async Task RecoverHiddenReview_ShouldNotFound()
        {
            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.DoesReviewExists(10)).Returns(Task.FromResult(false)).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.RecoverHidden(10);

            var controllerResult = result as NotFoundResult;
            Assert.IsNotNull(controllerResult);

            mock.Verify();
            mock.Verify(repo => repo.DoesReviewExists(10), Times.Once);
        }

        [TestMethod]
        public async Task RecoverDeletedReview_ShouldTask()
        {
            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.DoesReviewExists(1)).Returns(Task.FromResult(true)).Verifiable();
            mock.Setup(repo => repo.RecoverDeletedReviewAsync(1, "")).Returns(Task.Run(() => { })).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.RecoverDeleted(1);

            var controllerResult = result as OkResult;
            Assert.IsNotNull(controllerResult);

            mock.Verify();
            mock.Verify(repo => repo.DoesReviewExists(1), Times.Once);
            mock.Verify(repo => repo.RecoverDeletedReviewAsync(1, ""), Times.Once);
        }

        [TestMethod]
        public async Task RecoverDeletedReview_ShouldNotFound()
        {
            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.DoesReviewExists(1)).Returns(Task.FromResult(false)).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.RecoverDeleted(1);

            var controllerResult = result as NotFoundResult;
            Assert.IsNotNull(controllerResult);

            mock.Verify();
            mock.Verify(repo => repo.DoesReviewExists(1), Times.Once);
        }

        [TestMethod]
        public async Task GetMeanRating_ShouldOK()
        {
            var mock = new Mock<IReviewService>(MockBehavior.Strict);
            mock.Setup(repo => repo.GetMeanRating(1)).Returns(Task.FromResult((double)4)).Verifiable();
            var controller = new ReviewController(mock.Object);

            var result = await controller.GetMeanRating(1);

            var controllerResult = result as OkObjectResult;
            Assert.IsNotNull(controllerResult);

            mock.Verify();
            mock.Verify(repo => repo.GetMeanRating(1), Times.Once);
        }
    }
}
